using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControll : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        initTriggerController();
    }
    public void initTriggerController()
    {
        anim = GetComponent<Animator>();
    }
    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
}
