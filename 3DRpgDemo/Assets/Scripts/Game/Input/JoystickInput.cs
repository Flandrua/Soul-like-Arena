using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    [Header("====Joystick Settings====")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axisJright = "axis4";
    public string axisJup = "axis5";
    public string axisCright = "axis6";
    public string axisCup = "axis7";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnX = "btn2";
    public string btnY = "btn3";
    public string btnJstick = "btn9";

    public string btnLB = "btn4";
    public string btnRB = "btn5";
    public string axisLT = "axis9";
    public string axisRT = "axis10";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonX = new MyButton();
    public MyButton buttonY = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonRT = new MyButton();
    public MyButton buttonJstick = new MyButton();

    public MyButton buttonCup = new MyButton();
    public MyButton buttonCdown = new MyButton();
    public MyButton buttonCright = new MyButton();
    public MyButton buttonCleft = new MyButton();



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
    // Update is called once per frame
    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonX.Tick(Input.GetButton(btnX));
        buttonY.Tick(Input.GetButton(btnY));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonLT.Tick(Input.GetAxis(axisLT) >= 0.9f ? true : false);
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonRT.Tick(Input.GetAxis(axisRT) >= 0.9f ? true : false);
        buttonJstick.Tick(Input.GetButton(btnJstick));
        buttonCup.Tick(Input.GetAxis(axisCup) >= 0.9f ? true : false);
        buttonCright.Tick(Input.GetAxis(axisCright) >= 0.9f ? true : false);
        buttonCleft.Tick(Input.GetAxis(axisCright) <= -0.9f ? true : false);
        buttonCdown.Tick(Input.GetAxis(axisCup) <= -0.9f ? true : false);


        if (UIManager.Instance.popWins.activeInHierarchy)
        {
            if (buttonA.OnPressed) EventManager.DispatchEvent(EventCommon.CONFIRM_INDEX);
            if (buttonCup.OnPressed) EventManager.DispatchEvent<int>(EventCommon.POP_INDEX, -1);
            if (buttonCdown.OnPressed) EventManager.DispatchEvent<int>(EventCommon.POP_INDEX, 1);
        }
        else
        {
            Jup = -1 * Input.GetAxis(axisJup);
            Jright = Input.GetAxis(axisJright);


            targetDup = Input.GetAxis(axisY);
            targetDright = Input.GetAxis(axisX);
            if (!inputEnabled)
            {
                targetDup = 0;
                targetDright = 0;
            }

            Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
            Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

            //Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
            //float Dright2 = tempDAxis.x;
            //float Dup2 = tempDAxis.y;

            //Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
            //Dvec = Dright2 * transform.right + Dup2 * transform.forward;

            Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
            Dvec = Dright * transform.right + Dup * transform.forward;


            run = buttonA.IsPressing;
            defense = buttonLB.IsPressing;
            jump = buttonB.OnPressed;
            action = buttonX.OnPressed;
            rb = buttonRB.OnPressed;
            rt = buttonRT.OnPressed;
            lb = buttonLB.OnPressed;
            lt = buttonLT.OnPressed;
            lockon = buttonJstick.OnPressed;
            slot1 = buttonCup.OnPressed;
            slot2 = buttonCleft.OnPressed;
            slot3 = buttonCdown.OnPressed;
            slot4 = buttonCright.OnPressed;
        }
    }

}
