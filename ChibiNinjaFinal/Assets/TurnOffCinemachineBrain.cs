using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCinemachineBrain : MonoBehaviour
{
    public float timeToTurnOff;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnOffCinemachineBrainMethod", timeToTurnOff);
    }
    private void TurnOffCinemachineBrainMethod()
    {
        GetComponent<CinemachineBrain>().enabled = false;        
        print("BrainOFF");
    }
}
