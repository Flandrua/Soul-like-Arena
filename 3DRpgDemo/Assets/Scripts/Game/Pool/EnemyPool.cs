using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoSingleton<EnemyPool>
{
    public List<GameObject> poolList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToPool(GameObject enemy)
    {
        poolList.Add(enemy);
        enemy.transform.parent=this.transform;
        enemy.transform.localPosition = Vector3.zero;
        enemy.SetActive(false);
    }
    public GameObject GetEnemy(string name)
    {
        if (poolList.Count > 0)
        {
            foreach(GameObject obj in poolList)
            {
                if(obj.name.Contains(name))
                {
                    obj.SetActive(true);
                    ActorManager am = obj.GetComponent<ActorManager>();
                    StateManager sm = am.sm;
                    am.deathEffect.ResetAlpha();
                    am.initManager();
                    sm.initData(sm.HPMax,sm.MPMax, sm.ATK);
                    am.ac.casterController.gameObject.SetActive(false);
                    poolList.Remove(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
