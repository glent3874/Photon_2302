using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager
{
    #region ³æ¨Ò¼Ò¦¡
    public static PlayerInfoManager instance
    {
        get
        {
            if(_instance != null)
            {
                _instance = new PlayerInfoManager();
            }
            return _instance;
        }
    }
    static PlayerInfoManager _instance = null;
    #endregion
}

[System.Serializable]
public struct playerData
{
    
}