using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //移动速度
    public float speed = 8.0f;
    //重力
    const float GRAVITY = 9.8f;
    //速度向量
    public Vector3 velocity = Vector3.zero;
    //跳跃高度
    public float jumpHeight = 1.2f;

    private float runSpeed;
    private float walkSpeed;
    public KeyCode runCode = KeyCode.LeftShift;


    private CharacterController characterController;
    private Animator animator;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        runSpeed = speed * 2;
        walkSpeed = speed;
    }
    private void Update()
    {
        //移动更新函数
        Move_Update();
        //高度更新函数
        Height_Update();
        //Move方法
        characterController.Move(velocity * Time.deltaTime);
        //攻击
        Attack_Update();
    }

    public void Move_Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        velocity.x = dir.x * speed;
        velocity.z = dir.z * speed;
        if (Input.GetKey(runCode))
        {
            if (speed < runSpeed)
                speed += Time.deltaTime * 5;
        }
        else
        {
            if (speed > walkSpeed)
                speed -= Time.deltaTime * 5;

        }

        //Debug.Log(velocity);
        animator.SetFloat("VelocityZ", velocity.z);
        animator.SetFloat("VelocityX", velocity.x);
    }

    public void Height_Update()
    {

        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //v平方 = 2gh
                velocity.y = Mathf.Sqrt(2 * GRAVITY * jumpHeight);
            }

            // 如果角色接触地面，则高度置零，这里设置-1而不是0，是因为有时候0无法起跳。
            if (velocity.y < -1)
                velocity.y = -1;
        }
        else
        {
            // 如果角色不在地面，则不断减去高度。
            //v=gt 自由落体运动
            velocity.y -= GRAVITY * Time.deltaTime;
        }

    }
    public void Attack_Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetInteger("action", 2);
        }
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetInteger("action", 1);
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            animator.SetInteger("action", 0);
        }

    }


}