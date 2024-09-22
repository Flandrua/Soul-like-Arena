using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : IActorManagerInterface
{
    private CapsuleCollider interCol;
    public List<EventCasterController> overlapEcastms=new List<EventCasterController>();
    // Start is called before the first frame update
    void Start()
    {
        initManager();
    }
    public void initManager()
    {
        interCol = GetComponent<CapsuleCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EventCasterController[] ecasterms = other.GetComponents<EventCasterController>();
        foreach(var ecasterm  in ecasterms)
        {
            if (!overlapEcastms.Contains(ecasterm))
            {
                overlapEcastms.Add(ecasterm);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        EventCasterController[] ecasterms = other.GetComponents<EventCasterController>();
        foreach (var ecasterm in ecasterms)
        {
            if (overlapEcastms.Contains(ecasterm))
            {
                overlapEcastms.Remove(ecasterm);
            }
        }
    }
}
