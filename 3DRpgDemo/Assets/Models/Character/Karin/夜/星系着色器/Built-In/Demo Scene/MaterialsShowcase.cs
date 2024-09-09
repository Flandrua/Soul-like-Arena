using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsShowcase : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    public List<Material> materials;
    public float timewait = 1; 

    private int index =0;
    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > timewait)
        {
            time -= timewait;
            mesh.material = Instantiate(materials[index % materials.Count]);
            index++;
            Debug.Log(index % materials.Count);
        }
    }


}
