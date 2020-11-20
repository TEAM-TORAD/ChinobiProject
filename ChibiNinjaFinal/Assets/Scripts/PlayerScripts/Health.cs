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
    private Transform healthNStaminaPanel;
    private Image healthImage, healthImageBG;
    private Color lerpedColor;
    public Color greyColor, redColor;
    private bool alive = true;

 

    void Start()
    {
        //ragdoll states
        if(transform.CompareTag("Player"))
        {
            setRigidbodyState(true);
            setColliderState(false);
        }

        if (transform.CompareTag("Player"))
        {
            healthNStaminaPanel = GameObject.FindGameObjectWithTag("HealthNStamina").transform;
            healthImage = healthNStaminaPanel.transform.Find("HealthFill").GetComponent<Image>();
            healthImageBG = healthNStaminaPanel.transform.Find("DamageFill").GetComponent<Image>();
        }
    }
    private void Update()
    {
        if (transform.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.O)) TakeDamage(10);
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
        if (transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }

    }
    public void ResetHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        if (health > maxHealth) health = maxHealth;
        else if (health < 0) health = 0;

        if (transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }

    IEnumerator PauseGameAfterDying(float time)
    {
        CursorScript.instance.playerDead = true;
        yield return new WaitForSeconds(time);
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
    }

    public void TakeDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            health = 0;
            // Death effect
            if (transform.CompareTag("Player"))
            {
                if (alive)
                {
                    CursorScript.instance.playerDead = true;
                    // death animation
                    Animator playerAnim = transform.GetComponent<Animator>();
                    playerAnim.SetTrigger("Dying");

                    //ragdoll trigger
                    GetComponent<Animator>().enabled = false;
                    setRigidbodyState(false);
                    setColliderState(true);
                    StartCoroutine(PauseGameAfterDying(5));
                    
                    alive = false;

                    // Open dead-panel
                }
            }
            if (transform.CompareTag("ExplodingNPC"))
            {
                transform.GetComponent<ExplodingNPCController>().Die();
                alive = false;
            }
            if (transform.CompareTag("WaspNPC"))
            {
                transform.GetComponent<WaspNPCScript>().Die();
                alive = false;
            }
        }
        else
        {
            // Take damage
            //Debug.Log(transform.name + " took " + value + " in damage.");

            if (transform.CompareTag("ExplodingNPC"))
            {

            }
            else if (transform.CompareTag("WaspNPC"))
            {

            }
        }
        if (transform.CompareTag("Player"))
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }

    #region Ragdoll

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(transform.CompareTag("Player"))
        {
            if (collision.relativeVelocity.y > 15)
            {
                //ragdoll trigger
                GetComponent<Animator>().enabled = false;
                setRigidbodyState(false);
                setColliderState(true);
                TakeDamage(maxHealth);
               
            }
        }

        
    }
}
