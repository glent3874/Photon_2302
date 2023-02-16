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
    [SerializeField] SetPlayerInfo ��A�t�� = null;

    GameObject �۩�� = null;
    Vector3 lastPos = Vector3.zero;
    float moveSpeed = 0f;
    Quaternion moveQuaternion = Quaternion.identity;
    Vector3 bufferAB = Vector3.zero;

    void Start()
    {
        if (photonView.IsMine)
        {
            �۩�� = GameObject.Find("CameraPos");
            �۩��.transform.SetParent(this.transform);
            �۩��.transform.localPosition = Vector3.zero;
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
        Vector3 �ץ��᪺move = CameraRotate.instance.transform.TransformDirection(move);

        rig.velocity = �ץ��᪺move;
    }

    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    void AniUpdate()
    {
        float �{�b���t�׹F��W�����ʤ��� = moveSpeed / speed;
        if (��A�t��.�ʵe�t�� != null) 
        {
            ��A�t��.�ʵe�t��.SetFloat("Speed", �{�b���t�׹F��W�����ʤ���);
        }
    }
}
