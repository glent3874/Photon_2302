using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYENavAI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �ĤH
/// </summary>
public class Enemy : AYENavAI<���A>
{
    #region ���
    /// <summary>
    /// ���y�����d��
    /// </summary>
    [SerializeField] float �����d�� = 0f;
    /// <summary>
    /// ���a���ϼh �n�l�ܪ�����ϼh
    /// </summary>
    [SerializeField] LayerMask ���a�ϼh;
    /// <summary>
    /// ��e���l�ܥؼ�
    /// </summary>
    [SerializeField] Transform target = null;
    /// <summary>
    /// �i�H�Ϫ��a���˪��Z��
    /// </summary>
    [SerializeField] float �����Z�� = 1.5f;
    [SerializeField] float ���޲��ʳt�� = 2f;
    [SerializeField] float ���y���ʳt�� = 4f;
    [SerializeField] Animator �ʵe = null;
    #endregion

    void Awake()
    {
        //�n�O���A
        AddStatus(���A.����l��, ����l��, ����l�Ƥ�, ���}����l��);
        AddStatus(���A.�ݾ�, �ݾ�, �ݾ���, ���}�ݾ�);
        AddStatus(���A.����, ����, ���ޤ�, ���}����);
        AddStatus(���A.���y, ���y, ���y��, ���}���y);
        AddStatus(���A.����, ����, ������, ���}����);
        AddStatus(���A.����, ����, ���ˤ�, ���}����);
        AddStatus(���A.����, ����, ������, ���}����);
        AddStatus(���A.�k�], �k�], �k�]��, ���}�k�]);
    }

    #region ����l��
    void ����l��()
    {

    }

    void ����l�Ƥ�()
    {
        //�p�G�ڬO�Ъ� �N�ϼĤH�i�ݾ����A
        if (PhotonNetwork.IsMasterClient == true)
        {
            status = ���A.�ݾ�;
        }
    }

    void ���}����l��()
    {

    }
    #endregion

    #region �ݾ�
    void �ݾ�()
    {

    }

    void �ݾ���()
    {
        //�b�����A�@�q�ɶ��N����
        if (IsTime(Random.Range(2f, 5f)))
        {
            status = ���A.����;
        }
    }

    void ���}�ݾ�()
    {

    }
    #endregion

    #region ����
    void ����()
    {
        IsMove(true);                                                   //�Ұʲ��ʰʵe
        IsWalkOrRun(0);                                                 //�]�w�����]�B�ʵe
        speed = ���޲��ʳt��;                                            //���ު��t��
        Vector3 �H����X����@�ӥi�H�����I = GetRandomXZPos(5f, 3f);      //���o�d�򤺪��H���I
        Goto(�H����X����@�ӥi�H�����I, ��ؼЦ�m);                      //����(�ت��a, �����ɭn������)
    }

    /// <summary>
    /// ��F��m�ɨ��������ʵe
    /// </summary>
    void ��ؼЦ�m()
    {
        IsMove(false);
    }

    void ���ޤ�()
    {
        //�i�J���A�@�q�ɶ��i�J�ݾ�
        if (IsTime(3f))
        {
            status = ���A.�ݾ�;
        }
    }

    void ���}����()
    {
        IsMove(false);                  //���������ʵe
        StopGoto();                     //�����
    }
    #endregion

    #region ���y
    void ���y()
    {
        IsMove(true);                   //�Ұʨ����ʵe
        IsWalkOrRun(1);                 //�]�w�����]�B�ʵe
        speed = ���y���ʳt��;
    }

    void ���y��()
    {
        Goto(target.position, ��ؼЦ�m);       //���ʦ�(�ت��a, ��F����n������);
        //�l�ܤ@�q�ɶ���ݾ�
        if (IsTime(2f))
        {
            status = ���A.�ݾ�;
        }

        //�ڻP�ؼЪ��Z���b�d�򤺴N�Ұʧ���
        float �ڻP�ؼЪ��Z�� = Vector3.Distance(this.transform.position, target.position);
        if (�ڻP�ؼЪ��Z�� < �����Z��)
        {
            status = ���A.����;
        }
    }

    void ���}���y()
    {
        IsMove(false);
        StopGoto();
    }
    #endregion

    #region ����
    void ����()
    {
        Attack(true);               //�Ұʧ����ʵe
    }

    void ������()
    {
        //�i�J���A�@�q�ɶ��N�ݾ�
        if (IsTime(2.5f))
        {
            status = ���A.�ݾ�;
        }
    }

    void ���}����()
    {
        Attack(false);
    }
    #endregion

    #region ����
    void ����()
    {

    }

    void ���ˤ�()
    {

    }

    void ���}����()
    {

    }
    #endregion

    #region ����
    void ����()
    {

    }

    void ������()
    {

    }

    void ���}����()
    {

    }
    #endregion

    #region �k�]
    void �k�]()
    {

    }

    void �k�]��()
    {

    }

    void ���}�k�]()
    {

    }
    #endregion

    #region �ʵe�B�z
    /// <summary>
    /// �ϥβ��ʰʵe
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void IsMove(bool v)
    {
        �ʵe.SetBool("�O�_����", v);
        // �p�G�ڬO�Ъ� �P�B��L�ĤH�蹳���ʵe
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsMove", RpcTarget.Others, v);
        }
    }

    /// <summary>
    /// �����]�B�ʵe
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void IsWalkOrRun(float v)
    {
        �ʵe.SetFloat("�����ζ]�B", v);
        // �p�G�ڬO�Ъ� �P�B��L�ĤH�蹳���ʵe
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsWalkOrRun", RpcTarget.Others, v);
        }
    }

    /// <summary>
    /// �����ʵe
    /// </summary>
    /// <param name="v"></param>
    [PunRPC]
    void Attack(bool v)
    {
        if (v == true)
        {
            �ʵe.SetTrigger("���q����");
        }
        else
        {
            �ʵe.ResetTrigger("���q����");
        }
        
        // �p�G�ڬO�Ъ� �P�B��L�ĤH�蹳���ʵe
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Attack", RpcTarget.Others, v);
        }
    }
    #endregion

    #region ���u�˩w
    protected override void FixedUpdate30()
    {
        base.FixedUpdate30();

        //��b�ݾ��Ψ��ު��A�� �M��d�򤺬O"���a�ϼh"������
        if (status == ���A.�ݾ� || status == ���A.����)
        {
            Collider[] �Ҩ��һD = Physics.OverlapSphere(this.transform.position, �����d��, ���a�ϼh);

            //�p�G�o�{�F��
            if (�Ҩ��һD != null)
            {
                if (�Ҩ��һD.Length > 0)
                {
                    target = �Ҩ��һD[0].transform;     //�N���ʥؼг]���o�{���F��}�C���Ĥ@�� (�o�{���Ĥ@�Ӫ��a)
                    status = ���A.���y;                 //�i�J���y
                }
            }
        }
    }
    #endregion
}

/// <summary>
/// AI�����A��
/// </summary>
public enum ���A
{
    ����l��,
    �ݾ�,
    ����,
    ���y,
    ����,
    ����,
    ����,
    �k�],
}
