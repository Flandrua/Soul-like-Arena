using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public WeaponManager wm;
    private WeaponFactory weaponFac;
    /// <summary>
    /// make iot a singleton 
    /// </summary>
    // Start is called before the first frame update

    public override void Init()
    {

        DontDestroyOnLoad(this);

    }

    public void InitWeaponFactory()
    {
        weaponFac = new WeaponFactory(DataManager.Instance.CfgWeapon);
    }
    void Start()
    {
        //LockUnlockCursor();
        InitWeaponFactory();
        wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Sword", "R", wm));
        wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Shield", "L", wm));
        wm.ChangeToLancer(false);

    }
    public void LockUnlockCursor(bool flag = true)
    {
        if (flag) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "R Sword"))
        {
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Sword", "R", wm));
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Shield", "L", wm));
            wm.ChangeToLancer(false);
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "R Pike"))
        {
            wm.UnloadWeapon("L");
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Pike", "R", wm));
            wm.ChangeToLancer(true);
        }
    }




    // Update is called once per frame
    void Update()
    {

    }

}
