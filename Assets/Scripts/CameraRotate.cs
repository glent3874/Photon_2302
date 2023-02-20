using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 將此物件的旋轉面向變得跟camera一樣
/// </summary>
public class CameraRotate : MonoBehaviour
{
    #region 單例模式
    public static CameraRotate instance = null;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region 事件
    void Update()
    {
        float 攝影機的Y值 = Camera.main.transform.rotation.eulerAngles.y;        //取得main camera的旋轉值(.y)
        this.transform.rotation = Quaternion.Euler(0f, 攝影機的Y值, 0f);         //將此物件的旋轉值(.y)變成跟main camera一樣
    }
    #endregion
}
