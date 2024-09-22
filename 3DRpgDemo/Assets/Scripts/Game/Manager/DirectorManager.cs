using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;
    [Header("===Timeline assets===")]
    public TimelineAsset warriorExecute;
    public TimelineAsset openBox;
    public TimelineAsset leverUp;
    public TimelineAsset lanceExecute;
    [Header("===Assets Settings===")]
    public ActorManager attacker;
    public ActorManager victim;
    // Start is called before the first frame update
    void Start()
    {

        //pd.playableAsset = Instantiate(frontStab);
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "AttackerScrpit")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, attacker);
        //    }
        //    else if (track.streamName == "VictimScrpit")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, victim);
        //    }
        //    else if (track.streamName == "AttackerAnimation")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
        //    }
        //    else if (track.streamName == "VictimAnimation")
        //    {
        //        pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
        //    }
        //}
    }

    public void initManager()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pd.Play();
        }
    }

    public bool IsPlaying()
    {
        if (pd.state == PlayState.Playing) return true;
        else return false;
    }

    public void PlayFrontStab(string timelineName, ActorManager attacker, ActorManager victim)//被处决就会直接死
    {
        //if (pd.playableAsset != null) return;
        //if (pd.state == PlayState.Playing) return;
        if (timelineName == "frontStab")
        {
            if (attacker.wm.wcR.GetClass().Equals(CharacterClass.Warrior))
                pd.playableAsset = Instantiate(warriorExecute);
            if (attacker.wm.wcR.GetClass().Equals(CharacterClass.Lancer))
                pd.playableAsset = Instantiate(lanceExecute);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "AttackerScript")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "VictimScript")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        mybehav.stateLock = true;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);
                    }
                }
                else if (track.name == "AttackerAnimation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "VictimAnimation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
        }
        else if (timelineName == "openBox")
        {
            pd.playableAsset = Instantiate(openBox);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "PlayerScript")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "BoxScript")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);
                    }
                }
                else if (track.name == "PlayerAnimation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "BoxAnimation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
        }
        else if (timelineName == "leverUp")
        {
            pd.playableAsset = Instantiate(leverUp);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "PlayerScript")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "LeverScript")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);
                    }
                }
                else if (track.name == "PlayerAnimation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "LeverAnimation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }
        }

        pd.Evaluate();
        pd.Play();
    }
}

