using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILogic : MonoBehaviour
{
    public Transform target;
    public float stopDistance = 2.0f; // AIֹͣ�ƶ��ľ�����ֵ
    private NavMeshAgent agent;
    private Vector3 destination;
    private Animator animator;
    private bool isOverridingDestination = false; // �Ƿ���ͣ����Ŀ��λ��
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        destination = target.position;
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAgentStoppedByStoppingDistance())
            LookAtTarget();
        if (target.position != destination)
            UpdateTargetPos();
        UpdateAnimatorParameters();
    }
    void UpdateAnimatorParameters()
    {
        animator.SetFloat("VelocityZ", Mathf.Abs(agent.velocity.z));
        animator.SetFloat("VelocityX", Mathf.Abs(agent.velocity.x));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>())
        {
            Debug.Log("��ײ���˽�ɫ: " + other.gameObject.name);
        }
    }

    private void LookAtTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; // ȷ�����ڴ�ֱ��Y�ᣩ��������ת
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void UpdateTargetPos()
    {
        destination = target.position;
        agent.SetDestination(destination);
    }

    private bool IsAgentStoppedByStoppingDistance()
    {
        // ȷ��·����Ч�����
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && !agent.pathPending && !agent.isPathStale)
        {
            // ���ʣ������Ƿ�С�ڻ����stoppingDistance
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // ����ٶ��Ƿ�ӽ���
                if (agent.velocity.magnitude < 0.1f)
                {
                    return true; // ������Ϊ�ﵽ stoppingDistance ��ֹͣ
                }
            }
        }
        return false; // ��������Ϊ stoppingDistance ��ֹͣ
    }
}
