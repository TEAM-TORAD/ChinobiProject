using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugTurner : MonoBehaviour
{
    public WheelTurner wheelTurner;
    private Transform player;
    private Transform angleChecker;
    public Transform targetSocket;
    private bool pickedUp = false, canInteract = true;
    private Rigidbody RB;
    public Transform playerLeftHand;
    private MeshCollider coll;
    private bool hintPrinted = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        angleChecker = transform.Find("AngleChecker");
        RB = GetComponent<Rigidbody>();
        coll = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hintPrinted && CheckAngle())
        {
            hintPrinted = true;
            Economy.economy.InstantiateServerMessage("Press 'E' to pick up the Key!");
        }
        if (!pickedUp && canInteract)
        {
            
            if(Input.GetKeyDown(KeyCode.E) && CheckAngle())
            {
                // Pick up the object
                pickedUp = true;
                transform.parent = playerLeftHand;
                transform.position = playerLeftHand.position;
                RB.isKinematic = true;
                RB.useGravity = false;
                coll.enabled = false;
            }
        }
    }
    private void Place()
    {
        transform.parent = targetSocket.GetChild(0);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        wheelTurner.locked = false;
        pickedUp = false;
        canInteract = false;
    }
    private bool CheckAngle()
    {
        float distance = Vector3.Distance(angleChecker.position, player.position);
        angleChecker.position = new Vector3(angleChecker.position.x, player.position.y, angleChecker.position.z);
        angleChecker.rotation = Quaternion.Euler(0, 0, 0);
        Vector3 targetDir = angleChecker.position - player.position;
        float angleView = Vector3.Angle(targetDir, player.forward);
        if (distance < 2.0f && angleView < 80.0f) return true;
        else return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetSocket && pickedUp)
        {
            Place();
        }
    }
    void RotateBlade(Vector3 hitPoint, Transform hitObject, Vector3 launchPos)
    {
        transform.parent = hitObject;
        var heading = hitPoint - launchPos;
        Vector3 newRot = Vector3.RotateTowards(transform.forward, heading, Mathf.Infinity, 0.0f);
        transform.position = hitPoint;
        transform.rotation = Quaternion.LookRotation(newRot);
    }
}
