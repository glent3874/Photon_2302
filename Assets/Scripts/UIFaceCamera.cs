using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    #region 欄位
    Transform 主攝影機座標 = null;
    #endregion

    #region 事件
    private void Start()
    {
        主攝影機座標 = Camera.main.transform;
    }

    private void LateUpdate()
    {
        this.transform.LookAt(主攝影機座標.position);
        this.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
    #endregion
}
