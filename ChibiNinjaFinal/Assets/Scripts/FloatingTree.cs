using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTree : MonoBehaviour
{
    public float yRotSpeed = 30.0f;
    public float moveHeight = 1.0f, timeToComplete = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 moveToVector = transform.position;
        moveToVector.y += moveHeight;
        Move(moveToVector);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, yRotSpeed * Time.deltaTime, 0));
    }
    void Move(Vector3 targetPos)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetPos, "time", timeToComplete, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutSine));
        //iTween.RotateBy(gameObject, iTween.Hash("y", 1, "time", 5, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
    }
}
