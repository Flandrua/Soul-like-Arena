using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Configs;

public class DataManager : Singleton<DataManager>
{
    private Object[] configRes;
    public void LoadConfig(Object[] objArr)
    {
        configRes = objArr;
        _cfgGlobal = SetConfigData<CfgGlobal>("CfgGlobal");
        _cfgLanguage = SetConfigData<CfgLanguage>("CfgLanguage");
        _cfgWeapon = SetConfigData<CfgWeapon>("CfgWeapon");
        _cfgEnemy = SetConfigData<CfgEnemy>("CfgEnemy");
        _cfgItem = SetConfigData<CfgItem>("CfgItem");
        _cfgSkill = SetConfigData<CfgSkill>("CfgSkill");
    }

    private CfgGlobal _cfgGlobal;

    private CfgLanguage _cfgLanguage;

    private CfgWeapon _cfgWeapon;

    private CfgEnemy _cfgEnemy;
    private CfgItem _cfgItem;
    private CfgSkill _cfgSkill;

    public CfgGlobal CfgGlobal { get => _cfgGlobal; }

    public CfgLanguage CfgLanguage { get => _cfgLanguage; }

    public CfgWeapon CfgWeapon { get => _cfgWeapon; }
    public CfgEnemy CfgEnemy { get => _cfgEnemy; }
    public CfgSkill CfgSkill { get => _cfgSkill; }
    public CfgItem CfgItem { get => _cfgItem; }

    private T SetConfigData<T>(string name)
    {
        for (var i = 0; i < configRes.Length; i++)
        {
            if (configRes[i].name.Equals(name))
            {
                T config = default(T);
                try
                {
                    config = JsonMapper.ToObject<T>(configRes[i].ToString());
                }
                catch
                {
                    Debug.LogError(string.Format("{0} 表出错，请检查！！！！！", name));
                }
                return config;
            }
        }
        return default(T);
    }

}
