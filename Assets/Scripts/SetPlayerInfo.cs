using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 設置玩家的資料
/// </summary>
public class SetPlayerInfo : MonoBehaviourPunCallbacks
{
    #region 欄位
    [SerializeField] List<GameObject> 所有模型 = new List<GameObject>();
    [SerializeField] Text 名稱 = null;

    int skinID = 0;
    public Animator 動畫系統 = null;
    #endregion

    #region 事件
    private void Start()
    {
        //如果我是本尊 生成角色時叫大家更新造型與名稱
        if (photonView.IsMine)
        {
            //Buffered 將訊息儲存起來給將來加入的用戶
            //慎用 非常吃流量
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.skin);   //叫大家更新我的造型
            更新名稱();
            PlayerInfoManager.instance.Act_Save += 更新名稱;
        }
    }

    private void OnDestroy()
    {
        //摧毀角色時 如果我是本尊 取消訂閱委派
        if (photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= 更新名稱;
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 更換造型
    /// </summary>
    public void ChangeSkin()
    {
        //如果我是本尊
        if (photonView.IsMine)
        {
            //選單迴圈
            skinID += 1;
            if (skinID >= 所有模型.Count)
            {
                skinID = 0;
            }
            PlayerInfoManager.instance.data.skin = skinID;                      //紀錄玩家選擇的造型資料
            PlayerInfoManager.instance.Save();                                  //存檔
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, skinID);     //通知其他鏡像刷新造型
        }
    }
    /// <summary>
    /// 刷新造型
    /// </summary>
    [PunRPC]
    public void RPCUpdateSkin(int _skinID)
    {
        this.skinID = _skinID;                                      //將此鏡像當前的造型設為本尊傳進來的ID
        for (int i = 0; i < 所有模型.Count; i++)
        {
            所有模型[i].SetActive(i == _skinID);                    //將相同ID的造型開啟 不同的關閉
            if (i == _skinID)
            {
                動畫系統 = 所有模型[i].GetComponent<Animator>();     //取得當前造型的動畫控制器
            }
        }
    }

    /// <summary>
    /// 通知所有人更新名字
    /// </summary>
    public void 更新名稱()
    {
        photonView.RPC("RPCUpdateName", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.nickName);
    }

    /// <summary>
    /// 更新名字
    /// </summary>
    /// <param name="_nickName"></param>
    [PunRPC]
    public void RPCUpdateName(string _nickName)
    {
        名稱.text = _nickName;
    }
    #endregion
}
