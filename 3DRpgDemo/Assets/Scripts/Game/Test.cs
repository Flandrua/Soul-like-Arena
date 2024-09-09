using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataCenter.Instance.GameData.MainPlayer.hp = 10;
        Debug.Log(DataCenter.Instance.GameData.MainPlayer.hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
