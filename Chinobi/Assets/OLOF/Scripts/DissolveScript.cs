using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{

    private MeshRenderer thisMesh;
    public float dissolveTime = 0.5f;
    private float dissolveTimer;
    // Start is called before the first frame update
    void Start()
    {
        thisMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dissolveTimer += Time.deltaTime * (1 / dissolveTime);
        thisMesh.material.SetFloat("_Cutoff", dissolveTimer);
        if (dissolveTimer >= 1)
        {
            Destroy(this.gameObject);
        }
    }
    
}
