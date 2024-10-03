using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static ActorController;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class UIManager : MonoSingleton<UIManager>
{
    // Start is called before the first frame update
    public Image hpBall;
    public Image mpBall;
    public Toggle sword;
    public Toggle lance;
    public Toggle skill;
    public Button potion;
    public Text potionText;



    //===Level clear===


    public GameObject popWins;
    public Toggle ATK;
    public Toggle HP;
    public Toggle MP;
    public Toggle Potion;

    private Toggle[] contrlArray;
    private int popIndex = 1;

    void Start()
    {
        EventManager.AddListener<float[]>(EventCommon.UPDATE_HP, UpdateHP);
        EventManager.AddListener<float[]>(EventCommon.UPDATE_MP, UpdateMP);
        EventManager.AddListener<string>(EventCommon.CHANGE_WEAPON, ChangeWeapon);
        EventManager.AddListener(EventCommon.USE_SKILL, UseSkill);

        EventManager.AddListener(EventCommon.SLOT_ONE, SlotOne);
        EventManager.AddListener(EventCommon.SLOT_TWO, SlotTwo);
        EventManager.AddListener(EventCommon.SLOT_THREE, SlotThree);
        EventManager.AddListener(EventCommon.SLOT_FOUR, SlotFour);
        EventManager.AddListener(EventCommon.ADD_POTION, ChangePotion);

        EventManager.AddListener(EventCommon.POP_WINDOW, PopWindow);
        EventManager.AddListener<int>(EventCommon.POP_INDEX, PopIndex);
        EventManager.AddListener(EventCommon.CONFIRM_INDEX, ConfirmIndex);

        contrlArray = new Toggle[4] {ATK,HP,MP, Potion };

    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<float[]>(EventCommon.UPDATE_HP, UpdateHP);
        EventManager.RemoveListener<float[]>(EventCommon.UPDATE_MP, UpdateMP);
        EventManager.RemoveListener<string>(EventCommon.CHANGE_WEAPON, ChangeWeapon);
        EventManager.RemoveListener(EventCommon.USE_SKILL, UseSkill);

        EventManager.RemoveListener(EventCommon.SLOT_ONE, SlotOne);
        EventManager.RemoveListener(EventCommon.SLOT_TWO, SlotTwo);
        EventManager.RemoveListener(EventCommon.SLOT_THREE, SlotThree);
        EventManager.RemoveListener(EventCommon.SLOT_FOUR, SlotFour);
        EventManager.RemoveListener(EventCommon.ADD_POTION, ChangePotion);

        EventManager.RemoveListener(EventCommon.POP_WINDOW, PopWindow);
        EventManager.RemoveListener<int>(EventCommon.POP_INDEX, PopIndex);
        EventManager.RemoveListener(EventCommon.CONFIRM_INDEX, ConfirmIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHP(float[] hp)//0是当前生命值，1是最大生命值
    {
        hpBall.fillAmount = hp[0] / hp[1];
    }
    public void UpdateMP(float[] mp)
    {
        mpBall.fillAmount = mp[0] / mp[1];
    }
    public void ChangeWeapon(string name)
    {
        if (name == "sword" && !sword.isOn)
        {
            sword.isOn = true;
            sword.Select();
        }
        else if (name == "lance" && !lance.isOn)
        {
            lance.isOn = true;
            lance.Select();
        }
    }
    public void UseSkill()
    {
        if (skill.isOn)
            skill.isOn = false;
        else
            skill.isOn = true;
        skill.Select();
    }
    public void ChangePotion()
    {
        potionText.text = $"{DataCenter.Instance.GameData.MainPlayer.itemBag.Count}";
    }


    private void SlotOne()
    {
        ChangeWeapon("sword");
    }

    private void SlotTwo()
    {
        ChangeWeapon("lance");
    }
    private void SlotThree()
    {
        UseSkill();
    }
    private void SlotFour()
    {
        ChangePotion();
    }

    private void PopWindow()
    {
        if (popWins.activeInHierarchy)
            popWins.SetActive(false);
        else
            popWins.SetActive(true);
    }
    private void PopIndex(int index)
    {
        popIndex += index;
        if (popIndex > contrlArray.Length-1)
            popIndex = 0;
        else if(popIndex<0)
            popIndex  = contrlArray.Length-1;

        else if (popIndex == contrlArray.Length)
            popIndex = 1;

        for (int i = 0; i < contrlArray.Length; i++)
        {
            if (i == popIndex)
            {
                contrlArray[i].isOn = true;
            }
            else
            {
                contrlArray[i].isOn = false;
            }
        }
    }

    public void OnPopToggleClick(int newIndex) {
        popIndex= newIndex;
        ConfirmIndex();
    }
    public void OnPointerEnter()
    {
        for (int i = 0; i < contrlArray.Length; i++)
        {
                contrlArray[i].isOn = false;
        }
    }
    private void ConfirmIndex()
    {
        PopWindow();

        switch (popIndex)
        {
            case 0:
                EventManager.DispatchEvent(EventCommon.CHOOSE_ATK); break;
            case 1:
                EventManager.DispatchEvent(EventCommon.CHOOSE_HPMAX); break;
            case 2:
                EventManager.DispatchEvent(EventCommon.CHOOSE_MPMAX); break;
            case 3:
                EventManager.DispatchEvent(EventCommon.CHOOSE_POTION); break;
        }

        EventManager.DispatchEvent(EventCommon.NEXT_WAVE);
    }
}
