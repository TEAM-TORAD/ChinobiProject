using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statue {
    public Transform statue;
    public float minAngle;
    public float maxAngle;
    public bool inPosition = false;
    [HideInInspector]
    public Quaternion startAngle;
    }
public class WheelTurner : MonoBehaviour
{

    public RisingWater RW;
    public Transform bowlWater;
    public Statue[] targets;
    private int target;

    public bool locked = true;
    public float wheelTurnPerSec = 30.0f, targetTurnSpeedPerSec = 15.0f, targetAngleGoalPerSec = 30.0f;
    private Transform player;
    private float angleChange = 0, newAngle;
    private float distance, angleView;
    private Transform angleChecker;
    private Transform beamTaget;
    bool hintPrinted = false;
    // Start is called before the first frame update
    void Start()
    {
        target = 0;
        foreach(Statue s in targets)
        {
            s.startAngle = s.statue.rotation;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        angleChecker = transform.parent.Find("AngleChecker");
        beamTaget = GameObject.FindGameObjectWithTag("BeamTarget").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.L)) LeverEfect();
        bool CA = CheckAngle();
        angleChange = 0;
        if(CA)
        {
            if(!hintPrinted)
            {
                hintPrinted = true;
                Economy.economy.InstantiateServerMessage("Use 'Q' or 'E' to rotate the wheel!", true);
            }
            if (!locked)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    angleChange = -targetAngleGoalPerSec * Time.deltaTime;

                }
                else if (Input.GetKey(KeyCode.E))
                {
                    angleChange = targetAngleGoalPerSec * Time.deltaTime;
                }
                //Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString();
            }
            else
            {
                // Wheel is locked
                if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) Economy.economy.InstantiateServerMessage("This wheel is locked!", true);
            }
        }
        newAngle += angleChange;
        float wheelChange = transform.localRotation.eulerAngles.z;
        wheelChange += angleChange * (wheelTurnPerSec / targetAngleGoalPerSec);

        //Rotate the turning wheel
        Quaternion newPosWheel = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0,0,wheelChange) , wheelTurnPerSec);
        transform.localRotation = newPosWheel;

        // Rotate the target
        Quaternion newTargetPos = Quaternion.Euler(0, newAngle, 0) * targets[target].startAngle;
        Quaternion newPosTarget = Quaternion.Lerp(targets[target].statue.rotation, newTargetPos, targetTurnSpeedPerSec);
        targets[target].statue.rotation = newPosTarget;
        if (targets[target].statue.rotation.eulerAngles.y <= targets[target].maxAngle && targets[target].statue.rotation.eulerAngles.y >= targets[target].minAngle) targets[target].inPosition = true;
        else targets[target].inPosition = false;



    }
    void RotateBeam( Transform beam, Transform hitObject)
    {
        transform.parent = hitObject;
        var heading = hitObject.position - beam.position;
        Vector3 newRot = Vector3.RotateTowards(beam.position, heading, Mathf.Infinity, 0.0f);
        beam.rotation = Quaternion.LookRotation(newRot);
    }
    public void LeverEfect()
    {
        locked = true;
        
        foreach(Statue s in targets)
        {
            Transform beam = s.statue.Find("DragonBeam");
            RotateBeam(beam, beamTaget);
            beam.GetComponent<ParticleSystem>().Play();
        }
        StartCoroutine(TurnOffBeams());
    }
    IEnumerator TurnOffBeams()
    {
        var smokeParticles = beamTaget.Find("Smoke").GetComponent<ParticleSystem>().main;

        yield return new WaitForSeconds(1);
        // Double the size of the smoke
        smokeParticles.startSize = Mathf.Lerp(smokeParticles.startSize.constant, smokeParticles.startSize.constant * 3, 3);
        Color endColor;
        ColorUtility.TryParseHtmlString("#4D391A", out endColor);
        // Lerp from the startcolor to a dark smoke
        smokeParticles.startColor = Color.Lerp(smokeParticles.startColor.color, endColor, 3);

        //Increase the water in the bowl
        Vector3 bowlWaterPos = bowlWater.position;
        bowlWaterPos.y += 0.4f;
        var conditions = iTween.Hash("position", bowlWaterPos, "time", 5.0f, "easetype", iTween.EaseType.easeInOutSine);
        iTween.MoveTo(bowlWater.gameObject, conditions);

        yield return new WaitForSeconds(5);
        // Destroy the 'evil' orb
        foreach(Transform t in beamTaget)
        {
            //Stop all particle systems in the beamTarget
            if (t.GetComponent<ParticleSystem>() != null) t.GetComponent<ParticleSystem>().Stop();
            // Deactivate the 'shepere' gameobject
            else if (t.name == "Sphere") t.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(7);
        // Turn of the beams
        foreach(Statue s in targets)
        {
            s.statue.Find("DragonBeam").GetComponent<ParticleSystem>().Stop();
        }
        // Start rising the water
        RW.WaterRise();
    }
    public bool CheckUnlocked()
    {
        bool allCompleted = true;
        foreach(Statue s in targets)
        {
            if (!s.inPosition)
            {
                allCompleted = false;
                break;
            }
        }
        return allCompleted;
    }
    public void NextStatue()
    {
        //Set the start angle to make sure the statue won't 'jump' into the rotation it had when the game started
        targets[target].startAngle = targets[target].statue.rotation;
        // Increment the index unless we reached the last item in the array
        if (target < targets.Length - 1) ++target;
        else target = 0;
        //Set the added angle to 0 to avoid the newlyselected statue to 'jump' into a new position
        newAngle = 0;
    }
    private bool CheckAngle()
    {
        distance = Vector3.Distance(angleChecker.position, player.position);
        angleChecker.position = new Vector3(angleChecker.position.x, player.position.y, angleChecker.position.z);
        Vector3 targetDir = angleChecker.position - player.position;
        angleView = Vector3.Angle(targetDir, player.forward);
        if (distance < 2.0f && angleView < 80.0f) return true;
        else return false;
    }

}
