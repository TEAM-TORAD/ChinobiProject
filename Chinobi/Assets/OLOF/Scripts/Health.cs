using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Health fix
public class Health : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public bool healthCriticalRunning = false, online = false;
    //public int lives;
    public GameObject healthImagePrefab;
    private Transform healthPanel;
    private Image healthImage, healthImageBG;
    private Color lerpedColor;
    public Color greyColor, redColor;

    void Start()
    {
        if (transform.CompareTag("Player"))
        {
            healthPanel = GameObject.FindGameObjectWithTag("HealthPanel").transform;
            GameObject healthImageObject = Instantiate(healthImagePrefab, healthPanel);
            healthImage = healthImageObject.transform.GetChild(1).GetComponent<Image>();
            healthImageBG = healthImageObject.transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Take Damage (testing purposes)
            TakeDamage(10);
        }
        if(transform.CompareTag("Player"))
        {
            if ((float)health / maxHealth < 0.2f)
            {
                lerpedColor = Color.Lerp(greyColor, redColor, Mathf.PingPong(Time.time, 1.25f));
                healthImageBG.color = lerpedColor;
            }
            else
            {
                healthImageBG.color = greyColor;
            }
        }
    }
    public void AddHealth(int value)
    {
        health += value;
        if (health > maxHealth) health = maxHealth;
        if(transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
        
    }
    public void ResetHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        if (transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }

    public void TakeDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            health = 0;
            // Death effect
            if(transform.CompareTag("Player"))
            {

            }
            if (transform.CompareTag("ExplodingNPC"))
            {
                transform.GetComponent<ExplodingNPCController>().Die();
            }
            if(transform.CompareTag("WaspNPC"))
            {
                //transform.GetComponentInChildren<Animator>().SetTrigger("Die");
                transform.GetComponent<WaspNPCScript>().Die();
            }
        }
        else
        {
            // Take damage
            Debug.Log(transform.name + " took " + value + " in damage.");
            
            if (transform.CompareTag("ExplodingNPC"))
            {

            }
            else if(transform.CompareTag("WaspNPC"))
            {

            }
        }
        if (transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }
}
