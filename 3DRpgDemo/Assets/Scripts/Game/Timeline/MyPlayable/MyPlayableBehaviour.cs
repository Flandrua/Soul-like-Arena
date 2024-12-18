using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviour : PlayableBehaviour
{
    public ActorManager am;
    public float myFloat;
    public bool stateLock = false;  
    //PlayableDirector pd;
    public override void OnPlayableCreate(Playable playable)
    {

    }

    public override void OnGraphStart(Playable playable)
    {
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();

    }
    public override void OnGraphStop(Playable playable)
    {
        //pd.playableAsset = null;
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (stateLock) {
            am.DieByDM();
            return; 
        }
        am.LockUnlockActorController(false);
    }
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        am.LockUnlockActorController(true);
    }

}
