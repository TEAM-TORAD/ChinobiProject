using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int goldValue = 10;
    public GameObject pickupEffectPrefab;
    public float destroySecondsAferPickup = 2.0f;
    public ParticleSystem[] activeOnAwake;
    public ParticleSystem[] activeOnPickup;
    public iTween.EaseType coinSpinEaseType = iTween.EaseType.linear;
    private bool pickedUp = false;
    Hashtable hash;
    Transform mesh;

    //magnet coins
    Transform player;
    public float distance;
    Rigidbody rb;
    public float magnetDistance;
    public AudioSource koban;
    public AudioClip coinSound;
    public float magnetSpeed;



    // Start is called before the first frame update
    void Start()
    {
        //magnet coins
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;


        mesh = transform.Find("Mesh");
        Rotate();
        foreach (ParticleSystem p in activeOnAwake)
        {
            p.Play();
        }

        foreach (ParticleSystem p in activeOnPickup)
        {
            p.Stop();
            p.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //magnet coins
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < magnetDistance)
        {
            rb.AddForce((player.transform.position - transform.position) * magnetSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !pickedUp)
        {
            Economy.economy.AddGold(goldValue);
            pickedUp = true;
            StartCoroutine(PickupCoin(destroySecondsAferPickup, other.transform));
            koban.PlayOneShot(coinSound);
        }
    }

    IEnumerator PickupCoin(float destroyAfter, Transform pickupTransform)
    {
        

        foreach (ParticleSystem p in activeOnAwake)
        {
            p.Stop();
            p.Clear();
        }

        foreach (ParticleSystem p in activeOnPickup)
        {
            p.Play();
        }

        transform.Find("Mesh").gameObject.SetActive(false);
        GameObject pickupEffect = Instantiate(pickupEffectPrefab, pickupTransform.position, Quaternion.identity, pickupTransform);
        yield return new WaitForSeconds(destroyAfter);
        Destroy(transform.gameObject);
        Destroy(pickupEffect);
    }
    void Rotate()
    {
        // http://www.pixelplacement.com/itween/documentation.php#RotateAdd
        iTween.RotateAdd(transform, transform.gameObject, iTween.Hash(
            "space", Space.World,
            "time", 2.0f,
            "amount", new Vector3(0.0f, 360.0f, 0.0f),
            "easeType", coinSpinEaseType,
            "loopType", iTween.LoopType.none,
            "oncomplete", "RotationComplete",
            "oncompletetarget", transform.gameObject
            ));
    }
    public void RotationComplete()
    {
        Rotate();
    }
}
