using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image hpBall;
    public Image mpBall;
    public Toggle sword;
    public Toggle lance;
    public Toggle skill;
    public Button drug;
    public Text drugText;
    private int drugCount;
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
        EventManager.AddListener<int>(EventCommon.ADD_DRUG, AddDrug);
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
        EventManager.RemoveListener<int>(EventCommon.ADD_DRUG, AddDrug);
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
    public void UseDrug()
    {
        drugCount--;
        drugText.text = $"{drugCount}";
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
        UseDrug();
    }

    private void AddDrug(int count)
    {
        drugCount = count;
    }

}
