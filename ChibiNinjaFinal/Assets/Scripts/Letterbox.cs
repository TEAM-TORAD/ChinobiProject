using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    private RectTransform topBar;
    private RectTransform bottomBar;

    private float changeSizeAmount;
    private float targetSize;

    private bool isActive;


    private void Awake()
    {
        //top bar
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        topBar = gameObject.GetComponent<RectTransform>();

        topBar.anchorMin = new Vector2(0, 1); //top left
        topBar.anchorMax = new Vector2(1, 1); //top right
        topBar.sizeDelta = new Vector2(0, 0); //creates the image from the top to 300 (~middle)

        //bottom bar
        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        bottomBar = gameObject.GetComponent<RectTransform>();

        bottomBar.anchorMin = new Vector2(0, 0); //bottom left
        bottomBar.anchorMax = new Vector2(1, 0); //bottom right
        bottomBar.sizeDelta = new Vector2(0, 0); //creates the image from the bottom to 300 (~middle)
    }

    public void ShowBar(float targetSize, float time)
    {
        this.targetSize = targetSize;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }

    public void HideBar(float time)
    {
        targetSize = 0f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Vector2 sizeDelta = topBar.sizeDelta;
            sizeDelta.y += changeSizeAmount * Time.deltaTime;

            //stop the animation
            if (changeSizeAmount > 0)
            {
                if (sizeDelta.y >= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            else
            {
                if (sizeDelta.y <= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            topBar.sizeDelta = sizeDelta;
            bottomBar.sizeDelta = sizeDelta;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowBar(105, 1.3f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            HideBar(1.3f);
        }
    }

}
