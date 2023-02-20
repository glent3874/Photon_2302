using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/// <summary>
/// ���a���H
/// </summary>
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
        Lobby.instance.�n�O�J��(this);                  //�V�j�U�n�O�ڤJ���F

        //�p�G�ڬO���L �N�ǳƳ]��false
        if(photonView.IsMine)
        {
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.�n�O����(this);                  //��j�U�������}�F
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

    /// <summary>
    /// �ǳƻP�_ �æP�ɳ]�w��L�蹳�����A
    /// </summary>
    public bool isReady
    {
        get { return _isReady; }
        set
        {
            //(�ڬO�s���a �� �ǳƪ��A����) �B �ڬO���L
            //�q����L�蹳���A���ܤF
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
                Lobby.instance.�T�{�O�_�����ǳƦn();         //�Ҧ����a���ǳƴN�}�l�C��
            }
        }
    }
    bool _isReady = false;

    /// <summary>
    /// �]���ڬO�蹳 �N�ǳƳ]�����L��ڻ������A
    /// </summary>
    /// <param name="_isReady"></param>
    [PunRPC]
    public void RPCIsReady(bool _isReady)
    {
        this.isReady = _isReady;
    }

    /// <summary>
    /// �b���󪱮a���}���ɭ� �����ǳƪ��A
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