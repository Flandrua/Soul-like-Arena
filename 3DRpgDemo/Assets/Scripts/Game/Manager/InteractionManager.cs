using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : IActorManagerInterface
{
    private CapsuleCollider interCol;
    public List<EventCasterManager> overlapEcastms=new List<EventCasterManager>();
    // Start is called before the first frame update
    void Start()
    {
        interCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EventCasterManager[] ecasterms = other.GetComponents<EventCasterManager>();
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
        EventCasterManager[] ecasterms = other.GetComponents<EventCasterManager>();
        foreach (var ecasterm in ecasterms)
        {
            if (overlapEcastms.Contains(ecasterm))
            {
                overlapEcastms.Remove(ecasterm);
            }
        }
    }
}
