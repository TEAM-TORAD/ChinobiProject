using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Vector3 target;
    private void Update()
    {
        RayCheck();
        transform.LookAt(target);
    }
    void RayCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);
        target = hit.point;
    }
}
