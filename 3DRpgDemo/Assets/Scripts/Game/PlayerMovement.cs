using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //�ƶ��ٶ�
    public float speed = 8.0f;
    //����
    const float GRAVITY = 9.8f;
    //�ٶ�����
    public Vector3 velocity = Vector3.zero;
    //��Ծ�߶�
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
        //�ƶ����º���
        Move_Update();
        //�߶ȸ��º���
        Height_Update();
        //Move����
        characterController.Move(velocity * Time.deltaTime);
        //����
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
                //vƽ�� = 2gh
                velocity.y = Mathf.Sqrt(2 * GRAVITY * jumpHeight);
            }

            // �����ɫ�Ӵ����棬��߶����㣬��������-1������0������Ϊ��ʱ��0�޷�������
            if (velocity.y < -1)
                velocity.y = -1;
        }
        else
        {
            // �����ɫ���ڵ��棬�򲻶ϼ�ȥ�߶ȡ�
            //v=gt ���������˶�
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