using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBone : MonoBehaviour
{
    public SkinnedMeshRenderer srcMeshRenderer;
    public SkinnedMeshRenderer tgtMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        tgtMeshRenderer.bones = srcMeshRenderer.bones;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
