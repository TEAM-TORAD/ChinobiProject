using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ServerMessageScript : MonoBehaviour
{
    [HideInInspector]
    public string message;
    public float dissolveAfter = 2.0f;
    public float destroyAfter = 3.0f;
    private float timer;
    private Image image;
    private TextMeshProUGUI TMP;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        TMP = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (destroyAfter <= 0) print("The server message won't be destroyed since destroyAfter is set to " + destroyAfter + ".");

        if (destroyAfter > 0 &&  dissolveAfter > destroyAfter) dissolveAfter = destroyAfter;

    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= dissolveAfter)
        {
            Color newColorBG = image.color;
            newColorBG.a -= (1 / (destroyAfter - dissolveAfter)) * Time.deltaTime;
            image.color = newColorBG;

            Color newColorText = TMP.color;
            newColorText.a -= (1 / (destroyAfter - dissolveAfter)) * Time.deltaTime;
            TMP.color = newColorText;
        }
        if(destroyAfter > 0 &&  timer >= destroyAfter)
        {
            Destroy(transform.gameObject);
        }
    }
    public void SetServerText(string _text)
    {
        transform.Find("Text").GetComponent<TextMeshProUGUI>().text = _text;
    }
}
