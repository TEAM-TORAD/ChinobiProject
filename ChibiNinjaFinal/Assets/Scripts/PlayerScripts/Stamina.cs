using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

public class Stamina : MonoBehaviour
{

    public float stamina = 100;
    public float maxStamina = 100;
    public float drainPerSecond = 2.0f, gainPerSecond = 1.0f;
    public bool online = false, usingStamina = false;
    //public int lives;
    private Transform staminaPanel;
    private Image staminaImage;
    private NavMeshObstacle navOb;
    private Animator animator;

    //private Color lerpedColor; // Not currently used
    // Start is called before the first frame update

    public float minForceFieldScale = 2f;
    public float maxForceFieldScale = 3f;

    public Transform forceFieldTransform;
    public Transform electricity, bubble;  

    
    private float deltaForceFieldScale => maxForceFieldScale - minForceFieldScale;

    void Start()
    {
        if(transform.CompareTag("Player"))
        {
            staminaPanel = GameObject.FindGameObjectWithTag("HealthNStamina").transform;
            staminaImage = staminaPanel.transform.Find("ShieldFill").GetComponent<Image>();
            navOb = GetComponent<NavMeshObstacle>();
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if(usingStamina)
        {
            DrainStamina();
            forceFieldTransform.gameObject.SetActive(true);
            electricity.gameObject.SetActive(true);
            //navOb.enabled = true;
        }
        else
        {
            RegenerateStamina();
            forceFieldTransform.gameObject.SetActive(false);
            electricity.gameObject.SetActive(false);
            //navOb.enabled = false;
        }
        if(transform.CompareTag("Player"))
        {
            staminaImage.fillAmount = (float)stamina / maxStamina;
            UpdateForceFieldSize();
        }
    }

    private void UpdateForceFieldSize()
    {
        float actualStaminaPercent = stamina / maxStamina;
        float forceFieldSize = minForceFieldScale + deltaForceFieldScale * actualStaminaPercent;

        forceFieldTransform.localScale = Vector3.one * forceFieldSize;
        if(electricity != null)
        {
            if(electricity.gameObject.activeSelf)
            {
                var electricityPS = electricity.GetComponent<ParticleSystem>().main;
                electricityPS.startSize = minForceFieldScale / 2.5f + deltaForceFieldScale * actualStaminaPercent * 0.15f;
                //electricity.GetComponent<ParticleSystem>().Clear();
            }

        }
        if(bubble != null)
        {
            if(electricity.gameObject.activeSelf)
            {
                var bubblePS = bubble.GetComponent<ParticleSystem>().main;
                bubblePS.startSize = minForceFieldScale / 2.5f + deltaForceFieldScale * actualStaminaPercent * 0.3f;
            }
        }
    }

    private void RegenerateStamina()
    {
        stamina += gainPerSecond * Time.deltaTime;
        if (stamina > maxStamina) stamina = maxStamina;
    }
    public void StaminaDamage(float value)
    {
        stamina -= value;
    }

    private void DrainStamina()
    {
        stamina -= drainPerSecond * Time.deltaTime;
        if(stamina < 0)
        {
            // Out of stamina
            stamina = 0;
        }
    }
    public void ResetStamina(float _stamina, float _maxStamina)
    {
        stamina = _stamina;
        maxStamina = _maxStamina;
        if (stamina > maxStamina) stamina = maxStamina;
        else if (stamina < 0) stamina = 0;
        if (transform.CompareTag("Player"))
        {
            staminaImage.fillAmount = (float)stamina / maxStamina;
        }
    }
}
