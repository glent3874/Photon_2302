using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �]�m���a�����
/// </summary>
public class SetPlayerInfo : MonoBehaviourPunCallbacks
{
    #region ���
    [SerializeField] List<GameObject> �Ҧ��ҫ� = new List<GameObject>();
    [SerializeField] Text �W�� = null;

    int skinID = 0;
    public Animator �ʵe�t�� = null;
    #endregion

    #region �ƥ�
    private void Start()
    {
        //�p�G�ڬO���L �ͦ�����ɥs�j�a��s�y���P�W��
        if (photonView.IsMine)
        {
            //Buffered �N�T���x�s�_�ӵ��N�ӥ[�J���Τ�
            //�V�� �D�`�Y�y�q
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.skin);   //�s�j�a��s�ڪ��y��
            ��s�W��();
            PlayerInfoManager.instance.Act_Save += ��s�W��;
        }
    }

    private void OnDestroy()
    {
        //�R������� �p�G�ڬO���L �����q�\�e��
        if (photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= ��s�W��;
        }
    }
    #endregion

    #region ��k
    /// <summary>
    /// �󴫳y��
    /// </summary>
    public void ChangeSkin()
    {
        //�p�G�ڬO���L
        if (photonView.IsMine)
        {
            //���j��
            skinID += 1;
            if (skinID >= �Ҧ��ҫ�.Count)
            {
                skinID = 0;
            }
            PlayerInfoManager.instance.data.skin = skinID;                      //�������a��ܪ��y�����
            PlayerInfoManager.instance.Save();                                  //�s��
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, skinID);     //�q����L�蹳��s�y��
        }
    }
    /// <summary>
    /// ��s�y��
    /// </summary>
    [PunRPC]
    public void RPCUpdateSkin(int _skinID)
    {
        this.skinID = _skinID;                                      //�N���蹳��e���y���]�����L�Ƕi�Ӫ�ID
        for (int i = 0; i < �Ҧ��ҫ�.Count; i++)
        {
            �Ҧ��ҫ�[i].SetActive(i == _skinID);                    //�N�ۦPID���y���}�� ���P������
            if (i == _skinID)
            {
                �ʵe�t�� = �Ҧ��ҫ�[i].GetComponent<Animator>();     //���o��e�y�����ʵe���
            }
        }
    }

    /// <summary>
    /// �q���Ҧ��H��s�W�r
    /// </summary>
    public void ��s�W��()
    {
        photonView.RPC("RPCUpdateName", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.nickName);
    }

    /// <summary>
    /// ��s�W�r
    /// </summary>
    /// <param name="_nickName"></param>
    [PunRPC]
    public void RPCUpdateName(string _nickName)
    {
        �W��.text = _nickName;
    }
    #endregion
}
