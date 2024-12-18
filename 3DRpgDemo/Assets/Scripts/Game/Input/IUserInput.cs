using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("====Output Singals====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;


    public bool run;
    public bool defense;
    public bool jump;
    public bool action;
    protected bool lastJump;
    //public bool attack;
    protected bool lastAttack;
    public bool roll;
    public bool lockon;
    public bool switchRightLock;
    public bool switchLeftLock;
    public bool lb;
    public bool lt;
    /// <summary>
    /// rb���Ҽ��
    /// </summary>
    public bool rb;
    public bool rt;

    public bool slot1;
    public bool slot2;
    public bool slot3;
    public bool slot4;



    [Header("====Others====")]
    public bool inputEnabled = true;

    
   protected float targetDup;
   protected float targetDright;
   protected float velocityDup;
   protected float velocityDright;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

    protected void UpdateDmagDvec(float Dup,float Dirhgt)
    {
        Dmag = Mathf.Sqrt((Dup * Dup) + (Dirhgt * Dirhgt));
        Dvec = Dirhgt * transform.right + Dup * transform.forward;
    }
}
