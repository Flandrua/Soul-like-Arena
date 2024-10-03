using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wd;
    public string curTag;
    private void Awake()
    {
        initController();
    }
    public void initController()
    {
        wd = GetComponentInChildren<WeaponData>();
    }
    public float GetATK()
    {
        return wd.ATK + wm.am.sm.ATK;
    }
    public float GetDEF() { return wd.DEF; }
    public CharacterClass GetClass() { return wd.pClass; }
}
