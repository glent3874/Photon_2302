using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �N�����󪺱��ୱ�V�ܱo��camera�@��
/// </summary>
public class CameraRotate : MonoBehaviour
{
    #region ��ҼҦ�
    public static CameraRotate instance = null;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region �ƥ�
    void Update()
    {
        float ��v����Y�� = Camera.main.transform.rotation.eulerAngles.y;        //���omain camera�������(.y)
        this.transform.rotation = Quaternion.Euler(0f, ��v����Y��, 0f);         //�N�����󪺱����(.y)�ܦ���main camera�@��
    }
    #endregion
}
