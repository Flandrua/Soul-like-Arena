/*
* 自动生成ConfigTools
*/
using System.Collections.Generic;

namespace Configs
{
    public class CfgItem
    {
        public Dictionary<string, ItemData> mDataMap;
    }

    public class ItemData
    {
        //ID
        public int id;
        //描述
        public string desc;
        //血量
        public float HP;
        //法力
        public float MP;
        //经验
        public float EXP;
        //攻击
        public float ATK;
        //防御
        public float DEF;
    }
}
