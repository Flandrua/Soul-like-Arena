using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Security.Cryptography;
using Configs;

public class GameData
{
    private long _saveTime = 0;
    private CharacterData _mainPlayer = new CharacterData();
    public long SaveTime { get => _saveTime; set => _saveTime = value; }
    public CharacterData MainPlayer { get => _mainPlayer; set => _mainPlayer = value; }
}

public class CharacterData
{
    public float hp;
    public float mp;
    public float exp;
    public float atk;
    public float def;

    public List<ItemData> itemBag = new List<ItemData>();
    public List<SkillData> skillData = new List<SkillData>();

}



public class DataCenter : Singleton<DataCenter>
{
    GameData _gameData;
    public GameData GameData { get => _gameData; set => _gameData = value; }
    public void InitData()
    {

        //PlayerPrefs.DeleteKey("GameData");
        string str = PlayerPrefs.GetString("GameData");
        if (str == null)
        {
            _gameData = new GameData();
            SaveData();
        }
        else
        {
            _gameData = JsonMapper.ToObject<GameData>(str);
        }
    }

    public void SaveData()
    {
        _gameData.SaveTime = GetTimeStamp();
        var json = JsonMapper.ToJson(_gameData);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        ;
    }

    private long GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    public Color HexToColor(string hex)
    {
        Color nowColor;
        ColorUtility.TryParseHtmlString(hex, out nowColor);
        return nowColor;
    }

    public void AddItem(int id, int count = 1)
    {
        if (id == 1) { EventManager.DispatchEvent(EventCommon.ADD_DRUG, count); }
        for (int i = 0; i < count; i++)
        {
            var cfgItem = DataManager.Instance.CfgItem;
            GameData.MainPlayer.itemBag.Add(cfgItem.mDataMap[$"{id}"]);
        }
    }

    public void AddSkill(int id)
    {
        var cfgSkill = DataManager.Instance.CfgSkill;
        GameData.MainPlayer.skillData.Add(cfgSkill.mDataMap[$"{id}"]);
    }

    public ItemData ReturnAvaibleItem(int id)
    {
        foreach (ItemData item in GameData.MainPlayer.itemBag)
        {
            if (item.id == id)
            {
                GameData.MainPlayer.itemBag.Remove(item);
                return item;
            }
        }
        Debug.LogWarning($"Cant find id:{id} item in your bag");
        return null;
    }

    public SkillData ReturnAvaibleSkill(int id)
    {
        foreach (SkillData skill in GameData.MainPlayer.skillData)
        {
            if (skill.id == id)
            {
                return skill;
            }
        }
        Debug.LogWarning($"Cant find id:{id} skill in your skill list");
        return null;
    }

}
