using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupsScript : MonoBehaviour
{
    private Transform followThis;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        // All this needs to happen in the Start function. Other scripts are referencing this script in Awake. 
        // That way those scripts can assign variables referencing this gameobject before it becomes unparented

        // Create an empty game ojbect to follow
        var followObject = new GameObject("PopupFollowThis");
        followObject.transform.position = transform.position;
        followObject.transform.parent = transform.parent;
        followThis = followObject.transform;

        // Unparent the InteractionPopup gameobject
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        FollowCharacter();
    }
    public void ResetPosition()
    {
        transform.position = followThis.position;
    }
    void FollowCharacter()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, followThis.position, lerpSpeed * Time.deltaTime);
        transform.position = newPos;
        // Set the rotation to look at the camera
        transform.LookAt(Camera.main.transform);
        // Make sure the x rotation doesn't exceed 20' or it will look wierd.
        Vector3 newRot = transform.rotation.eulerAngles;
        // Make sure the x rotation matches the values in the inspector!
        if (newRot.x > 180) newRot.x -= 360;
        // Cap the rotaiton at 20'
        if (newRot.x > 20)
        {
            newRot.x = 20;
            transform.rotation = Quaternion.Euler(newRot);
        }
        else if (newRot.x < -20)
        {
            newRot.x = -20;
            transform.rotation = Quaternion.Euler(newRot);
        }
    }
}
