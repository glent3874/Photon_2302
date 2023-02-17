using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYENavAI;

public class Enemy : AYENavAI<���A>
{
    #region ���
    [SerializeField] float �����d�� = 0f;
    [SerializeField] LayerMask ���a�ϼh;
    [SerializeField] Transform target = null;
    [SerializeField] float ���޲��ʳt�� = 2f;
    [SerializeField] float ���y���ʳt�� = 4f;
    [SerializeField] Animator �ʵe = null;
    #endregion

    void Awake()
    {
        AddStatus(���A.�ݾ�, �ݾ�, �ݾ���, ���}�ݾ�);
        AddStatus(���A.����, ����, ���ޤ�, ���}����);
        AddStatus(���A.���y, ���y, ���y��, ���}���y);
        AddStatus(���A.����, ����, ������, ���}����);
        AddStatus(���A.����, ����, ���ˤ�, ���}����);
        AddStatus(���A.����, ����, ������, ���}����);
        AddStatus(���A.�k�], �k�], �k�]��, ���}�k�]);
    }

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
        �ʵe.SetBool("�O�_����", true);
        �ʵe.SetFloat("�����ζ]�B", 0);
        speed = ���޲��ʳt��;
        Vector3 �H����X����@�ӥi�H�����I = GetRandomXZPos(5f, 3f);
        Goto(�H����X����@�ӥi�H�����I, ��ؼЦ�m);
    }

    void ��ؼЦ�m()
    {
        �ʵe.SetBool("�O�_����", false);
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
        �ʵe.SetBool("�O�_����", false);
        StopGoto();
    }
    #endregion

    #region ���y
    void ���y()
    {
        �ʵe.SetBool("�O�_����", true);
        �ʵe.SetFloat("�����ζ]�B", 1);
        speed = ���y���ʳt��;
    }

    void ���y��()
    {
        Goto(target.position, ��ؼЦ�m);
        if (IsTime(2f))
        {
            status = ���A.�ݾ�;
        }
    }

    void ���}���y()
    {
        �ʵe.SetBool("�O�_����", false);
        StopGoto();
    }
    #endregion

    #region ����
    void ����()
    {
        �ʵe.SetTrigger("���q����");
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
        �ʵe.ResetTrigger("���q����");
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

[SerializeField]
public enum ���A
{
    �ݾ�,
    ����,
    ���y,
    ����,
    ����,
    ����,
    �k�],
}
