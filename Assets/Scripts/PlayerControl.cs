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
    [SerializeField] LayerMask 滑鼠可以點擊的圖層;
    [SerializeField] GameObject 魔法砲彈 = null;

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool 是否有點到地板 = Physics.Raycast(ray, out raycastHit, 999, 滑鼠可以點擊的圖層);
            Vector3 目標位置 = raycastHit.point;

            photonView.RPC("Shoot", RpcTarget.All, this.transform.position + (Vector3.up * 2f), 目標位置);
        }
    }

    [PunRPC]
    void Shoot(Vector3 startPos,Vector3 targetPos,PhotonMessageInfo info)
    {
        GameObject 砲彈 = Instantiate(魔法砲彈, startPos, Quaternion.identity);

        double lag多久 = PhotonNetwork.Time - info.SentServerTime;

        BulletControl 砲彈的程式 = 砲彈.GetComponent<BulletControl>();

        砲彈的程式.設定目標(targetPos);
        砲彈的程式.補償時間((float)lag多久);
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
