using Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public ActorManager playerAM;
    public WeaponManager playerWM;
    public GameObject spawn1;
    public GameObject spawn2;
    public EnemyPool enemyPool;

    public string enemyTag = "Enemy";
    public string playerTag = "Player";
    private WeaponFactory weaponFac;
    private EnemyFactory enemyFac;
    private int Wave = 0;
    private int spawnPoint = 1;
    private int aliveAI = 0;
    /// <summary>
    /// make iot a singleton 
    /// </summary>
    // Start is called before the first frame update

    public override void Init()
    {
        EventManager.AddListener(EventCommon.SLOT_ONE, SlotOne);
        EventManager.AddListener(EventCommon.SLOT_TWO, SlotTwo);

        EventManager.AddListener(EventCommon.CHOOSE_ATK, ChooseATK);
        EventManager.AddListener(EventCommon.CHOOSE_HPMAX, ChooseHPMax);
        EventManager.AddListener(EventCommon.CHOOSE_MPMAX, ChooseMPMax);
        EventManager.AddListener(EventCommon.CHOOSE_POTION, ChoosePotion);
        EventManager.AddListener(EventCommon.NEXT_WAVE, NextWave);




    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(EventCommon.SLOT_ONE, SlotOne);
        EventManager.RemoveListener(EventCommon.SLOT_TWO, SlotTwo);

        EventManager.RemoveListener(EventCommon.CHOOSE_ATK, ChooseATK);
        EventManager.RemoveListener(EventCommon.CHOOSE_HPMAX, ChooseHPMax);
        EventManager.RemoveListener(EventCommon.CHOOSE_MPMAX, ChooseMPMax);
        EventManager.RemoveListener(EventCommon.CHOOSE_POTION, ChoosePotion);
        EventManager.RemoveListener(EventCommon.NEXT_WAVE, NextWave);
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
        CreateWeapon(playerWM, false, playerTag);

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
            CreateWeapon(playerWM, false, playerTag);
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "R Pike"))
        {
            CreateWeapon(playerWM, true, playerTag);
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
            ActorManager am = enemy.GetComponent<ActorManager>();
            WeaponManager wm = am.wm;
            VFXController vfxc = am.ac.vfxController;
            DummyIUserInput dummyIUserInput = enemy.GetComponent<DummyIUserInput>();
            dummyIUserInput.player = playerWM.transform.parent.GetComponent<ActorManager>();
            switch (pClass)
            {
                case CharacterClass.Warrior:
                    CreateWeapon(wm, false, enemyTag);
                    break;
                case CharacterClass.Lancer:
                    CreateWeapon(wm, true, enemyTag);
                    break;
            }
        }
        enemy.transform.SetParent(parent.transform);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
    }

    public void CreateWeapon(WeaponManager wm, bool isLancer, string tag)
    {
        if (isLancer)
        {
            wm.UnloadWeapon("L");
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Pike", "R", wm, tag));
            wm.ChangeToLancer(true);
        }
        else
        {
            wm.UnloadWeapon("R");
            wm.UpdateWeaponCollider("R", weaponFac.CreateWeapon("WeaponPrefab/Sword", "R", wm, tag));
            wm.UpdateWeaponCollider("L", weaponFac.CreateWeapon("WeaponPrefab/Shield", "L", wm, tag));
            wm.ChangeToLancer(false);
        }
    }

    public void SlotOne()
    {
        CreateWeapon(playerWM, false, playerTag);
    }
    public void SlotTwo()
    {
        CreateWeapon(playerWM, true, playerTag);
    }

    private void ChooseATK()
    {
        playerAM.sm.ATK += 5;
    }
    private void ChooseHPMax()
    {
        playerAM.sm.HPMax += 20;
    }
    private void ChooseMPMax()
    {
        playerAM.sm.MPMax += 10;
    }
    private void ChoosePotion()
    {
        DataCenter.Instance.AddItem(1, 3);
    }
    private void NextWave()
    {
        Wave++;
        aliveAI = Wave;
        CfgEnemy cfgEnemy = DataManager.Instance.CfgEnemy;
        for (int i = 0; i < Wave; i++)
        {
            int randomValue = UnityEngine.Random.Range(0, cfgEnemy.mDataMap.Count);
            EnemyData enemy = cfgEnemy.mDataMap[$"{randomValue + 1}"];
            if (Enum.TryParse(enemy.pClass, out CharacterClass result))
            {
                GameObject spawnPos = spawn1;
                if (spawnPoint == 2)
                {
                    spawnPos = spawn2;
                    spawnPoint = 1;
                }
                else { spawnPoint = 2; }
                CreateEnemy(enemy.name, result, spawnPos);
            }

        }
    }

    public void CheckAIExist()
    {
        aliveAI--;
        if (aliveAI == 0) EventManager.DispatchEvent(EventCommon.POP_WINDOW);
    }
    // Update is called once per frame
    void Update()
    {

    }

}
