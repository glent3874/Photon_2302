using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    #region ���
    [SerializeField] Transform �ǳƦn�F = null;
    [SerializeField] SetPlayerInfo ����A�{�� = null;

    bool newPlayer = true;
    #endregion

    #region �ƥ�
    private void Start()
    {
        Lobby.instance.�n�O�J��(this);

        if(photonView.IsMine)
        {
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.�n�O����(this);
    }
    #endregion

    #region ��k
    /// <summary>
    /// ��������ҫ�
    /// </summary>
    public void ChangeSkin()
    {
        ����A�{��.ChangeSkin();
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
                �ǳƦn�F.localScale = Vector3.one;
            }
            else
            {
                �ǳƦn�F.localScale = Vector3.zero;
            }

            if (value == true)
            {
                Lobby.instance.�T�{�O�_�����ǳƦn();
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