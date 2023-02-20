using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 遊戲開始後的管理器
/// </summary>
public class RoomManager : MonoBehaviourPunCallbacks
{
    #region 欄位
    [SerializeField] Vector3 生成中心點 = Vector3.zero;
    [SerializeField] float 隨機半徑 = 2f;
    #endregion

    #region 事件
    void Start()
    {
        this.transform.position = 生成中心點;
        this.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
        Vector3 生成點 = this.transform.TransformPoint(0f, 0f, Random.Range(0f, 隨機半徑));
        PhotonNetwork.Instantiate("PlayerGameplay", 生成點, Quaternion.identity);              //在這個網路中生成"PlayerGameplay"
    }
    #endregion
}
