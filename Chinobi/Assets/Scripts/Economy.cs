using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Economy : MonoBehaviour
{
    public static Economy economy;
    public GameObject inventoryGoldPrefab;
    public int gold;
    Transform goldPanel;
    TextMeshProUGUI goldText;
    public float glowTimePeak = 1.5f;
    private bool updateGlow = false;
    private float currentStrength, updateValue;
    

    private void Awake()
    {
        if (economy == null) economy = this;
        else Destroy(this);
        goldPanel = GameObject.FindGameObjectWithTag("GoldPanel").transform;
        GameObject thisObject = Instantiate(inventoryGoldPrefab, goldPanel);
        goldText = thisObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        goldText.text = gold.ToString();
        SetGlowStrength(currentStrength);

    }
    private void Update()
    {
        if(updateGlow)
        {
            currentStrength += Time.deltaTime * updateValue;
            SetGlowStrength(currentStrength);
        }
    }
    private void SetGlowStrength(float value)
    {
        if (value < 0) value = 0;
        else if (value > 1) value = 1.0f;
        goldText.font.material.SetFloat(ShaderUtilities.ID_GlowPower, value);
    }
    public void AddGold(int amount)
    {
        gold += amount;
        goldText.text = gold.ToString();
        GlowText();
    }
    public void GlowText()
    {
        updateGlow = true;
        updateValue = 1 / glowTimePeak;
        StartCoroutine(GlowTextCR());
    }
    IEnumerator GlowTextCR()
    {
        
        yield return new  WaitForSeconds(glowTimePeak);
        updateValue = -1 / glowTimePeak;
        yield return new WaitForSeconds(glowTimePeak);
        updateGlow = false;
        currentStrength = 0;
        SetGlowStrength(currentStrength);
    }
}
