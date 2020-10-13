using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stamina : MonoBehaviour
{

    public float stamina = 100;
    public float maxStamina = 100;
    public float drainPerSecond = 2.0f, gainPerSecond = 1.0f;
    public bool online = false, usingStamina = false;
    //public int lives;
    public GameObject staminaImagePrefab;
    private Transform staminaPanel;
    private Image staminaImage;
    //private Color lerpedColor; // Not currently used
    // Start is called before the first frame update
    void Start()
    {
        
            if(transform.CompareTag("Player"))
            {
                staminaPanel = GameObject.FindGameObjectWithTag("StaminaPanel").transform;
                GameObject staminaObject = Instantiate(staminaImagePrefab, staminaPanel);
                staminaImage = staminaObject.transform.GetChild(1).GetComponent<Image>();
            }
        
    }

    // Update is called once per frame
    void Update()
    {
       
            if(usingStamina)
            {
                DrainStamina();
            }
            else
            {
                RegenerateStamina();
            }
            if(transform.CompareTag("Player"))
            {
                staminaImage.fillAmount = (float)stamina / maxStamina;
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
        if (transform.CompareTag("Player"))
        {
            staminaImage.fillAmount = (float)stamina / maxStamina;
        }
    }
}
