using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DeathDyingOut : MonoBehaviour
{
    // Start is called before the first frame update
    public SkinnedMeshRenderer[] mr;
    public float alpha = 1.0f;
    private float deathDuration = 3f;
    public bool isAI = false;//AM里面初始化这个bool

    public bool death = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (death)
        {
            foreach (SkinnedMeshRenderer mr in mr)
            {
                Color temp = mr.material.GetColor("_BaseColor");
                temp.a = alpha;
                alpha -= Time.deltaTime / deathDuration;
                mr.material.SetColor("_BaseColor", temp);
            }
            if (alpha < 0.1f)
            {
                alpha = 1.0f;
                death = false;
                if (isAI)
                {
                    EnemyPool.Instance.BackToPool(transform.parent.gameObject);
                }
                else
                {
                    //做玩家死亡的通知
                }
            }
        }
    }
    public void ResetAlpha()
    {
        alpha=1.0f;
        foreach (SkinnedMeshRenderer mr in mr)
        {
            Color temp = mr.material.GetColor("_BaseColor");
            temp.a = alpha;
            mr.material.SetColor("_BaseColor", temp);
        }
    }

}
