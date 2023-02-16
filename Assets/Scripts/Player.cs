using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    #region 逆
    [SerializeField] Transform 非称 = null;
    [SerializeField] SetPlayerInfo 传︾狝祘Α = null;

    bool newPlayer = true;
    #endregion

    #region ㄆン
    private void Start()
    {
        Lobby.instance.祅癘初(this);

        if(photonView.IsMine)
        {
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.祅癘瞒初(this);
    }
    #endregion

    #region よ猭
    /// <summary>
    /// ち传à︹家
    /// </summary>
    public void ChangeSkin()
    {
        传︾狝祘Α.ChangeSkin();
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
                非称.localScale = Vector3.one;
            }
            else
            {
                非称.localScale = Vector3.zero;
            }

            if (value == true)
            {
                Lobby.instance.絋粄琌常非称();
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