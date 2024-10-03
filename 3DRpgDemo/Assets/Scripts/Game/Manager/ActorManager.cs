using Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    public DeathDyingOut deathEffect;
    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;
    public CharacterAudioManager charam;
    [Header("===Override Animators===")]
    public AnimatorOverrideController swordAnim;
    public AnimatorOverrideController lanceAnim;


    private List<SkillData> activeSkill = new List<SkillData>();
    public bool isLastEnemy = false;
    // Start is called before the first frame update
    private void Awake()
    {
        initManager();
    }
    public void initManager()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = null;
        try
        {
            sensor = transform.Find("sensor").gameObject;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(gameObject.name + " has no sensor");
        }

        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InteractionManager>(sensor);
        charam = Bind<CharacterAudioManager>(gameObject);
        ac.onAction += DoAction;

        //sm单独自己初始化 ac初始化所有controller
        wm.initManager();//wm中初始化wc
        bm.initManager();
        dm.initManager();
        im.initManager();
        charam.initManager();
        ac.am = this;
        sm.am = this;
        ac.initController();
        deathEffect.isAI = ac.camController.isAI;
    }
    public void DoAction()
    {
        if (im.overlapEcastms.Count != 0)
        {
            if (im.overlapEcastms[0].active == true && !dm.IsPlaying())//这里不能是第[0]个，必须得好好锁定到目标？
            {
                if (im.overlapEcastms[0].eventName == "frontStab")
                {
                    transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                    ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                    dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);
                    im.overlapEcastms.Remove(im.overlapEcastms[0]);
                }
                else if (im.overlapEcastms[0].eventName == "openBox")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))
                    {
                        im.overlapEcastms[0].active = false;
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("openBox", this, im.overlapEcastms[0].am);

                    }
                }
                else if (im.overlapEcastms[0].eventName == "leverUp")
                {
                    if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))
                    {
                        im.overlapEcastms[0].active = false;
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        dm.PlayFrontStab("leverUp", this, im.overlapEcastms[0].am);

                    }
                }
                else if (im.overlapEcastms[0].eventName == "backStab")
                {
                    transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                    ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                    dm.PlayFrontStab("backStab", this, im.overlapEcastms[0].am);
                }
            }
        }
    }

    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempInstance;
        if (go == null) return null;
        tempInstance = go.GetComponent<T>();
        if (tempInstance == null)
            tempInstance = go.AddComponent<T>();
        tempInstance.am = this;
        return tempInstance;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TryDoDamage(WeaponController targetWc, bool attackValid, bool counterValid)
    {
        if (ac.CheckState("roll")) return;
        if (sm.isCounterBackSucess)
        {
            if (counterValid)
                targetWc.wm.am.Stunned();
            //Do nothing
        }
        else if (sm.isCounterBackFailure)
        {
            if (attackValid)
                HitOrDie(targetWc, false);
        }
        else if (sm.isImmortal)
        {
            //Do nothing
        }
        else if (sm.isDefense)
        {
            //Attack should be blocked
            if (attackValid)
                Blocked();
        }
        else
        {
            if (sm.HP <= 0)
            {
                //Already dead.

            }
            else
            {
                if (attackValid)
                    HitOrDie(targetWc, true);
            }
        }
    }
    public void HitOrDie(WeaponController targetWc, bool doHitAnimation)
    {
        sm.AddHP(-1 * targetWc.GetATK());
        if (sm.HP > 0)
        {
            if (doHitAnimation)
                Hit();
            //do some VFX   
        }
        else
        {
            Die();
        }
    }
    /// <summary>
    /// 暂时只做了加血的功能
    /// </summary>
    /// <param name="id"></param>
    public void UseItem(int id)
    {
        ItemData item = DataCenter.Instance.ReturnAvaibleItem(id);
        if(item != null)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_FOUR);
            sm.AddHP(item.HP);
        }
    }

    public void UseSkill(int id)
    {
        SkillData skill = DataCenter.Instance.ReturnAvaibleSkill(id);
        if (skill != null)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_THREE);
            if (activeSkill.Count > 0)
            {
                foreach (SkillData activeSkill in activeSkill)
                {
                    if (skill == activeSkill)
                    {
                        InactiveSKill(skill);
                        return;
                    }
                    else
                    {
                        ActiveSkill(skill);
                        return;
                    }
                }
            }
            else ActiveSkill(skill);
        }
    }

    public void OutOfMP()
    {
        if (activeSkill.Count > 0)
        {
            foreach(SkillData skill in activeSkill)
                InactiveSKill(skill);
        }
    }

    public void ActiveSkill(SkillData skill)
    {
        activeSkill.Add(skill);
        sm.AddMPPerSecond(-skill.MP);
        sm.ATK += skill.ATK;
        if (skill.id == 1) { ac.vfxController.canVFX = true; }
    }
    public void InactiveSKill(SkillData skill)
    {
        activeSkill.Remove(skill);
        sm.AddMPPerSecond(skill.MP);
        sm.ATK -= skill.ATK;
        if (skill.id == 1) { ac.vfxController.canVFX = false; }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }
    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }
    public void Hit()
    {
        ac.IssueTrigger("hit");
    }
    public void Die()
    {
        sm.HP = 0;
        OutOfMP();
       
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;     
        if (ac.camController.lockState == true)
        {
            ac.camController.LockUnlock();
        }
        ac.camController.enabled = false;
        deathEffect.death = true;

        if(ac.camController.isAI) { 
            GameManager.Instance.CheckAIExist(); 
        }
    }
    public void DieByDM()
    {
        sm.HP = 0;
        OutOfMP();
        ac.pi.inputEnabled = false;
        if (ac.camController.lockState == true)
        {
            ac.camController.LockUnlock();
        }
        ac.camController.enabled = false;
        deathEffect.death = true;

        if (ac.camController.isAI)
        {
            GameManager.Instance.CheckAIExist();
        }
    }
    public void LockUnlockActorController(bool value)
    {
        if (ac != null)
            ac.SetBool("lock", value);
    }

    public void ChangeToLancer(bool isLancer)
    {
        if (isLancer)
        {
            ac.anim.runtimeAnimatorController = lanceAnim;
        }
        else
        {
            ac.anim.runtimeAnimatorController = swordAnim;
        }
    }
}
