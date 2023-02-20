using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �l�u���檺���
/// </summary>
public class BulletControl : MonoBehaviour
{
    #region ���
    [SerializeField] float speed = 5f;
    #endregion

    #region �ƥ�
    private void Update()
    {
        this.transform.Translate(0f, 0f, speed * Time.deltaTime, Space.Self);    //�u�ۦ�����Z�b�e�i
    }
    #endregion

    #region ��k
    /// <summary>
    /// �]�w�l�u���V
    /// </summary>
    /// <param name="�ؼ��I"></param>
    public void �]�w�ؼ�(Vector3 �ؼ��I)
    {
        this.transform.LookAt(�ؼ��I);
    }

    /// <summary>
    /// ����P�蹳���l�u�o�g�ɶ��I���P �ҥH�ݭn���ɥ��h������Z��
    /// </summary>
    /// <param name="_lag�h�["></param>
    public void ���v�ɶ�(float _lag�h�[)
    {
        this.transform.Translate(0f, 0f, speed * _lag�h�[, Space.Self);
    }
    #endregion
}
