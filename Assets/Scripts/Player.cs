using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/// <summary>
/// 玩家本人
/// </summary>
public class Player : MonoBehaviourPunCallbacks
{
    #region 欄位
    [SerializeField] Transform 準備好了 = null;
    [SerializeField] SetPlayerInfo 換衣服程式 = null;

    bool newPlayer = true;
    #endregion

    #region 事件
    private void Start()
    {
        Lobby.instance.登記入場(this);                  //向大廳登記我入場了

        //如果我是本尊 將準備設為false
        if(photonView.IsMine)
        {
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.登記離場(this);                  //跟大廳說我離開了
    }
    #endregion

    #region 方法
    /// <summary>
    /// 切換角色模型
    /// </summary>
    public void ChangeSkin()
    {
        換衣服程式.ChangeSkin();
    }

    /// <summary>
    /// 準備與否 並同時設定其他鏡像的狀態
    /// </summary>
    public bool isReady
    {
        get { return _isReady; }
        set
        {
            //(我是新玩家 或 準備狀態改變) 且 我是本尊
            //通知其他鏡像狀態改變了
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
                Lobby.instance.確認是否全都準備好();         //所有玩家都準備就開始遊戲
            }
        }
    }
    bool _isReady = false;

    /// <summary>
    /// 因為我是鏡像 將準備設為本尊跟我說的狀態
    /// </summary>
    /// <param name="_isReady"></param>
    [PunRPC]
    public void RPCIsReady(bool _isReady)
    {
        this.isReady = _isReady;
    }

    /// <summary>
    /// 在任何玩家離開的時候 取消準備狀態
    /// </summary>
    /// <param name="otherPlayer"></param>
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