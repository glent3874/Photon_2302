using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a��Ʀs���t��
/// </summary>
public class PlayerInfoManager
{
    #region ��ҼҦ�
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

    #region ���
    public PlayerData data = new PlayerData();

    public System.Action Act_Save = null;
    #endregion

    #region ��k
    /// <summary>
    /// Ū���s�ɸ��
    /// </summary>
    public void Load()
    {
        string �U����JSON��� = PlayerPrefs.GetString("PLAYERDATA", "");     //�M��W��"PLAYERDATA"����� �䤣��N�^��""
        //�S��ƴN�ؤ@��
        if(�U����JSON��� == "")
        {
            data = new PlayerData();
            data.nickName = "���R�W���a";
            data.skin = 0;
        }
        else
        {
            data = JsonUtility.FromJson<PlayerData>(�U����JSON���);
        }        
    }

    /// <summary>
    /// �s��
    /// </summary>
    public void Save()
    {
        string �Y�N�s�ɪ�JSON��� = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PLAYERDATA", �Y�N�s�ɪ�JSON���);

        if(Act_Save != null)
        {
            Act_Save.Invoke();
        }
    }
    #endregion
}

#region ���c
[System.Serializable]
public struct PlayerData
{
    [SerializeField] public string nickName;
    [SerializeField] public int skin;
}
#endregion