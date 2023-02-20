using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家資料存取系統
/// </summary>
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
    /// <summary>
    /// 讀取存檔資料
    /// </summary>
    public void Load()
    {
        string 下載的JSON文件 = PlayerPrefs.GetString("PLAYERDATA", "");     //尋找名為"PLAYERDATA"的資料 找不到就回傳""
        //沒資料就建一個
        if(下載的JSON文件 == "")
        {
            data = new PlayerData();
            data.nickName = "未命名玩家";
            data.skin = 0;
        }
        else
        {
            data = JsonUtility.FromJson<PlayerData>(下載的JSON文件);
        }        
    }

    /// <summary>
    /// 存檔
    /// </summary>
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
    [SerializeField] public int skin;
}
#endregion