using Configs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponFactory
{
    private string path = "WeaponPrefab/";
    private CfgWeapon weaponDB;
    public WeaponFactory(CfgWeapon cfgWeapon)
    {

        weaponDB = cfgWeapon;

    }
    public Collider CreateWeapon(string weaponName, string side,WeaponManager wm,string tag)
    {
        WeaponController wc;
        if (side == "L")
        {
            wc = wm.wcL;
        }
        else if(side == "R")
        {
            wc = wm.wcR;
        }
        else
        {
            return null;
        }
        GameObject preafab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(preafab);
        obj.transform.parent = wc.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        ApplyWeaponData(obj, weaponName,wc,tag);
        return obj.GetComponent<Collider>();

    }

    private void ApplyWeaponData(GameObject obj,string weaponName,WeaponController wc,string tag)
    {
        WeaponData wd = obj.AddComponent<WeaponData>();
        foreach (var weapon in weaponDB.mDataMap.Values)
        {
            if (path + weapon.desc == weaponName)
            {
                wd.ATK = weapon.ATK;
                wd.DEF = weapon.DEF;
                wd.pClass = (CharacterClass)System.Enum.Parse(typeof(CharacterClass), weapon.Pclass);
            }
        }

       wc.wd = wd;
        wc.curTag = tag;
        if (wd.ATK == -1) Debug.LogError(weaponName + " cant find data");
    }

}
