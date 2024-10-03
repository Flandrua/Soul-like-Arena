using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : IActorManagerInterface
{
    private AudioSource _as;

    public AudioClip hit;
    public AudioClip slayed;
    public AudioClip defend;
    public AudioClip roll;
    public AudioClip step;
    public AudioClip jab;
    // Start is called before the first frame update
    void Start()
    {
        initManager();
    }
    public void initManager()
    {
        _as.GetComponent<AudioSource>();
    }
    public void PlayerAudio(string audio)
    {
        switch (audio)
        {
            case "hit":
                PlayerAudio(hit); break;
            case "slayed":
                PlayerAudio(slayed); break;
            case "defend":
                PlayerAudio(defend); break;
            case "roll":
                PlayerAudio(roll); break;
            case "step":
                PlayerAudio(step); break;
            case "jab":
                PlayerAudio(jab); break;
            default:
                Debug.LogWarning($"can find audio named{audio}"); break;
        }
    }

    private void PlayerAudio(AudioClip audio)
    {
        _as.clip = audio;
        _as.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
