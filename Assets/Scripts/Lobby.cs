using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
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

    public GameObject mainPlayer = null;
    public Player mainPlayerScript = null;
    List<Player> 玩家列表 = new List<Player>();
    #endregion

    #region 事件
    private void Start()
    {
        連線狀態.text = "連線中...";

        //利用預設值連接到機房
        PhotonNetwork.ConnectUsingSettings();

        加入房間按鈕.localScale = Vector3.zero;
        離開房間按鈕.localScale = Vector3.zero;
    }

    /// <summary>
    /// 連線機房成功時會執行
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        連線狀態.text = "連線到: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing();
        加入房間按鈕.localScale = Vector3.one;
        離開房間按鈕.localScale = Vector3.zero;
    }
    #endregion

    #region 方法
    public void 自動加入()
    {
        連線狀態.text = "配對中...";
        //加入隨機房間
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// 加入任何房間成功
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        連線狀態.text = "進入了: " + PhotonNetwork.CurrentRoom.Name;

        加入房間按鈕.localScale = Vector3.zero;
        離開房間按鈕.localScale = Vector3.one;

        //建立自己的角色
        mainPlayer = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity);
        mainPlayerScript = mainPlayer.GetComponent<Player>();
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
        //createRoom 需要給一個房名string
        PhotonNetwork.CreateRoom(Random.Range(100000, 999999).ToString(), roomOptions); ;
    }

    /// <summary>
    /// 向伺服器發出離開房間請求
    /// </summary>
    public void 離開房間()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void 更換外觀()
    {
        mainPlayerScript.ChangeSkin();
    }

    public void 登記入場(Player playerIn)
    {
        玩家列表.Add(playerIn);
        排列玩家順序();
    }

    public void 登記離場(Player playerOut)
    {
        玩家列表.Remove(playerOut);
        排列玩家順序();
    }

    void 排列玩家順序()
    {
        for (int i = 0; i < 玩家列表.Count; i++) 
        {
            玩家列表[i].transform.position = new Vector3(i * 2, 0f, 0f);
        }
    }
    #endregion
}
