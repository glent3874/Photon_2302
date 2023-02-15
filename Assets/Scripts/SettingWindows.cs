using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindows : Windows<SettingWindows>
{
    #region ���
    [SerializeField] InputField ��J�� = null;
    #endregion

    #region ��k
    public override void Open()
    {
        base.Open();
        ��J��.text = PlayerInfoManager.instance.data.nickName;
    }

    public override void Close()
    {
        base.Close();
        if(��J��.text != "")
        {
            PlayerInfoManager.instance.data.nickName = ��J��.text;
            PlayerInfoManager.instance.Save();
        }
    }
    #endregion
}
