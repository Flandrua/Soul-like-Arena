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
        wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Falchion", "R", wm));
        wm.ChangeDualHands(false);

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
            wm.ChangeDualHands(false);
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "R Halberd"))
        {
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Halberd", "R", wm));
            wm.ChangeDualHands(true);
        }
        if (GUI.Button(new Rect(10, 90, 150, 30), "R Mace"))
        {
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Mace", "R", wm));
            wm.ChangeDualHands(false);
        }
        if (GUI.Button(new Rect(10, 130, 150, 30), "R Clear"))
        {
            wm.UnloadWeapon("R");
            wm.ChangeDualHands(false);
        }
    }




    // Update is called once per frame
    void Update()
    {

    }

}
