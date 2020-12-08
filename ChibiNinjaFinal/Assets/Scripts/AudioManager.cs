using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterSlider, musicSlider, sfxSlider;

    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MasterPref = "Master";
    private static readonly string MusicPref = "BGMusic";
    private static readonly string SFXPref = "SoundEffects";

    private int firstPlayInt;
    private float masterFloat, musicFloat, sfxFloat;

    public void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            masterFloat = 1f;
            musicFloat = 1f;
            sfxFloat = 1f;
            if (masterSlider != null) masterSlider.value = masterFloat;
            if (musicSlider != null) musicSlider.value = musicFloat;
            if (sfxSlider != null) sfxSlider.value = sfxFloat;

            PlayerPrefs.SetFloat(MasterPref, masterFloat);
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SFXPref, sfxFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);

        }

        else
        {
            masterFloat = PlayerPrefs.GetFloat(MasterPref);
            if (masterSlider != null) masterSlider.value = masterFloat;
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            if (musicSlider != null) musicSlider.value = musicFloat;
            sfxFloat = PlayerPrefs.GetFloat(SFXPref);
            if (sfxSlider != null) sfxSlider.value = sfxFloat;
        }

    }

    public void Update()
    {
        if (masterSlider != null) masterFloat = masterSlider.value;
        if (musicSlider != null) musicFloat = musicSlider.value;
        if (sfxSlider != null) sfxFloat = sfxSlider.value;

        //print("Master Volume = " + masterFloat);
        //print("Music Volume = " + musicFloat);
        //print("SFX Volume = " + sfxFloat);
    }

    public void SaveSoundSetting()
    {
        if (masterSlider != null) PlayerPrefs.SetFloat(MasterPref, masterSlider.value);
        if (musicSlider != null) PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        if (sfxSlider != null) PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
    }

    public void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSetting();
        }
    }

    public void UpdateSound()
    {
        if (masterSlider != null) audioMixer.SetFloat("Master", Mathf.Log10(masterFloat) * 20);
        if (musicSlider != null) audioMixer.SetFloat("BGMusic", Mathf.Log10(musicFloat) * 20);
        if (sfxSlider != null) audioMixer.SetFloat("SoundEffects", Mathf.Log10(sfxFloat) * 20);
    }
}
