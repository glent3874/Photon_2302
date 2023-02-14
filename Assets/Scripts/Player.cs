using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{
    #region ���
    [SerializeField] List<GameObject> �Ҧ��ҫ� = new List<GameObject>();

    int skinID = 0;
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
    #endregion
}