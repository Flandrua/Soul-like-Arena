using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject vfx;
    private Transform _curVFX;
    private ParticleSystem _combo1;
    private ParticleSystem _combo2;
    private ParticleSystem _combo3;
    private ParticleSystem _combo4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayEffect(int order)
    {
        switch (order)
        {
            case 1:
                _combo1.Play(); break;
            case 2:
                _combo2.Play(); break;
            case 3:
                _combo3.Play(); break;
            case 4:
                _combo4.Play(); break;
        }
    }

    public void ResetNewVFX(CharacterClass pClass)
    {
        if (vfx == null) { return; }
        if (_curVFX != null) { _curVFX.gameObject.SetActive(false); }
        ParticleSystem[] particleArray = null;
        if (pClass == CharacterClass.Lancer)
        {
            _curVFX = vfx.transform.Find("Lancer");        
        }
        if (pClass == CharacterClass.Warrior)
        {
            _curVFX = vfx.transform.Find("Warrior");
        }
        particleArray = _curVFX.GetComponentsInChildren<ParticleSystem>();
        _curVFX.gameObject.SetActive(true);
        SetParticle(particleArray);
    }

    private void SetParticle(ParticleSystem[] psArray)
    {

        foreach (ParticleSystem particle in psArray)
        {
            if (particle.name == "1") _combo1 = particle;
            if (particle.name == "2") _combo2 = particle;
            if (particle.name == "3") _combo3 = particle;
            if (particle.name == "4") _combo4 = particle;
        }
    }
}
