using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class CharacterSelectController : MonoBehaviour
{
    public CharacterButton startingCharacter;
    public GameObject playerInfo;
    // TextObjects
    public TextMeshProUGUI pilotName;
    public TextMeshProUGUI pilotInfo;
    public TextMeshProUGUI shipName;
    public TextMeshProUGUI pilotHealth;
    public TextMeshProUGUI pilotEnergy;
    public TextMeshProUGUI pilotCredits;
    public Image shipImage;
    public Pilot currentPilot;
    // Start is called before the first frame update
    void Start()
    {
        DisplayPilotInfo(startingCharacter.pilot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPilotInfo(Pilot pilot) {
        // Set the pilot's name
        pilotName.text = pilot.pilotName;
        // Set the pilot's info
        pilotInfo.text = pilot.pilotInfo;
        // Set the pilot's health
        pilotHealth.text = "Health: " + pilot.startingHealth.ToString();
        // Set the pilot's energy
        pilotEnergy.text = "Energy: " + pilot.startingEnergy.ToString();
        // Set the pilot's credits
        pilotCredits.text = "Credits: " + pilot.startingCredits.ToString() + "c";
        // Set the pilot's ship name
        shipName.text = pilot.startingShip.shipName;
        // Set the pilot's ship image
        shipImage.sprite = pilot.startingShip.shipImage;

        SoundManager.Instance.PlaySFX("UI_In");

        currentPilot = pilot;
    }

    public void GoToGame() {
        // Delete the previos player and map info (Application.persistentDataPath + "/mapSave.json")
        // Check if the player has a save file
        if (SaveSystem.SaveFileExists()) {
            SaveSystem.DeletePlayer();
        }
        string path = Application.persistentDataPath + "/mapSave.json";
        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log("Deleted mapSave.json");
        } else {
            Debug.Log("No Previous mapSave.json found.");
        }



        // Save the selected pilot to playerinfo
        playerInfo.GetComponent<PlayerInfo>().SetPlayerPilot(currentPilot);
        SoundManager.Instance.PlaySFX("UI_In");
        // Load the game scene
        SceneManager.LoadScene("MapScene");
    }
    public void GoToMainMenu() {
        // Load the main menu scene
        SoundManager.Instance.PlaySFX("UI_Out");
        SceneManager.LoadScene("MainMenuScene");
    }
}
