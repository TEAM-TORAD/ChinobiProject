using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private Rigidbody rb;
    public float force;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, force * Time.deltaTime, 0));
    }
}
