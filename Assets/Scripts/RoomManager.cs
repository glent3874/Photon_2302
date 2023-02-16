using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 生成中心點 = Vector3.zero;
    [SerializeField] float 隨機半徑 = 2f;

    void Start()
    {
        this.transform.position = 生成中心點;
        this.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
        Vector3 生成點 = this.transform.TransformPoint(0f, 0f, Random.Range(0f, 隨機半徑));
        PhotonNetwork.Instantiate("PlayerGameplay", 生成點, Quaternion.identity);
    }
}
