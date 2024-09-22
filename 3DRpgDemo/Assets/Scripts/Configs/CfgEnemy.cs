/*
* 自动生成ConfigTools
*/
using System.Collections.Generic;

namespace Configs
{
    public class CfgEnemy
    {
        public Dictionary<string, EnemyData> mDataMap;
    }

    public class EnemyData
    {
        //ID
        public int id;
        //描述
        public string desc;
        //名字
        public string name;
        //血量
        public float HP;
        //攻击力
        public float ATK;
        //职业
        public string pClass;
    }
}
