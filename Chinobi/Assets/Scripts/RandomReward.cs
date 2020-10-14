using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.AI;

public class RandomReward : MonoBehaviour
{
    
    public Transform deadEnemy;

    [Header("Drop Items")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    List<GameObject> itemDrops;

    void Start()
    {
        itemDrops = new List<GameObject>();
        itemDrops.Add(item1);
        itemDrops.Add(item2);
        itemDrops.Add(item3);
        itemDrops.Add(item4);
    
    }

    public void DropItem()
    {
        StartCoroutine(DropDelay());
        
    }

    IEnumerator DropDelay()
    {
        yield return new WaitForSeconds(3);

        int num = Random.Range(0, itemDrops.Count);
        if(deadEnemy != null) Instantiate(itemDrops[num], deadEnemy.position, deadEnemy.rotation);
        Destroy(deadEnemy.parent.gameObject);
    }


}
