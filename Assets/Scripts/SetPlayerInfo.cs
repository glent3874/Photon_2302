using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SetPlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> 所有模型 = new List<GameObject>();
    [SerializeField] Text 名稱 = null;

    int skinID = 0;
    public Animator 動畫系統 = null;

    private void Start()
    {
        if (photonView.IsMine)
        {
            //Buffered將訊息儲存起來給將來加入的用戶
            //慎用 非常吃流量
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, PlayerInfoManager.instance.data.skin);
            更新名稱();
            PlayerInfoManager.instance.Act_Save += 更新名稱;
        }
    }

    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            PlayerInfoManager.instance.Act_Save -= 更新名稱;
        }
    }

    public void ChangeSkin()
    {
        if (photonView.IsMine)
        {
            skinID += 1;
            if (skinID >= 所有模型.Count)
            {
                skinID = 0;
            }
            PlayerInfoManager.instance.data.skin = skinID;
            PlayerInfoManager.instance.Save();
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
            if (i == _skinID)
            {
                動畫系統 = 所有模型[i].GetComponent<Animator>();
            }
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
}
