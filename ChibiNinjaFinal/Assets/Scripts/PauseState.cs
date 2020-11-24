using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseState : MonoBehaviour
{
    public GameObject pauseUI; // EGO containing the whole pause UI
    public GameObject pauseMenu; // EGO containing the pause UI
    public GameObject optionsMenu; // EGO containing the options UI
    public GameObject deadMenu;

    public bool pauseMenuActive;

    public AudioMixer audioMixerMusic;

    //private PlayerInputs playerInputs;

    public Slider BGMusicSlider;

    // Start is called before the first frame update
    void Start()
    {
        // hides UI at start of scene
        pauseUI.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        deadMenu.SetActive(false);
        pauseMenuActive = false;

       // playerInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CursorScript.instance.playerDead)
        {
            pauseUI.SetActive(true);
            deadMenu.SetActive(true);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
        //// what happens when escape button is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenuActive)
            {
                GamePaused();
            }
            else
            {
                Resume();
            }       
        }
    }
    public void Resume()
    {
        // hides pause UI
        pauseUI.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1.0f; // resumes the game speed
        pauseMenuActive = false;
        CursorScript.instance.menuOpen = false;
        AudioListener.pause = false;

    }
    public void GamePaused()
    {
        // shows pause UI
        pauseUI.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f; // pause game speed
        pauseMenuActive = true;
        CursorScript.instance.menuOpen = true;
        AudioListener.pause = true;
    }
    public void ReloadLevel()
    {
        string thisScene = SceneManager.GetActiveScene().name;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SceneManager.LoadScene(thisScene);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f; // resumes time  when leaving game scene to menu
        AudioListener.pause = false;
        SceneManager.LoadScene("NewMainMenu");
    }

    public void SetVolume()
    {
        audioMixerMusic.SetFloat("BG Music", Mathf.Log10(BGMusicSlider.value) * 20);
        print("volume set");
    }
}
