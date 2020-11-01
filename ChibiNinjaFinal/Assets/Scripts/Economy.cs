using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public int price;
    public Sprite image;
    public ItemEffects effect;
}

[System.Serializable]
public class ItemEffects
{
    public int effectValue;
    public ItemEffectTarget target;
}
public enum ItemEffectTarget
{
    MAX_HEALTH,
    MAX_STAMINA,
    HEALTH,
    STAMINA
}


public class Economy : MonoBehaviour
{

    public KeyCode storeButton = KeyCode.P;
    private GameObject player;
    public static Economy economy;
    public GameObject inventoryGoldPrefab;
    public int gold;
    Transform goldPanel;
    [HideInInspector]
    public Transform storePanel;
    Transform serverMessagePanel;
    TextMeshProUGUI goldText;
    public float glowTimePeak = 1.5f;
    private bool updateGlow = false;
    private float currentStrength, updateValue;


    public GameObject messageRegular;
    public GameObject messageHint;

    public GameObject itemPrefab;
    public Item[] items;


    private void Awake()
    {
        if (economy == null) economy = this;
        else Destroy(this);
        goldPanel = GameObject.FindGameObjectWithTag("GoldPanel").transform;

        goldText = goldPanel.transform.Find("Amount Text").GetComponent<TextMeshProUGUI>();
        goldText.text = gold.ToString();
        SetGlowStrength(currentStrength);

        player = GameObject.FindGameObjectWithTag("Player");


        serverMessagePanel = GameObject.FindGameObjectWithTag("MessagePanel").transform;

        storePanel = GameObject.FindGameObjectWithTag("StorePanel").transform;
        //PopulateStore();
        storePanel.gameObject.SetActive(false);


    }
    private void Update()
    {
        if (updateGlow)
        {
            currentStrength += Time.deltaTime * updateValue;
            SetGlowStrength(currentStrength);
        }
        if (Input.GetKeyDown(storeButton))
        {
            if (storePanel.gameObject.activeSelf)
            {
                storePanel.gameObject.SetActive(false);
                CursorScript.instance.storeOpen = false;
            }
            else
            {
                storePanel.gameObject.SetActive(true);
                CursorScript.instance.storeOpen = true;
            }
        }
    }
    public void PurchaseItem(Item _item)
    {
        if (gold >= _item.price)
        {
            // You can afford the item

            // Add health
            if (_item.effect.target == ItemEffectTarget.HEALTH)
            {
                Health playerHealth = player.GetComponent<Health>();
                playerHealth.AddHealth(_item.effect.effectValue);
                AddGold(-_item.price);
                //print("Health incresed with " + _item.effect.effectValue.ToString() + "!");
                //print("You payed " + _item.price.ToString() + ".");
                if (serverMessagePanel != null)
                {
                    GameObject newText = Instantiate(messageRegular, serverMessagePanel);
                    newText.GetComponent<ServerMessageScript>().SetServerText("Health incresed with " + _item.effect.effectValue.ToString() + "!");
                }
            }
            // Add max health
            else if (_item.effect.target == ItemEffectTarget.MAX_HEALTH)
            {
                Health playerHealth = player.GetComponent<Health>();
                playerHealth.ResetHealth(playerHealth.health, playerHealth.maxHealth + _item.effect.effectValue);
                AddGold(-_item.price);
                //print("Max health incresed with " + _item.effect.effectValue.ToString() + "!");
                //print("You payed " + _item.price.ToString() + "."); 
                if (serverMessagePanel != null)
                {
                    GameObject newText = Instantiate(messageRegular, serverMessagePanel);
                    newText.GetComponent<ServerMessageScript>().SetServerText("Max health incresed with " + _item.effect.effectValue.ToString() + "!");
                }
            }
            // Add stamina
            else if (_item.effect.target == ItemEffectTarget.STAMINA)
            {
                Stamina playerStamina = player.GetComponent<Stamina>();
                playerStamina.ResetStamina(playerStamina.stamina + _item.effect.effectValue, playerStamina.maxStamina);
                AddGold(-_item.price);
                //print("Stamina incresed with " + _item.effect.effectValue.ToString() + "!");
                //print("You payed " + _item.price.ToString() + ".");
                if (serverMessagePanel != null)
                {
                    GameObject newText = Instantiate(messageRegular, serverMessagePanel);
                    newText.GetComponent<ServerMessageScript>().SetServerText("Stamina incresed with " + _item.effect.effectValue.ToString() + "!");
                }
            }
            // Add max stamina
            else if (_item.effect.target == ItemEffectTarget.MAX_STAMINA)
            {
                Stamina playerStamina = player.GetComponent<Stamina>();
                playerStamina.ResetStamina(playerStamina.stamina, playerStamina.maxStamina + _item.effect.effectValue);
                AddGold(-_item.price);
                //print("Max stamina incresed with " + _item.effect.effectValue.ToString() + "!");
                //print("You payed " + _item.price.ToString() + ".");
                if (serverMessagePanel != null)
                {
                    GameObject newText = Instantiate(messageRegular, serverMessagePanel);
                    newText.GetComponent<ServerMessageScript>().SetServerText("Max stamina incresed with " + _item.effect.effectValue.ToString() + "!");
                }
            }
        }
        else
        {
            // You don't have enough gold.
            if (serverMessagePanel != null)
            {
                GameObject newText = Instantiate(messageRegular, serverMessagePanel);
                newText.GetComponent<ServerMessageScript>().SetServerText("You don't have enough gold to buy this item!");
            }
            print("Not enough gold!");
        }
    }
    public void DestroyOldMessages()
    {
        foreach (Transform t in serverMessagePanel)
        {
            if (!t.GetComponent<ServerMessageScript>().autoDestroy)
            {
                Destroy(t.gameObject);
            }
        }
    }
    public void InstantiateServerMessage(string message, bool destroy)
    {

        if (destroy)
        {
            GameObject newText = Instantiate(messageRegular, serverMessagePanel);
            ServerMessageScript SMS = newText.GetComponent<ServerMessageScript>();
            SMS.SetServerText(message);
            SMS.autoDestroy = true;
        }
        else
        {
            GameObject newText = Instantiate(messageRegular, serverMessagePanel);
            ServerMessageScript SMS = newText.GetComponent<ServerMessageScript>();
            SMS.SetServerText(message);
            SMS.autoDestroy = false;
        }
    }
    public void PopulateStore(Item[] _items)
    {
        if (storePanel != null)
        {
            Transform content = storePanel.transform.Find("Items Panel").Find("Content").transform;
            //Destroy old content
            foreach (Transform t in content)
            {
                Destroy(t.gameObject);
            }
            // Add new content
            foreach (Item i in _items)
            {
                GameObject newItem = Instantiate(itemPrefab, content);
                newItem.transform.Find("Image").GetComponent<Image>().sprite = i.image;
                newItem.transform.Find("Hover Panel").Find("Title").GetComponent<TextMeshProUGUI>().text = i.name;
                newItem.transform.Find("Hover Panel").Find("Info").GetComponent<TextMeshProUGUI>().text = i.description;
                newItem.transform.Find("Hover Panel").Find("Price").GetComponent<TextMeshProUGUI>().text = i.price.ToString();
                //newItem.GetComponent<ItemScript>().item = i;
                newItem.GetComponent<Button>().onClick.AddListener(() => PurchaseItem(i));
            }
        }
        else print("storePanel is null!");
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

        yield return new WaitForSeconds(glowTimePeak);
        updateValue = -1 / glowTimePeak;
        yield return new WaitForSeconds(glowTimePeak);
        updateGlow = false;
        currentStrength = 0;
        SetGlowStrength(currentStrength);
    }
}
