using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RandomReward : MonoBehaviour
{

    public static RandomReward RR;

    [Header("Drop Items")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    public List<GameObject> itemDrops;

    void Start()
    {
        if (RR == null) RR = this;
        else Destroy(this);

        itemDrops = new List<GameObject>();
        itemDrops.Add(item1);
        itemDrops.Add(item2);
        itemDrops.Add(item3);
        itemDrops.Add(item4);
    }



}
