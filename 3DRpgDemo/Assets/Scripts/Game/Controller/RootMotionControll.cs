using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControll : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        initController();
    }
    public void initController()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);

    }
}
