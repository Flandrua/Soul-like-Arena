using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Security.Cryptography;

public class GameData
{
    private long _saveTime = 0;
    private CharacterData _mainPlayer = new CharacterData();
    public long SaveTime { get => _saveTime; set => _saveTime = value; }
    public CharacterData MainPlayer { get => _mainPlayer; set => _mainPlayer = value; }
}

public class CharacterData
{
    public int hp;
    public int mp;
    public int exp;
    public int atk;
    public int def;

    public List<ItemData> itemBag;
    public List<SkillData> skillData;

}

public class ItemData
{
    public int id;

    public int hp;
    public int mp;
    public int exp;
    public int atk;
    public int def;

    public ItemType type;
}

public class SkillData
{
    public int id;

    public int hp;
    public int mp;
    public int exp;
    public int atk;
    public int def;

    public SkillType type;
}

public class DataCenter : Singleton<DataCenter>
{
    GameData _gameData;
    public GameData GameData { get => _gameData; set => _gameData = value; }
    public void InitData()
    {
 
        //PlayerPrefs.DeleteKey("GameData");
        string str = PlayerPrefs.GetString("GameData");
        if (string.IsNullOrEmpty(str))
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
;    }

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

}
