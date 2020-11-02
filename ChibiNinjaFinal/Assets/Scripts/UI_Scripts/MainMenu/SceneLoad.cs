using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public string sceneName;
    private FadeScript fadeScript;
    private void Start()
    {
        fadeScript = GetComponent<FadeScript>();
    }
    public void FadeBeforeLoad()
    {
        fadeScript.AddEvent(LoadScene);
        fadeScript.StartFade();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
