using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    private Collider weaponColL;
    private Collider weaponColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;

    private void Start()
    {

        try
        {
            whL = transform.DeepFind("weaponHandleL").gameObject;
            wcL = BindWeaponController(whL);
            weaponColL = whL.GetComponentInChildren<Collider>();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(gameObject.name + " has no weaponHandleL ");
        }
        try
        {
            whR = transform.DeepFind("weaponHandleR").gameObject;
            wcR = BindWeaponController(whR);
            weaponColR = whR.GetComponentInChildren<Collider>();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(gameObject.name + " has no weaponHandleR ");
        }
    }

    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }
    public void WeaponEnable()
    {
        if (am.ac.CheckStateTag("attackL"))
        {
            weaponColL.enabled = true;
        }
        else
        {
            weaponColR.enabled = true;
        }

    }
    public void UpdateWeaponCollider(string side,Collider col)
    {
        if (side == "L")
        {
            weaponColL = col;
        }
        else if(side == "R")
        {
            weaponColR = col;
        }
    }

    public void UnloadWeapon(string side)
    {
        if(side=="L")
        {
            foreach( Transform trans in whL.transform)
            {
                weaponColL = null;
                wcL.wd = null;
                Destroy(trans.gameObject);
            }
        }
        else if (side == "R")
        {
            foreach (Transform trans in whR.transform)
            {
                weaponColR=null;
                wcR.wd = null;
                Destroy(trans.gameObject);
            }
        }
    }
    public void WeaponDisable()
    {
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }
    public void CounterBackEnable()
    {
        am.sm.SetIsCounterBack(true);
    }
    public void CounterBackDisable()
    {
        am.sm.SetIsCounterBack(false);
     
    }

    public void ChangeDualHands(bool dualOn)
    {
       am.ChangeDualHands(dualOn);
    }
}
