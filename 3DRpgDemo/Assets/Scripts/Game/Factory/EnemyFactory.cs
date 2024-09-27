using Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    // Start is called before the first frame update
    private string path = "Enemy/";
    private CfgEnemy enemyDB = null;
    public EnemyFactory(CfgEnemy cfgEnemy)
    {

        enemyDB = cfgEnemy;

    }

    public GameObject CreateEnemy(string enemyName)
    {
        GameObject preafab = Resources.Load(path + enemyName) as GameObject;
        GameObject obj = GameObject.Instantiate(preafab);
        foreach (var enemy in enemyDB.mDataMap.Values)
        {
            if(enemyName ==enemy.name)
            {
                //应用敌人数据
                ActorManager am = obj.GetComponent<ActorManager>();
                StateManager sm = am.sm;
                am.initManager();
                sm.initData(enemy.HP,enemy.MP,enemy.ATK);
                am.GetComponent<DummyIUserInput>().InitAI();
            }
        }
            return obj;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
