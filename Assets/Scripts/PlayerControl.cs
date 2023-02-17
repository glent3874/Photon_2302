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
    [SerializeField] float �蹳�ʵe�w�� = 5f;
    [SerializeField] LayerMask �ƹ��i�H�I�����ϼh;
    [SerializeField] GameObject �]�k���u = null;

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool �O�_���I��a�O = Physics.Raycast(ray, out raycastHit, 999, �ƹ��i�H�I�����ϼh);
            Vector3 �ؼЦ�m = raycastHit.point;

            photonView.RPC("Shoot", RpcTarget.All, this.transform.position + (Vector3.up * 2f), �ؼЦ�m);
        }
    }

    [PunRPC]
    void Shoot(Vector3 startPos,Vector3 targetPos,PhotonMessageInfo info)
    {
        GameObject ���u = Instantiate(�]�k���u, startPos, Quaternion.identity);

        double lag�h�[ = PhotonNetwork.Time - info.SentServerTime;

        BulletControl ���u���{�� = ���u.GetComponent<BulletControl>();

        ���u���{��.�]�w�ؼ�(targetPos);
        ���u���{��.���v�ɶ�((float)lag�h�[);
    }

    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    void AniUpdate()
    {
        float �{�b���t�׹F��W�����ʤ��� = moveSpeed / speed;

        if (photonView.IsMine == false)
        {
            �蹳�ʵe�w�� = Mathf.Lerp(�蹳�ʵe�w��, �{�b���t�׹F��W�����ʤ���, Time.deltaTime * 5f);
            if (��A�t��.�ʵe�t�� != null)
            {
                ��A�t��.�ʵe�t��.SetFloat("Speed", �蹳�ʵe�w��);
            }
        }
        else
        {
            if (��A�t��.�ʵe�t�� != null)
            {
                ��A�t��.�ʵe�t��.SetFloat("Speed", �{�b���t�׹F��W�����ʤ���);
            }
        }

        
    }
}
