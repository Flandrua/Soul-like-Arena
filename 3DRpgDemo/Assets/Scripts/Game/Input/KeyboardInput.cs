using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    [Header("====Key Setting====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyRight = "a";
    public string keyLeft = "d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    [Header("====Mouse Settings====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;
    //[Header("====Output Singals====")]
    //public float Dup;
    //public float Dright;
    //public float Dmag;
    //public Vector3 Dvec;
    //public float Jup;
    //public float Jright;

    //public bool run;
    //public bool jump;
    //private bool lastJump;
    //public bool attack;
    //public bool lastAttack;


    //[Header("====Others====")]
    //public bool inputEnabled = true;

    //private float targetDup;
    //private float targetDright;
    //private float velocityDup;
    //private float velocityDright;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (mouseEnable)
        {
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivityY*3;
            Jright = Input.GetAxis("Mouse X") * mouseSensitivityX*2.5f;
        }
        else
        {
            Jup = (Input.GetKey(keyJUp) ? 1f : 0) - (Input.GetKey(keyJDown) ? 1f : 0);
            Jright = (Input.GetKey(keyJRight) ? 1f : 0) - (Input.GetKey(keyJLeft) ? 1f : 0);
        }


        targetDup = (Input.GetKey(keyUp) ? 1f : 0) - (Input.GetKey(keyDown) ? 1f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1f : 0) - (Input.GetKey(keyLeft) ? 1f : 0);
        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(keyA);
        defense = Input.GetKey(keyD);

        bool newJump = Input.GetKey(keyB);
        if (newJump != lastJump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;

        bool newAttack = Input.GetKey(keyC);
        if (newAttack != lastAttack && newAttack == true)
        {
            rb = true;
        }
        else
        {
            rb = false;
        }
        lastAttack = newAttack;
    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output = Vector2.zero;
    //    output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

    //    return output;
    //}
}
