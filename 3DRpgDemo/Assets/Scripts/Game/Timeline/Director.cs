using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Director : MonoBehaviour
{
    public PlayableDirector pd;

    public Animator attacker;
    public Animator victim;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (var track in pd.playableAsset.outputs)
            {
                if (track.streamName == "AttackerAnimation")
                {
                    pd.SetGenericBinding(track.sourceObject, attacker);
                }
                else if(track.streamName == "VictimAnimation")
                {
                    pd.SetGenericBinding(track.sourceObject, victim);
                }

            }

            pd.Play();
        }
    }
}
