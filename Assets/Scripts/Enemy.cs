using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYENavAI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : AYENavAI<���A>
{
    #region ���
    [SerializeField] float �����d�� = 0f;
    [SerializeField] LayerMask ���a�ϼh;
    [SerializeField] Transform target = null;
    [SerializeField] float �����Z�� = 1.5f;
    [SerializeField] float ���޲��ʳt�� = 2f;
    [SerializeField] float ���y���ʳt�� = 4f;
    [SerializeField] Animator �ʵe = null;
    #endregion

    void Awake()
    {
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
        IsMove(true);
        IsWalkOrRun(0);
        speed = ���޲��ʳt��;
        Vector3 �H����X����@�ӥi�H�����I = GetRandomXZPos(5f, 3f);
        Goto(�H����X����@�ӥi�H�����I, ��ؼЦ�m);
    }

    void ��ؼЦ�m()
    {
        IsMove(false);
    }

    void ���ޤ�()
    {
        if (IsTime(3f))
        {
            status = ���A.�ݾ�;
        }
    }

    void ���}����()
    {
        IsMove(false);
        StopGoto();
    }
    #endregion

    #region ���y
    void ���y()
    {
        IsMove(true);
        IsWalkOrRun(1);
        speed = ���y���ʳt��;
    }

    void ���y��()
    {
        Goto(target.position, ��ؼЦ�m);
        if (IsTime(2f))
        {
            status = ���A.�ݾ�;
        }
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
        Attack(true);
    }

    void ������()
    {
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
    [PunRPC]
    void IsMove(bool v)
    {
        �ʵe.SetBool("�O�_����", v);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsMove", RpcTarget.Others, v);
        }
    }

    [PunRPC]
    void IsWalkOrRun(float v)
    {
        �ʵe.SetFloat("�����ζ]�B", v);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IsWalkOrRun", RpcTarget.Others, v);
        }
    }

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

        if (status == ���A.�ݾ� || status == ���A.����)
        {
            Collider[] �Ҩ��һD = Physics.OverlapSphere(this.transform.position, �����d��, ���a�ϼh);
            if (�Ҩ��һD != null)
            {
                if (�Ҩ��һD.Length > 0)
                {
                    target = �Ҩ��һD[0].transform;
                    status = ���A.���y;
                }
            }
        }
    }
    #endregion
}

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
