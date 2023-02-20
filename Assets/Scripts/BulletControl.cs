using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子彈飛行的控制器
/// </summary>
public class BulletControl : MonoBehaviour
{
    #region 欄位
    [SerializeField] float speed = 5f;
    #endregion

    #region 事件
    private void Update()
    {
        this.transform.Translate(0f, 0f, speed * Time.deltaTime, Space.Self);    //沿著此物件的Z軸前進
    }
    #endregion

    #region 方法
    /// <summary>
    /// 設定子彈面向
    /// </summary>
    /// <param name="目標點"></param>
    public void 設定目標(Vector3 目標點)
    {
        this.transform.LookAt(目標點);
    }

    /// <summary>
    /// 本體與鏡像的子彈發射時間點不同 所以需要彌補失去的飛行距離
    /// </summary>
    /// <param name="_lag多久"></param>
    public void 補償時間(float _lag多久)
    {
        this.transform.Translate(0f, 0f, speed * _lag多久, Space.Self);
    }
    #endregion
}
