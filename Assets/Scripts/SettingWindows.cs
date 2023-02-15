using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindows : Windows<SettingWindows>
{
    #region 欄位
    [SerializeField] InputField 輸入框 = null;
    #endregion

    #region 方法
    public override void Open()
    {
        base.Open();
        輸入框.text = PlayerInfoManager.instance.data.nickName;
    }

    public override void Close()
    {
        base.Close();
        if(輸入框.text != "")
        {
            PlayerInfoManager.instance.data.nickName = 輸入框.text;
            PlayerInfoManager.instance.Save();
        }
    }
    #endregion
}
