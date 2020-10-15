using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int goldValue = 10;
    Hashtable hash;
    // Start is called before the first frame update
    void Start()
    {
        Rotate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player")) {
            Economy.economy.AddGold(goldValue);
            Destroy(transform.parent.gameObject);
        }
    }
    void Rotate()
    {
        // http://www.pixelplacement.com/itween/documentation.php#RotateAdd
        iTween.RotateAdd(transform, transform.gameObject , iTween.Hash(
            "space", Space.World,
            "time", 2.0f,
            "amount", new Vector3(0.0f,360.0f,0.0f),
            "easeType", iTween.EaseType.spring,
            "loopType", iTween.LoopType.none,
            "oncomplete", "RotationComplete",
            "oncompletetarget", transform.gameObject
            ));

    }
    public void RotationComplete()
    {
        print("Rotation is completed.");
        Rotate();
    }
}
