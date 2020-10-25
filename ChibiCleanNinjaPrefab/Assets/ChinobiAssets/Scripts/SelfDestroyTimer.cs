using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyTimer : MonoBehaviour
{
    public float maxLifeTime;
    private float lifeTime;

    private void Update()
    {
        lifeTime += 0.1f * Time.deltaTime;
        if (lifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
