using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{

    private CapsuleCollider defCol;
    // Start is called before the first frame update
    void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Weapon")
        {
            WeaponController targetWc = col.GetComponentInParent<WeaponController>();
            if (targetWc == null) return;
            GameObject attacker = targetWc.wm.am.ac.model.gameObject;
            GameObject receiver = am.ac.model;
            am.TryDoDamage(targetWc, CheckAngleTarger(receiver,attacker,70), CheckAnglePlayer(receiver,attacker,30));
        }
    }
    public static bool CheckAnglePlayer(GameObject player, GameObject target, float playerAngleLimit)
    {
        Vector3 counterDire = target.transform.position - player.transform.position;
        float counterAngel1 = Vector3.Angle(player.transform.forward, counterDire);
        float counterAngel2 = Vector3.Angle(target.transform.forward, player.transform.forward); //shoulde be close to 180
        bool counterValid = (counterAngel1 < playerAngleLimit && Mathf.Abs(counterAngel2 - 180) < playerAngleLimit);
        return counterValid;

    }
    public static bool CheckAngleTarger(GameObject player,GameObject target, float targetAngleLimit) {
        Vector3 attackingDir = player.transform.position - target.transform.position;
        float attackingAngel1 = Vector3.Angle(target.transform.forward, attackingDir);
        bool attackValid = (attackingAngel1 < targetAngleLimit);
        return attackValid;
    }

}