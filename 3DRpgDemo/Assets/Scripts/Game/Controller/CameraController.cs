using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private IUserInput pi;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80f;
    public float cameraDampValue = 0.5f;
    public Image lockDot;
    public bool lockState = false;
    public bool isAI = false;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;

    private bool switchFlag = true;
    private GameObject model;
    private GameObject camera;
    private Vector3 cameraDampVelocity;

    public Vector3 boxSize = new Vector3(0.5f, 0.5f, 5f);
    [SerializeField]
    private LockTarget lockTarget;
    // Start is called before the first frame update
    private void Start()
    {
        initController();
        if (!isAI)
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
        }
        lockState = false;


    }

    public void initController()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;
            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
            tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);

            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            if (pi.Jright > 0.9f && switchFlag)//如果没有时间间隔是否会导致目标切换太快？新功能导致不解锁
            {
                SwitchFlag();
                //向右切换目标
                LockTarget lt = GetLeftRightTarget(true);
                if (lt == null) return;
                LockProcessA(lt, true, true, isAI);
            }
            else if (pi.Jright < -0.9f && switchFlag)
            {
                SwitchFlag();
                //向左切换目标
                LockTarget rt = GetLeftRightTarget(false);
                if (rt == null) return;
                LockProcessA(rt, true, true, isAI);
            }
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }
        //camera.transform.position = Vector3.Lerp(camera.transform.position,  transform.position,0.2f);
        if (!isAI)
        {
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
            //camera.transform.eulerAngles = transform.eulerAngles;
            camera.transform.LookAt(cameraHandle.transform);
        }
    }
    private void SwitchFlag()
    {
        switchFlag = false;
        TimeManager.Instance.AddTask(0.2f, false, () => { switchFlag = true; }, this);
    }
    private LockTarget GetLeftRightTarget(bool isRight)
    {
        Collider[] cols = GetColsByCamera();
        float closestLeftAngel = float.MaxValue;
        float closestRightAngel = float.MaxValue;
        Collider leftTarget = null;
        Collider rightTarget = null;
        Vector3 camOrigin1 = camera.transform.position; // 相机的位置
        Vector3 camRight = camera.transform.right; // 相机的右向量
        Vector3 player = model.transform.position; // 锁定目标的位置
        foreach (var col in cols)
        {
            if (col.gameObject == lockTarget.obj) continue;
            Collider colTransform = col;
            Vector3 directionToTarget = colTransform.transform.position - player; // 从锁定目标到碰撞体的方向
            //float distance = directionToTarget.magnitude; // 距离                                                     

            float checkAngel = Vector3.Angle(model.transform.forward, directionToTarget);  //人物与目标的夹角来限定目标范围

            float side = Vector3.Dot(camRight, directionToTarget);   // 判断碰撞体在锁定目标的左侧还是右侧

            if (side < 0)
            {
                // 在左侧
                if (checkAngel < closestLeftAngel)
                {
                    closestLeftAngel = checkAngel;
                    leftTarget = colTransform;
                }
            }
            else
            {
                // 在右侧
                if (checkAngel < closestRightAngel)
                {
                    closestRightAngel = checkAngel;
                    rightTarget = colTransform;
                }
            }
        }
        if (isRight)
        {
            if (rightTarget == null) return null;
            return new LockTarget(rightTarget.gameObject, rightTarget.bounds.extents.y);
        }
        else
        {
            if (leftTarget == null) return null;
            return new LockTarget(leftTarget.gameObject, leftTarget.bounds.extents.y);
        }


    }

    private Collider[] GetColsByCamera()
    {
        //以相机视线基础，实现锁定
        //实际上应该用射线和数学来算 raycast  把范围内的所有单位放进数组，然后按角度来算
        Vector3 camOrigin1 = camera.transform.position;
        return Physics.OverlapBox(camOrigin1, boxSize, camera.transform.rotation, LayerMask.GetMask(isAI ? "Player" : "Enemy"));
    }

    public void LockUnlock()
    {
        //按右摇杆，转换锁定目标

        //以模型实视线基础，实现锁定
        //Vector3 modelOrigin1 = model.transform.position;
        //Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        //Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        //Collider[] cols = Physics.OverlapBox(boxCenter, boxSize, model.transform.rotation, LayerMask.GetMask(isAI ? "Player" : "Enemy"));

        Collider[] cols = GetColsByCamera();
        if (cols.Length == 0)
        {
            LockProcessA(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    LockProcessA(null, false, false, isAI);
                    return;
                }
            }
            LockProcessA(new LockTarget(cols[0].gameObject, cols[0].bounds.extents.y), true, true, isAI);

        }
    }

    // 在Scene视图中绘制Box
    void OnDrawGizmos()
    {
        if (isAI) return;

        if (Camera.main != null)
        {
            // 设置Gizmos颜色
            Gizmos.color = Color.green;

            // 获取摄像机位置和旋转
            Vector3 boxCenter = Camera.main.transform.position;
            boxCenter.z += boxSize.z / 2;
            Vector3 boxRotation = Camera.main.transform.rotation.eulerAngles;
            Quaternion boxOrientation = Quaternion.Euler(boxRotation);

            // 绘制OverlapBox的线框
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxOrientation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        }

    }



    private void Update()
    {
        if (lockTarget != null)
        {
            if (!isAI)
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
            {
                LockProcessA(null, false, false, isAI);
            }
            else if (lockTarget.am != null && lockTarget.am.sm.isDie == true)
            {
                LockProcessA(null, false, false, isAI);
            }
        }
    }
    private void LockProcessA(LockTarget _locktarget, bool _lockDotEnable, bool _lockState, bool _isAI)
    {
        lockTarget = _locktarget;
        if (!_isAI)
            lockDot.enabled = _lockDotEnable;
        lockState = _lockState;
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public ActorManager am;

        public LockTarget(GameObject _obj, float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            am = _obj.GetComponent<ActorManager>();
        }

    }
}
