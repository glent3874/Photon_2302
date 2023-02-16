using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SetPlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> �Ҧ��ҫ� = new List<GameObject>();
    [SerializeField] Text �W�� = null;

    int skinID = 0;
    public Animator �ʵe�t�� = null;

    private void Start()
    {
        if (photonView.IsMine)
        {
            //Buffered�N�T���x�s�_�ӵ��N�ӥ[�J���Τ�
            //�V�� �D�`�Y�y�q
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.skin);
            ��s�W��();
            PlayerInfoManager.instance.Act_Save += ��s�W��;
        }
    }

    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= ��s�W��;
        }
    }

    public void ChangeSkin()
    {
        if (photonView.IsMine)
        {
            skinID += 1;
            if (skinID >= �Ҧ��ҫ�.Count)
            {
                skinID = 0;
            }
            PlayerInfoManager.instance.data.skin = skinID;
            PlayerInfoManager.instance.Save();
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
            if (i == _skinID)
            {
                �ʵe�t�� = �Ҧ��ҫ�[i].GetComponent<Animator>();
            }
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
}
