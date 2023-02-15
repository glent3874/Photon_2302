using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager
{
    #region 單例模式
    public static PlayerInfoManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new PlayerInfoManager();
                _instance.Load();
            }
            return _instance;
        }
    }
    static PlayerInfoManager _instance = null;
    #endregion

    #region 欄位
    public PlayerData data = new PlayerData();

    public System.Action Act_Save = null;
    #endregion

    #region 方法
    public void Load()
    {
        string 下載的JSON文件 = PlayerPrefs.GetString("PLAYERDATA", "");
        if(下載的JSON文件 == "")
        {
            data = new PlayerData();
            data.nickName = "未命名玩家";
        }
        else
        {
            data = JsonUtility.FromJson<PlayerData>(下載的JSON文件);
        }        
    }

    public void Save()
    {
        string 即將存檔的JSON文件 = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PLAYERDATA", 即將存檔的JSON文件);

        if(Act_Save != null)
        {
            Act_Save.Invoke();
        }
    }
    #endregion
}

#region 結構
[System.Serializable]
public struct PlayerData
{
    [SerializeField] public string nickName;
}
#endregion