using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCasterController : IActorManagerInterface
{
    public string eventName;
    public bool active = false;
    public Vector3 offset = new Vector3 (0, 0, 0.5f);


    // Start is called before the first frame update
    void Start()
    {

    }
    public void initController()
    {
        if (am == null)
        {
            am = GetComponentInParent<ActorManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
