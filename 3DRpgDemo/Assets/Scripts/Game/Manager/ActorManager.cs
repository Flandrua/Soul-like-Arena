using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;

    [Header("===Override Animators===")]
    public AnimatorOverrideController swordAnim;
    public AnimatorOverrideController lanceAnim;
    // Start is called before the first frame update
    private void Awake() 
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
        ac.onAction += DoAction;


    }
    public void DoAction()
    {
        if (im.overlapEcastms.Count != 0)
        {
            if (im.overlapEcastms[0].active == true && !dm.IsPlaying())
            {
                if (im.overlapEcastms[0].eventName == "frontStab")
                {
                    transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                    ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                    dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);

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
                else if(im.overlapEcastms[0].eventName == "backStab")
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
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;
        if (ac.camcon.lockState == true)
        {
            ac.camcon.LockUnlock();
        }
        ac.camcon.enabled = false;
    }
    public void LockUnlockActorController(bool value)
    {
        if (ac != null)
            ac.SetBool("lock", value);
    }

    public void ChangeDualHands(bool dualOn)
    {
        if (dualOn)
        {
            ac.anim.runtimeAnimatorController = lanceAnim;
        }
        else
        {
            ac.anim.runtimeAnimatorController = swordAnim;
        }
    }
}
