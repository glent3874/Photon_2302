using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 大廳
/// 1.連線到機房
/// 2.顯示連到哪裡 顯示速度
/// </summary>
public class Lobby : MonoBehaviourPunCallbacks
{
    #region 單例模式
    public static Lobby instance = null;
    private void Awake()
    {
        instance = this;    
    }
    #endregion

    #region 欄位
    [SerializeField] Text 連線狀態;
    [SerializeField] Transform 加入房間按鈕 = null;
    [SerializeField] Transform 離開房間按鈕 = null;
    [SerializeField] GameObject 開始屏蔽 = null;

    public GameObject mainPlayer = null;
    public Player mainPlayerScript = null;
    List<Player> 玩家列表 = new List<Player>();
    int 準備好的人數 = 0;
    #endregion

    #region 事件
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;        //剛房長同一間房的玩家會自動跟房長一起切場景

        PlayerInfoManager.instance.Load();                  //叫玩家存檔系統讀取玩家資料

        連線狀態.text = "連線中...";

        PhotonNetwork.ConnectUsingSettings();               //利用預設值連接到機房

        加入房間按鈕.localScale = Vector3.zero;              //連線成功前先不讓玩家使用按鈕
        離開房間按鈕.localScale = Vector3.zero;
    }

    /// <summary>
    /// 連線機房成功時會執行
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        連線狀態.text = "連線到: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing();  //連線到: 伺服器國家    Ping:連線速度
        加入房間按鈕.localScale = Vector3.one;                //連機房成功 可以加入房間了
        離開房間按鈕.localScale = Vector3.zero;
    }
    #endregion

    #region 方法
    /// <summary>
    /// 自動尋找房間
    /// </summary>
    public void 自動加入()
    {
        連線狀態.text = "配對中...";

        PhotonNetwork.JoinRandomRoom();                      //加入隨機房間
    }

    /// <summary>
    /// 加入任何房間成功
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        連線狀態.text = "進入了: " + PhotonNetwork.CurrentRoom.Name;       //當前房間的編號

        加入房間按鈕.localScale = Vector3.zero;               //進入房間成功 可以離開房間了
        離開房間按鈕.localScale = Vector3.one;

        //產生自己的角色
        mainPlayer = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity);
        mainPlayerScript = mainPlayer.GetComponent<Player>();       //抓自己角色的腳本
    }

    /// <summary>
    /// 離開房間成功後
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        連線狀態.text = "";
        加入房間按鈕.localScale = Vector3.one;
        離開房間按鈕.localScale = Vector3.zero;
    }

    /// <summary>
    /// 加入隨機房間失敗
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        連線狀態.text = "找不到隨機房間!";

        //自動創建一個房間讓玩家加入
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(Random.Range(100000, 999999).ToString(), roomOptions);         //createRoom 需要給一個房名string
    }

    /// <summary>
    /// 向伺服器發出離開房間請求
    /// </summary>
    public void 離開房間()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 向角色要求更換造型
    /// </summary>
    public void 更換外觀()
    {
        mainPlayerScript.ChangeSkin();
    }

    /// <summary>
    /// 玩家進入房間後 新增至玩家列表並刷新排列
    /// </summary>
    /// <param name="playerIn"></param>
    public void 登記入場(Player playerIn)
    {
        玩家列表.Add(playerIn);
        排列玩家順序();
    }

    /// <summary>
    /// 玩家離開房間後 移除自玩家列表並刷新排列
    /// </summary>
    /// <param name="playerOut"></param>
    public void 登記離場(Player playerOut)
    {
        玩家列表.Remove(playerOut);
        排列玩家順序();
    }

    /// <summary>
    /// 將角色放在指定位置上
    /// </summary>
    void 排列玩家順序()
    {
        for (int i = 0; i < 玩家列表.Count; i++) 
        {
            玩家列表[i].transform.position = new Vector3(i * 2, 0f, 0f);
        }
    }

    /// <summary>
    /// 使角色進入準備狀態並確認是否全部準備
    /// </summary>
    public void 準備好了()
    {
        mainPlayerScript.isReady = true;

        確認是否全都準備好();
    }

    /// <summary>
    /// 如果我是房主就確認所有人的準備狀態
    /// </summary>
    public void 確認是否全都準備好()
    {
        //如果我是房主
        if (PhotonNetwork.IsMasterClient)
        {
            int 準備好的人數 = 0;
            for (int i = 0; i < 玩家列表.Count; i++)
            {
                if (玩家列表[i].isReady == true)
                {
                    準備好的人數++;
                }
            }

            //都準備好了就開始遊戲
            if (準備好的人數 >= 玩家列表.Count)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;                   //關閉房間

                photonView.RPC("RPCStartGame", RpcTarget.AllBuffered);      //通知所有鏡像開始遊戲
            }
        }
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    [PunRPC]
    public void RPCStartGame()
    {
        開始屏蔽.SetActive(true);                   //開啟屏蔽
        Invoke("ToGameplay", 2f);                   //兩秒後切換場景
    }

    /// <summary>
    /// 切換場景
    /// </summary>
    void ToGameplay()
    {
        //如果我是房主就切換場景
        if (PhotonNetwork.IsMasterClient == true) 
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
    #endregion
}
