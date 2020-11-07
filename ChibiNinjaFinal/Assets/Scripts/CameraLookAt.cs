using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [HideInInspector]
    public static CameraLookAt instance = null;
    private Transform target;
    private bool active;
    public float rotationSpeed = 5.0f, heightOffset = 2.0f;
    private ThirdPersonOrbitCamBasic orbitCam;
    // Start is called before the first frame update
    void Awake ()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        orbitCam = transform.GetComponent<ThirdPersonOrbitCamBasic>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (active) LookAt(target);
    }
    public void Activate(Transform _target)
    {
        target = _target;
        //orbitCam.enabled = false;
        active = true;
    }
    public void Deactivate()
    {
        active = false;
        target = null;
        //orbitCam.enabled = true;
    }
    void LookAt(Transform _target)
    {
        Vector3 cameraTarget = _target.position;
        cameraTarget.y += heightOffset;

        // Determine which direction to rotate towards
        Vector3 targetDirection = cameraTarget - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
