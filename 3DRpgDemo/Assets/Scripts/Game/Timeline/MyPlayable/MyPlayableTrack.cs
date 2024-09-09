using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 1f, 0.01039314f)]
[TrackClipType(typeof(MyPlayableClip))]
[TrackBindingType(typeof(ActorManager))]
public class MyPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MyPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
