using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��UI �¦V��v��
/// </summary>
public class UIFaceCamera : MonoBehaviour
{
    #region ���
    Transform �D��v���y�� = null;
    #endregion

    #region �ƥ�
    private void Start()
    {
        �D��v���y�� = Camera.main.transform;                 //��main camera�y��
    }

    private void LateUpdate()
    {
        this.transform.LookAt(�D��v���y��.position);
        this.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
    #endregion
}
