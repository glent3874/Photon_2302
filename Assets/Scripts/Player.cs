using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    #region 欄位
    [SerializeField] List<GameObject> 所有模型 = new List<GameObject>();
    [SerializeField] Text 名稱 = null;
    [SerializeField] Transform 準備好了 = null;

    int skinID = 0;
    bool newPlayer = true;
    #endregion

    #region 事件
    private void Start()
    {
        Lobby.instance.登記入場(this);

        if(photonView.IsMine)
        {
            //Buffered將訊息儲存起來給將來加入的用戶
            //慎用 非常吃流量
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, 0);
            更新名稱();
            PlayerInfoManager.instance.Act_Save += 更新名稱;
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.登記離場(this);
        if(photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= 更新名稱;
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 切換角色模型
    /// </summary>
    public void ChangeSkin()
    {
        if(photonView.IsMine)
        {
            skinID += 1;
            if (skinID >= 所有模型.Count)
            {
                skinID = 0;
            }
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, skinID);
        }
    }

    /// <summary>
    /// 刷新模型
    /// </summary>
    [PunRPC]
    public void RPCUpdateSkin(int _skinID)
    {
        this.skinID = _skinID;
        for (int i = 0; i < 所有模型.Count; i++)
        {
            所有模型[i].SetActive(i == _skinID);  
        }
    }

    public void 更新名稱()
    {
        photonView.RPC("RPCUpdateName", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.nickName);
    }

    [PunRPC]
    public void RPCUpdateName(string _nickName)
    {
        名稱.text = _nickName;
    }

    public bool isReady
    {
        get { return _isReady; }
        set
        {
            if ((newPlayer || _isReady != value) && photonView.IsMine)  
            {
                photonView.RPC("RPCIsReady", RpcTarget.OthersBuffered, value);
                newPlayer = false;
            }

            _isReady = value;
            if (value == true)
            {
                準備好了.localScale = Vector3.one;
            }
            else
            {
                準備好了.localScale = Vector3.zero;
            }

            if (value == true)
            {
                Lobby.instance.確認是否全都準備好();
            }
        }
    }
    bool _isReady = false;

    [PunRPC]
    public void RPCIsReady(bool _isReady)
    {
        this.isReady = _isReady;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (photonView.IsMine) 
        {
            isReady = false;
        }
    }
    #endregion
}