using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu;
    public Slider musicSlider, sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (optionsMenu.activeSelf) {
            CorrectMusicSlider();
            CorrectSFXSlider();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCharacterSelect() {
        SoundManager.Instance.PlaySFX("UI_In");
        // Load the character select scene
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void GoToMap() {
        SoundManager.Instance.PlaySFX("Land");
        // Load the map scene
        SceneManager.LoadScene("MapScene");
    }

    // --- Volume Control ---
    
    // Set slider to match the volume stored in PlayerPrefs
    public void CorrectMusicSlider() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    // Set slider to match the volume stored in PlayerPrefs
    public void CorrectSFXSlider() {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    public void ControlMusicVolume()
    {
        SoundManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void ControlSFXVolume()
    {
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void ToggleMenus() {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);

        // If the options menu is active, correct the sliders
        if (optionsMenu.activeSelf) {
            CorrectMusicSlider();
            CorrectSFXSlider();
            SoundManager.Instance.PlaySFX("UI_In");
        }
        else {
            SoundManager.Instance.PlaySFX("UI_Out");
        }
    }

    public void QuitGame() {
        SoundManager.Instance.PlaySFX("UI_Out");
        Application.Quit();
    }
}
