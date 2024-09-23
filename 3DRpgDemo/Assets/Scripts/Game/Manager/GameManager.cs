using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public WeaponManager playerWM;
    public GameObject spawn1;
    public GameObject spawn2;
    public EnemyPool enemyPool;
    private WeaponFactory weaponFac;
    private EnemyFactory enemyFac;
    /// <summary>
    /// make iot a singleton 
    /// </summary>
    // Start is called before the first frame update

    public override void Init()
    {



    }

    public void InitWeaponFactory()
    {
        weaponFac = new WeaponFactory(DataManager.Instance.CfgWeapon);
    }
    public void InitEnemyFactory()
    {
        enemyFac = new EnemyFactory(DataManager.Instance.CfgEnemy);
    }
    void Start()
    {
        //LockUnlockCursor();
        InitWeaponFactory();
        InitEnemyFactory();
        CreateWeapon(playerWM, false);

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
            CreateWeapon(playerWM, false);
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "R Pike"))
        {
            CreateWeapon(playerWM, true);
        }
        if (GUI.Button(new Rect(10, 90, 150, 30), "SpawnEnemyRed"))
        {
            CreateEnemy("ybotRed", CharacterClass.Warrior, spawn1);
        }
    }

    public void CreateEnemy(string enemyName, CharacterClass pClass, GameObject parent)
    {
        GameObject enemy = enemyPool.GetEnemy(enemyName);
        if (enemy == null)
        {
            enemy = enemyFac.CreateEnemy(enemyName);
            WeaponManager wm = enemy.GetComponent<ActorManager>().wm;
            switch (pClass)
            {
                case CharacterClass.Warrior:
                    CreateWeapon(wm, false);
                    break;
                case CharacterClass.Lancer:
                    CreateWeapon(wm, true);
                    break;
            }
        }
        enemy.transform.SetParent(parent.transform);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
    }

    public void CreateWeapon(WeaponManager wm, bool isLancer)
    {
        if (isLancer)
        {
            wm.UnloadWeapon("L");
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Pike", "R", wm));
            wm.ChangeToLancer(true);
        }
        else
        {
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Sword", "R", wm));
            wm.UpdateWeaponCollider("L", weaponFac.CreateWeapon("WeaponPrefab/Shield", "L", wm));
            wm.ChangeToLancer(false);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
