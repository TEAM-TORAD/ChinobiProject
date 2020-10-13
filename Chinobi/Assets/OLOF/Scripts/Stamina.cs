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

    public GameObject forceField;
    public float reduceSizeSpeed = 1.0f;
    public float increaseSizeSpeed = 1.0f;

    //private Color lerpedColor; // Not currently used
    // Start is called before the first frame update
    void Start()
    {

        if (transform.CompareTag("Player"))
        {
            staminaPanel = GameObject.FindGameObjectWithTag("StaminaPanel").transform;
            GameObject staminaObject = Instantiate(staminaImagePrefab, staminaPanel);
            staminaImage = staminaObject.transform.GetChild(1).GetComponent<Image>();
        }

        forceField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (usingStamina)
        {
            forceField.SetActive(true);
            DrainStamina();
        }
        else
        {
            RegenerateStamina();
            forceField.SetActive(false);
        }
        if (transform.CompareTag("Player"))
        {
            staminaImage.fillAmount = (float)stamina / maxStamina;
        }



    }

    private void RegenerateStamina()
    {
        stamina += gainPerSecond * Time.deltaTime;
        forceField.transform.localScale = Vector3.Lerp(forceField.transform.localScale, new Vector3(2, 2, 2), Time.deltaTime * increaseSizeSpeed);
        if (stamina > maxStamina) stamina = maxStamina;
    }
    public void StaminaDamage(float value)
    {
        stamina -= value;
    }

    private void DrainStamina()
    {
        stamina -= drainPerSecond * Time.deltaTime;
        forceField.transform.localScale = Vector3.Lerp(forceField.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * reduceSizeSpeed);

        if (stamina < 0)
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
