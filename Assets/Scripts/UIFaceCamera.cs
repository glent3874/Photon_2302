using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 讓UI 朝向攝影機
/// </summary>
public class UIFaceCamera : MonoBehaviour
{
    #region 欄位
    Transform 主攝影機座標 = null;
    #endregion

    #region 事件
    private void Start()
    {
        主攝影機座標 = Camera.main.transform;                 //抓main camera座標
    }

    private void LateUpdate()
    {
        this.transform.LookAt(主攝影機座標.position);
        this.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
    #endregion
}
