using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeScript : MonoBehaviour
{
    public Transform fadePanel;
    public bool active;
    Image image;
    Color curColor;
    [Range(0, 1)]
    public float targetAlpha = 0;
    [Range(0, 1)]
    public float startAlpha = 1;
    public float fadeTime = 2.0f;
    // https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html
    UnityEvent AfterFade;
    float timer, fadePerFrame;
    // Start is called before the first frame update
    void Start()
    {
        if (AfterFade == null) AfterFade = new UnityEvent();


        image = fadePanel.GetComponent<Image>();

        // Set the start alpha
        Color startColor = image.color;
        print(startColor.a);
        startColor.a = startAlpha;
        image.color = startColor;
        image.gameObject.SetActive(false);
        fadePerFrame = ((targetAlpha - startAlpha) / fadeTime);

    }
    private void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
            fadePanel.gameObject.SetActive(true);
            curColor = image.color;
            curColor.a += fadePerFrame * Time.deltaTime;

            image.color = curColor;
            //Debug.Log("Timer: " + timer + " Alpha: " + image.color.a);
            if (timer >= fadeTime)
            {
                AfterFade.Invoke();
                active = false;
            }
        }

    }
    public void AddEvent(UnityEngine.Events.UnityAction TheMethod)
    {
        AfterFade.AddListener(TheMethod);
    }
    public void StartFade()
    {
        active = true;
    }
}
