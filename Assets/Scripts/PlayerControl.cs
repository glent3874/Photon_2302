using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �b�C���������a����
/// </summary>
public class PlayerControl : MonoBehaviourPunCallbacks
{
    #region ���
    [SerializeField] Rigidbody rig = null;
    [SerializeField] [Tooltip("�]�t�]�w")] float speed = 5f;
    [SerializeField] Transform rotateRoot = null;
    [SerializeField] SetPlayerInfo ��A�t�� = null;
    [SerializeField] float �蹳�ʵe�w�� = 0f;
    [SerializeField] LayerMask �ƹ��i�H�I�����ϼh;
    [SerializeField] GameObject �]�k���u = null;

    [Tooltip("��v���l�ܥؼ�")]
    GameObject �۩�� = null;
    Vector3 lastPos = Vector3.zero;
    [Tooltip("��ڤW���]�t")]
    float moveSpeed = 0f;
    Quaternion moveQuaternion = Quaternion.identity;
    Vector3 bufferAB = Vector3.zero;
    #endregion

    #region �ƥ�
    void Start()
    {   
        //�p�G�ڬO���L ����v���l�ܪ��ؼ�"CameraPos" �����۩�� �ç@���ڪ��l����
        if (photonView.IsMine)
        {
            �۩�� = GameObject.Find("CameraPos");
            �۩��.transform.SetParent(this.transform);
            �۩��.transform.localPosition = Vector3.zero;
        }
    }

    private void Update()
    {
        //�p�G�ڬO���L 
        if (photonView.IsMine)
        {
            Move();                                             //����
        }
        Rotate();                                               //����
        AniUpdate();                                            //�ʵe��s
    }

    private void FixedUpdate()
    {
        Vector3 pos = rig.position;                             //��e��m
        Vector3 a = lastPos;                                    //�W�@������m
        Vector3 b = pos;
        a.y = b.y;                                              //�P�B����
        Vector3 ab = b - a;
        moveSpeed = Vector3.Distance(a, b);

        moveSpeed = moveSpeed / Time.fixedDeltaTime;            //�p��C�@�������ʶq

        //�����ʴN��s����
        if (moveSpeed > 0.01f)
        {
            bufferAB = Vector3.Lerp(bufferAB, ab, Time.fixedDeltaTime * 10f);           //����
            moveQuaternion = Quaternion.LookRotation(bufferAB);                         //�Nab���ʭȧ��ܦ�����q
        }
        lastPos = rig.position;
    }
    #endregion

    #region ��k
    /// <summary>
    /// ���z�t�β��ʻP�o�g�l�u
    /// </summary>
    void Move()
    {
        //���z�t�β���
        float ws = Input.GetAxisRaw("Vertical");                                                        //���o������J��
        float ad = Input.GetAxisRaw("Horizontal");                                                      //���o������J��

        Vector3 move = new Vector3(ad * speed, rig.velocity.y, ws * speed);                             //�ᤩx z�b���V�q
        move = Vector3.ClampMagnitude(move, speed);                                                     //�פ�V�����ʶZ���|�W�Xspeed���]�w �ҥH�n����
        move.y = rig.velocity.y;                                                                        //�i��|�J��שY���D �����Ny�b�^�_
        Vector3 �ץ��᪺move = CameraRotate.instance.transform.TransformDirection(move);                 //�Hcamerarotate�����V���Ѧ� ���ܨ��⪺�e�i��V

        rig.velocity = �ץ��᪺move;

        //�ƹ������I���o�g�l�u
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                //�g�X�@���p�g �_�I�Omain camera ���I�O�ƹ��I�����a��
            bool �O�_���I��a�O = Physics.Raycast(ray, out raycastHit, 999, �ƹ��i�H�I�����ϼh);           //�p�G����ƹ��i�H�I�����ϼh�Ntrue
            Vector3 �ؼЦ�m = raycastHit.point;                                                         //�l�u������ؼЬ��p�g�����쪺�����I

            //�R�O�Ҧ��蹳(�]�A�ۤv)�ϥ�shoot��k�o�g�l�u �ǤJ �l�u�_�I�P���I(�ƹ��I�U����m)
            photonView.RPC("Shoot", RpcTarget.All, this.transform.position + (Vector3.up * 2f), �ؼЦ�m);
        }
    }

    /// <summary>
    /// �g�X�l�u
    /// </summary>
    /// <param name="startPos">�l�u�_�I</param>
    /// <param name="targetPos">�ؼ��I</param>
    /// <param name="info">���A���W�Q�ǿ骺�H�����򥻸��</param>
    [PunRPC]
    void Shoot(Vector3 startPos,Vector3 targetPos,PhotonMessageInfo info)
    {
        GameObject ���u = Instantiate(�]�k���u, startPos, Quaternion.identity);       //�ͦ����u

        double lag�h�[ = PhotonNetwork.Time - info.SentServerTime;                   //�ڷ�e���A�����ɶ� - �T���e�X���ɶ� = lag�ɪ�

        BulletControl ���u���{�� = ���u.GetComponent<BulletControl>();

        //�P�B���𪺤l�u
        ���u���{��.�]�w�ؼ�(targetPos);
        ���u���{��.���v�ɶ�((float)lag�h�[);
    }

    /// <summary>
    /// �������
    /// </summary>
    void Rotate()
    {
        rotateRoot.rotation = Quaternion.Lerp(rotateRoot.rotation, moveQuaternion, Time.deltaTime * 20f);
    }

    /// <summary>
    /// �ʵe��s
    /// </summary>
    void AniUpdate()
    {
        float �{�b���t�׻P�W�����ʤ��� = moveSpeed / speed;

        //�p�G�ڤ��O���L �N�ϥνw�ĹL���ʵe�ƭ�
        if (photonView.IsMine == false)
        {
            �蹳�ʵe�w�� = Mathf.Lerp(�蹳�ʵe�w��, �{�b���t�׻P�W�����ʤ���, Time.deltaTime * 5f);
            if (��A�t��.�ʵe�t�� != null)
            {
                ��A�t��.�ʵe�t��.SetFloat("Speed", �蹳�ʵe�w��);
            }
        }
        else
        {
            if (��A�t��.�ʵe�t�� != null)
            {
                ��A�t��.�ʵe�t��.SetFloat("Speed", �{�b���t�׻P�W�����ʤ���);
            }
        }

        
    }
    #endregion
}
