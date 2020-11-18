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
    public bool destroyOnPurchase = false;
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
    STAMINA,
    ATTACK
}


public class Economy : MonoBehaviour
{
    private GameObject player;
    public static Economy economy;
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
    private ShopKeeperNPC currentShopKeeper;


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
                SpendGold(_item.price);
                //print("Health incresed with " + _item.effect.effectValue.ToString() + "!");
                //print("You payed " + _item.price.ToString() + ".");
                if (serverMessagePanel != null)
                {
                    DestroyOldMessages();
                    InstantiateServerMessage("Health incresed with " + _item.effect.effectValue.ToString() + "!", true);
                }
            }
            // Add max health
            else if (_item.effect.target == ItemEffectTarget.MAX_HEALTH)
            {
                Health playerHealth = player.GetComponent<Health>();
                playerHealth.ResetHealth(playerHealth.health, playerHealth.maxHealth + _item.effect.effectValue);
                SpendGold(_item.price);
                if (serverMessagePanel != null)
                {
                    DestroyOldMessages();
                    InstantiateServerMessage("Max health incresed with " + _item.effect.effectValue.ToString() + "!", true);
                }
            }
            // Add stamina
            else if (_item.effect.target == ItemEffectTarget.STAMINA)
            {
                Stamina playerStamina = player.GetComponent<Stamina>();
                playerStamina.ResetStamina(playerStamina.stamina + _item.effect.effectValue, playerStamina.maxStamina);
                SpendGold(_item.price);
                if (serverMessagePanel != null)
                {
                    DestroyOldMessages();
                    InstantiateServerMessage("Stamina incresed with " + _item.effect.effectValue.ToString() + "!", true);
                }
            }
            // Add max stamina
            else if (_item.effect.target == ItemEffectTarget.MAX_STAMINA)
            {
                Stamina playerStamina = player.GetComponent<Stamina>();
                playerStamina.ResetStamina(playerStamina.stamina, playerStamina.maxStamina + _item.effect.effectValue);
                SpendGold(_item.price);
                if (serverMessagePanel != null)
                {
                    DestroyOldMessages();
                    InstantiateServerMessage("Max stamina incresed with " + _item.effect.effectValue.ToString() + "!", true);
                }
            }
            // Add attack value
            else if (_item.effect.target == ItemEffectTarget.ATTACK)
            {
                Attack playerAttack = player.transform.Find("SwordAttackHitBox").GetComponent<Attack>();
                playerAttack.attackValue += _item.effect.effectValue;
                SpendGold(_item.price);
                if (serverMessagePanel != null)
                {
                    DestroyOldMessages();
                    InstantiateServerMessage("Attack increased by " + _item.effect.effectValue.ToString() + "!", true);
                }
            }


            if (_item.destroyOnPurchase)
            {
                Item[] _items = currentShopKeeper.items;

                // Create a new array of items without the one we are going to destroy
                Item[] newArray = new Item[_items.Length - 1];
                int i = 0;
                foreach (Item it in _items)
                {
                    // If the item in the itteration is NOT the one we want to delete, add it to the new array
                    if(it != _item)
                    {
                        newArray[i] = it;
                        ++i;
                    }
                }
                currentShopKeeper.items = newArray;
                //Repopulate the store with the new array
                PopulateStore(currentShopKeeper);
            }
        }
        else
        {
            // You don't have enough gold.
            if (serverMessagePanel != null)
            {
                DestroyOldMessages();
                InstantiateServerMessage("You don't have enough gold to buy this item!", true);
            }
        }
    }
    public void DestroyOldMessages()
    {
        foreach (Transform t in serverMessagePanel)
        {
            
            Destroy(t.gameObject);
        }
    }
    public void InstantiateServerMessage(string message, bool destroy)
    {
        DestroyOldMessages();
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
    public void PopulateStore(ShopKeeperNPC shopKeeper)
    {
        if (storePanel != null)
        {
            currentShopKeeper = shopKeeper;
            Transform content = storePanel.transform.Find("Items Panel").Find("Content").transform;
            //Destroy old content
            foreach (Transform t in content)
            {
                Destroy(t.gameObject);
            }
            Item[] _items = shopKeeper.items;
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
        // Play add gold sound

        gold += amount;
        goldText.text = gold.ToString();
        GlowText();
    }
    public void SpendGold(int amount)
    {
        // Play spend gold sound

        gold -= amount;
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
