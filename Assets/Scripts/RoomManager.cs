using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3 �ͦ������I = Vector3.zero;
    [SerializeField] float �H���b�| = 2f;

    void Start()
    {
        this.transform.position = �ͦ������I;
        this.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
        Vector3 �ͦ��I = this.transform.TransformPoint(0f, 0f, Random.Range(0f, �H���b�|));
        PhotonNetwork.Instantiate("PlayerGameplay", �ͦ��I, Quaternion.identity);
    }
}
