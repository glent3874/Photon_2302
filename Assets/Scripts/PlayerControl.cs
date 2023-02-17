using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody rig = null;
    [SerializeField] float speed = 5f;
    [SerializeField] Transform rotateRoot = null;
    [SerializeField] SetPlayerInfo 衣服系統 = null;
    [SerializeField] float 鏡像動畫緩衝 = 5f;

    GameObject 自拍棒 = null;
    Vector3 lastPos = Vector3.zero;
    float moveSpeed = 0f;
    Quaternion moveQuaternion = Quaternion.identity;
    Vector3 bufferAB = Vector3.zero;

    void Start()
    {
        if (photonView.IsMine)
        {
            自拍棒 = GameObject.Find("CameraPos");
            自拍棒.transform.SetParent(this.transform);
            自拍棒.transform.localPosition = Vector3.zero;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }
        Rotate();
        AniUpdate();
    }

    private void FixedUpdate()
    {
        Vector3 pos = rig.position;
        Vector3 a = lastPos;
        Vector3 b = pos;
        a.y = b.y;
        Vector3 ab = b - a;
        moveSpeed = Vector3.Distance(a, b);

        moveSpeed = moveSpeed / Time.fixedDeltaTime;

        if (moveSpeed > 0.01f)
        {
            bufferAB = Vector3.Lerp(bufferAB, ab, Time.fixedDeltaTime * 10f);
            moveQuaternion = Quaternion.LookRotation(bufferAB);
        }
        lastPos = rig.position;
    }

    void Move()
    {
        float ws = Input.GetAxisRaw("Vertical");
        float ad = Input.GetAxisRaw("Horizontal");

        Vector3 move = new Vector3(ad * speed, rig.velocity.y, ws * speed);
        move = Vector3.ClampMagnitude(move, speed);
        move.y = rig.velocity.y;
        Vector3 修正後的move = CameraRotate.instance.transform.TransformDirection(move);

        rig.velocity = 修正後的move;
    }

    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    void AniUpdate()
    {
        float 現在的速度達到上限的百分比 = moveSpeed / speed;

        if (photonView.IsMine == false)
        {
            鏡像動畫緩衝 = Mathf.Lerp(鏡像動畫緩衝, 現在的速度達到上限的百分比, Time.deltaTime * 5f);
            if (衣服系統.動畫系統 != null)
            {
                衣服系統.動畫系統.SetFloat("Speed", 鏡像動畫緩衝);
            }
        }
        else
        {
            if (衣服系統.動畫系統 != null)
            {
                衣服系統.動畫系統.SetFloat("Speed", 現在的速度達到上限的百分比);
            }
        }

        
    }
}
