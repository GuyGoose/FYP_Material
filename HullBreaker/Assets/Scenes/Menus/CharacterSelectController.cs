using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    // TextObjects
    public TextMeshProUGUI pilotName;
    public TextMeshProUGUI pilotInfo;
    public TextMeshProUGUI shipName;
    public TextMeshProUGUI pilotHealth;
    public TextMeshProUGUI pilotEnergy;
    public TextMeshProUGUI pilotCredits;
    public Image shipImage;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
