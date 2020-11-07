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
        if (transform.CompareTag("Player"))
        {
            healthNStaminaPanel = GameObject.FindGameObjectWithTag("HealthNStamina").transform;
            healthImage = healthNStaminaPanel.transform.Find("HealthFill").GetComponent<Image>();
            healthImageBG = healthNStaminaPanel.transform.Find("DamageFill").GetComponent<Image>();
        }
    }
    private void Update()
    {
        if(transform.CompareTag("Player"))
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
        if(transform.CompareTag("Player"))
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
            if(transform.CompareTag("Player"))
            {
                if(alive)
                {
                    CursorScript.instance.playerDead = true;
                    // death animation
                    Animator playerAnim = transform.GetComponent<Animator>();
                    playerAnim.SetTrigger("Dying");
                        AnimationClip[] clips = playerAnim.runtimeAnimatorController.animationClips;
                        foreach (AnimationClip clip in clips)
                        {
                            switch (clip.name)
                            {
                            // Find the name of the dying animation (Name of the clip itself, not the state)
                                case "Player Dying":
                                    StartCoroutine(PauseGameAfterDying(clip.length));
                                    break;
                            }
                        }
                    alive = false;

                    // Open dead-panel
                }
            }
            if (transform.CompareTag("ExplodingNPC"))
            {
                transform.GetComponent<ExplodingNPCController>().Die();
                alive = false;
            }
            if(transform.CompareTag("WaspNPC"))
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
