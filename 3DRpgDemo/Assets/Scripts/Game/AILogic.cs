using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILogic : MonoBehaviour
{
    public Transform target;
    public float stopDistance = 2.0f; // AI停止移动的距离阈值
    private NavMeshAgent agent;
    private Vector3 destination;
    private Animator animator;
    private bool isOverridingDestination = false; // 是否暂停更新目标位置
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
            Debug.Log("碰撞到了角色: " + other.gameObject.name);
        }
    }

    private void LookAtTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; // 确保不在垂直（Y轴）方向上旋转
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
        // 确保路径有效且完成
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && !agent.pathPending && !agent.isPathStale)
        {
            // 检查剩余距离是否小于或等于stoppingDistance
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // 检查速度是否接近零
                if (agent.velocity.magnitude < 0.1f)
                {
                    return true; // 代理因为达到 stoppingDistance 而停止
                }
            }
        }
        return false; // 代理不是因为 stoppingDistance 而停止
    }
}
