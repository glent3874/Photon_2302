using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks
{
    #region ���
    [SerializeField] List<GameObject> �Ҧ��ҫ� = new List<GameObject>();
    [SerializeField] Text �W�� = null;
    [SerializeField] Transform �ǳƦn�F = null;

    int skinID = 0;
    bool newPlayer = true;
    #endregion

    #region �ƥ�
    private void Start()
    {
        Lobby.instance.�n�O�J��(this);

        if(photonView.IsMine)
        {
            //Buffered�N�T���x�s�_�ӵ��N�ӥ[�J���Τ�
            //�V�� �D�`�Y�y�q
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, 0);
            ��s�W��();
            PlayerInfoManager.instance.Act_Save += ��s�W��;
            isReady = false;
        }
    }

    private void OnDestroy()
    {
        Lobby.instance.�n�O����(this);
        if(photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= ��s�W��;
        }
    }
    #endregion

    #region ��k
    /// <summary>
    /// ��������ҫ�
    /// </summary>
    public void ChangeSkin()
    {
        if(photonView.IsMine)
        {
            skinID += 1;
            if (skinID >= �Ҧ��ҫ�.Count)
            {
                skinID = 0;
            }
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, skinID);
        }
    }

    /// <summary>
    /// ��s�ҫ�
    /// </summary>
    [PunRPC]
    public void RPCUpdateSkin(int _skinID)
    {
        this.skinID = _skinID;
        for (int i = 0; i < �Ҧ��ҫ�.Count; i++)
        {
            �Ҧ��ҫ�[i].SetActive(i == _skinID);  
        }
    }

    public void ��s�W��()
    {
        photonView.RPC("RPCUpdateName", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.nickName);
    }

    [PunRPC]
    public void RPCUpdateName(string _nickName)
    {
        �W��.text = _nickName;
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