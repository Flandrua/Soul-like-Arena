using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public EventCasterController casterController;
    public CameraController camController;
    private RootMotionControll motionController;
    private TriggerControll triggerController;
    public VFXController vfxController;
    public ActorManager am;//am中已经指定这个am了
    public IUserInput pi;
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 5.0f;
    public float rollVelocity = 1f;

    [Space(10)]
    [Header("====Friction Settings====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    public Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private Collider col;
    //private float lerpTarget;
    private Vector3 deltaPos;
    public bool leftIsShield = true;

    public delegate void OnActionDelegate();
    public event OnActionDelegate onAction;
    private void Awake()
    {
        initController();
    }
    public void initController()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        motionController = model.GetComponent<RootMotionControll>();
        triggerController = model.GetComponent<TriggerControll>();
        vfxController = model.GetComponent<VFXController>();
        camController.initController();
        motionController.initController();
        triggerController.initTriggerController();
        casterController.initController();

    }
    void Start()
    {
        //AddListener();
    }
    private void OnDestroy()
    {
        //RemoveListenr();
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.lockon)
        {
            camController.LockUnlock();
        }
        if (camController.lockState == false)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.2f));
            anim.SetFloat("right", 0);

        }
        else
        {
            Vector3 localDVec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDVec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDVec.x * ((pi.run) ? 2.0f : 1.0f));
        }
        //anim.SetBool("defense", pi.defense);
        if (rigid.velocity.magnitude > 5.0f)
        {
            anim.SetTrigger("roll");
        }
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if ((pi.rb || pi.lb) & (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
        {
            if (pi.rb)
            {
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            else if (pi.lb && !leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
        }

        if ((pi.rt || pi.lt) && CheckState("ground") || CheckState("attackR") || CheckState("attackL") && canAttack)
        {
            if (pi.rt)
            {
                //do right heavy attack
            }
            else
            {
                if (!leftIsShield)
                {
                    // do left heavy attack
                }
                else
                {
                    anim.SetTrigger("counterBack");
                }
            }
        }


        if (leftIsShield)
        {
            if (CheckState("ground") || CheckState("blocked"))
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
            }
            else if (CheckState("counterBack"))
            {
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
            else
            {
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
            }
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
        }


        if (camController.lockState == false)
        {
            if (pi.inputEnabled == true)
            {
                if (pi.Dmag > 0.1f)
                    model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.2f);
            }

            if (!lockPlanar)
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }
        else
        {
            if (trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }

            if (lockPlanar == false)
            {
                planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        if (pi.action)
        {
            onAction.Invoke();
        }
        if (pi.slot1)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_ONE);
        }
        if (pi.slot2)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_TWO);
        }
        if (pi.slot3)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_THREE);
            am.UseSkill(1);
        }
        if (pi.slot4)
        {
            EventManager.DispatchEvent(EventCommon.SLOT_FOUR);
            am.UseItem(1);
        }
    }

    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
    }
    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }
    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
    public void SetBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }
    /// <summary>
    /// Message Processing Block   通知改
    /// </summary>
    public void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
        trackDirection = true;
    }


    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }
    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    //private void AddListener()
    //{
    //    EventManager.AddListener<GameObject>(EventCommon.ON_GROUND_ENTER, OnGroundEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_GROUND_EXIT, OnGroundExit);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_FALL_ENTER, OnFallEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_ROLL_ENTER, OnRollEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_JAB_ENTER, OnJabEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_JAB_UPDATE, OnJabUpdate);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_ATTACK_1H_ENTER, OnAttack1hAEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_ATTACK_1H_UPDATE, OnAttack1hAUpdate);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_ATTACK_EXIT, OnAttackExit);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_HIT_ENTER, OnHitEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_DIE_ENTER, OnDieEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_BLOCKED_ENTER, OnBlockedEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_STUNNED_ENTER, OnStunnedEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_COUNTER_BACK_ENTER, OnCounterBackEnter);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_COUNTER_BACK_EXIT, OnCounterBackExit);
    //    EventManager.AddListener<GameObject>(EventCommon.ON_LOCK_ENTER, OnLockEnter);
    //}
    //private void RemoveListenr()
    //{
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_GROUND_ENTER, OnGroundEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_GROUND_EXIT, OnGroundExit);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_FALL_ENTER, OnFallEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_ROLL_ENTER, OnRollEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_JAB_ENTER, OnJabEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_JAB_UPDATE, OnJabUpdate);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_ATTACK_1H_ENTER, OnAttack1hAEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_ATTACK_1H_UPDATE, OnAttack1hAUpdate);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_ATTACK_EXIT, OnAttackExit);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_HIT_ENTER, OnHitEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_DIE_ENTER, OnDieEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_BLOCKED_ENTER, OnBlockedEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_STUNNED_ENTER, OnStunnedEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_COUNTER_BACK_ENTER, OnCounterBackEnter);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_COUNTER_BACK_EXIT, OnCounterBackExit);
    //    EventManager.RemoveListener<GameObject>(EventCommon.ON_LOCK_ENTER, OnLockEnter);
    //}
    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }
    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        planarVec += pi.Dvec * walkSpeed * 2;
        thrustVec = new Vector3(0, rollVelocity, 0);
        trackDirection = true;
        lockPlanar = true;
    }
    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }
    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        //lerpTarget = 1.0f;

    }
    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f));
    }

    public void OnAttackExit()
    {

        model.SendMessage("WeaponDisable");

    }
    public void OnHitEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
    public void OnDieEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
    public void OnBlockedEnter()
    {
        pi.inputEnabled = false;
    }
    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC"))
            deltaPos += (0.8f * deltaPos + 0.2f * (Vector3)_deltaPos) / 1.0f;
    }
    public void OnStunnedEnter()
    {
        casterController.gameObject.SetActive(true);
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }
    public void OnStunnedExit()
    {
        casterController.gameObject.SetActive(false);
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
    }
    public void OnCounterBackExit()
    {

        model.SendMessage("CounterBackDisable");
    }
    public void OnLockEnter()
    {
        pi.inputEnabled = false;
        planarVec = Vector3.zero;
        model.SendMessage("WeaponDisable");
    }
}
