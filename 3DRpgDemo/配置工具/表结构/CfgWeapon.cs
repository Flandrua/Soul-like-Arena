/*
* 自动生成ConfigTools
*/
using System.Collections.Generic;

namespace Configs
{
    public class CfgWeapon
    {
        public Dictionary<string, WeaponData> mDataMap;
    }

    public class WeaponData
    {
        //ID
        public int id;
        //描述
        public string desc;
        //攻击
        public float ATK;
        //防御
        public float DEF;
        //职业
        public string Pclass;
    }
}
