using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{
    #region 逆
    [SerializeField] List<GameObject> ┮Τ家 = new List<GameObject>();

    int skinID = 0;
    #endregion

    #region ㄆン
    private void Start()
    {
        Lobby.instance.祅癘初(this);

        if(photonView.IsMine)
        {
            //Buffered盢癟纗癬ㄓ倒盢ㄓノめ
            //稸ノ 獶盽瑈秖
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, 0);
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
        if(photonView.IsMine)
        {
            skinID += 1;
            if (skinID >= ┮Τ家.Count)
            {
                skinID = 0;
            }
            photonView.RPC("RPCUpdateSkin", RpcTarget.AllBuffered, skinID);
        }
    }

    /// <summary>
    /// 穝家
    /// </summary>
    [PunRPC]
    public void RPCUpdateSkin(int _skinID)
    {
        this.skinID = _skinID;
        for (int i = 0; i < ┮Τ家.Count; i++)
        {
            ┮Τ家[i].SetActive(i == _skinID);  
        }
    }
    #endregion
}