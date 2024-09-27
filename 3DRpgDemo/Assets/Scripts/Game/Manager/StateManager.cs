using Configs;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
    public float HP = 15.0f;
    public float HPMax = 15.0f;
    public float MP = 10.0f;
    public float MPMax = 10.0f;
    public float ATK = 10.0f;


    private float mpPerSecond = 0.5f;


    [Header("1st order state flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack = false; //relate to state
    public bool isCounterBackEnable;//relate to animation events

    [Header("2nd order state flag")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSucess;
    public bool isCounterBackFailure;


    private void Start()
    {
        initData(HPMax, MPMax, ATK, false);
    }
    public void initData(float hpMax, float mpMax, float atk, bool isAI = true)//单独初始化
    {
        ATK = atk;
        HPMax = hpMax;
        HP = HPMax;
        MPMax = mpMax;
        MP = MPMax;
        if (!isAI)
        {
            //对玩家进行初始化
            DataCenter.Instance.GameData.MainPlayer.hp = hpMax;
            DataCenter.Instance.GameData.MainPlayer.mp = mpMax;
            DataCenter.Instance.GameData.MainPlayer.atk = ATK;
            DataCenter.Instance.AddItem(1, 10);
            DataCenter.Instance.AddSkill(1);
            AddHP(0);
            AddMP(0);
        }
    }

    private void Update()
    {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckStateTag("attackR") || am.ac.CheckStateTag("attackL");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked");
        //isDefense = am.ac.CheckState("defense1h", "defense");
        isCounterBack = am.ac.CheckState("counterBack");

        isCounterBackSucess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;
        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckState("defense1h", "defense");
        isImmortal = isRoll || isJab;


        if (mpPerSecond != 0)
        {
            AddMP(mpPerSecond * Time.deltaTime);
            if (MP <= 0)
            {
                MP = 0;
                mpPerSecond = 0.5f;
                am.OutOfMP();
            }
        }
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);
        float[] hp = { HP, HPMax };
        EventManager.DispatchEvent(EventCommon.UPDATE_HP, hp);
    }

    public void AddMP(float value)
    {
        MP += value;
        MP = Mathf.Clamp(MP, 0, MPMax);
        float[] mp = { MP, MPMax };
        EventManager.DispatchEvent(EventCommon.UPDATE_MP, mp);
    }
    public void SetIsCounterBack(bool value)
    {
        isCounterBackEnable = value;
    }
    // Start is called before the first frame update
    public void AddMPPerSecond(float value)
    {
        mpPerSecond += value;
    }


}
