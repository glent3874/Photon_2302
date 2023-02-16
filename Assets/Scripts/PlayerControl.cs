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
    [SerializeField] SetPlayerInfo ︾狝╰参 = null;

    GameObject ╃次 = null;
    Vector3 lastPos = Vector3.zero;
    float moveSpeed = 0f;
    Quaternion moveQuaternion = Quaternion.identity;
    Vector3 bufferAB = Vector3.zero;

    void Start()
    {
        if (photonView.IsMine)
        {
            ╃次 = GameObject.Find("CameraPos");
            ╃次.transform.SetParent(this.transform);
            ╃次.transform.localPosition = Vector3.zero;
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
        Vector3 タmove = CameraRotate.instance.transform.TransformDirection(move);

        rig.velocity = タmove;
    }

    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    void AniUpdate()
    {
        float 瞷硉笷κだゑ = moveSpeed / speed;
        if (︾狝╰参.笆礶╰参 != null) 
        {
            ︾狝╰参.笆礶╰参.SetFloat("Speed", 瞷硉笷κだゑ);
        }
    }
}
