using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class MenuControlScript : MonoBehaviour
{

    private GameObject optionCanvas;
    private PlayerPrefsControlScript prefsControl;
    private MusicControl musicControl;

	// Use this for initialization
	void Start ()
	{
	    optionCanvas = GameObject.Find("OptionsCanvas");
	    prefsControl = FindObjectOfType<PlayerPrefsControlScript>();
	    musicControl = FindObjectOfType<MusicControl>();
        SetDefaultValues();
	}

    private void SetDefaultValues()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = -1;
        var sliders = optionCanvas.gameObject.GetComponentsInChildren<Slider>();
        foreach (var slider in sliders)
        {
            if (slider.name == "Sld_MusicVol")
                slider.value = prefsControl.GetMusicVolume();
            else if (slider.name == "Sld_SFXVol")
                slider.value = prefsControl.GetSfXVolume();
        }
        var toggles = optionCanvas.gameObject.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            if (toggle.name == "Tog_AutoFire")
                toggle.isOn = prefsControl.GetAutoFire();
            else if (toggle.name == "Tog_Accelerometer")
                toggle.isOn = prefsControl.GetAccelerometer();
        }
    }

    public void ShowOptionCanvas()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void HideOptionCanvas()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = -1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveOptionsToPlayerPrefs()
    {
        var sliders = optionCanvas.gameObject.GetComponentsInChildren<Slider>();
        foreach (var slider in sliders)
        {
            if (slider.name == "Sld_MusicVol")
                prefsControl.SetMusicVolume(slider.value);
            else if (slider.name == "Sld_SFXVol")
                prefsControl.SetSfxVolume(slider.value);
        }
        var toggles = optionCanvas.gameObject.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            print(toggle.name);
            if (toggle.name == "Tog_AutoFire")
                prefsControl.SetAutoFire(toggle.isOn);
            else if (toggle.name == "Tog_Accelerometer")
                prefsControl.SetAccelerometer(toggle.isOn);
        }
    }

    public void ChangeMusicVolume()
    {
        GameObject slider = GameObject.Find("Sld_MusicVol");
        musicControl.ChangeVolume(slider.GetComponent<Slider>().value);
    }
}
