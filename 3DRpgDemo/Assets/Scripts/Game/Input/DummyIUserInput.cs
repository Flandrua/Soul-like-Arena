using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyIUserInput : IUserInput
{
    private float curDistance = float.MaxValue;
    public ActorManager player;
    public NavMeshAgent navAgent;
    //Start is called before the first frame update
    private void Start()
    {
        InitAI();
    }
    public void InitAI()
    {
        navAgent.updatePosition = false;       
    }
    // Update is called once per frame
    void Update()
    {
        curDistance = Vector3.Distance(transform.position, player.transform.position);
        AILogic();
        //UpdateDmagDvec(Dup, Dright);
    }


    private void AILogic()
    {
        if(!gameObject.activeInHierarchy)return;
        if (!navAgent.isOnNavMesh)
        {
            navAgent.Warp(new Vector3(-33.9f, 8.410954f, 32.48f));
        }
        if (!inputEnabled)
        {
            Dmag = 0f;
            run = false;
            navAgent.isStopped = true; // ֹͣ�ƶ�
            return;
        }
        navAgent.SetDestination(player.transform.position); // ����Ŀ��λ��Ϊ���
        //�ƶ���player
        if (curDistance <= navAgent.stoppingDistance)
        {
            Dmag = 0f;
            run = false;
            navAgent.isStopped = true; // ֹͣ�ƶ�
            rb = true; // ����rbΪtrue
        }
        // ����������stoppingDistance + 0.5f�������ƶ������
        else if (curDistance > (navAgent.stoppingDistance + 0.5f))
        {
            Dmag = 1.0f;
            run = true;
            navAgent.isStopped = false; // �����ƶ�
            navAgent.nextPosition = transform.position;
            rb = false; // ����rbΪfalse
        }
    }
}
