using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameObject[] shipButtons;

    // Start is called before the first frame update
    void Start()
    {
        // Get ship buttons (tagged as "ShipButton")
        shipButtons = GameObject.FindGameObjectsWithTag("ShipTab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupShipButtons(List<Ship> ships) {
        // Set the ship buttons to the ships in the player's inventory
        for (int i = 0; i < ships.Count; i++) {
            shipButtons[i].GetComponent<ShipTab>().AssignShip(ships[i]);
        }
        // Set the first ship in the list as the current ship
        shipButtons[0].GetComponent<ShipTab>().OnSelect();
    }

    public void ChangeSelectedShips(GameObject shipTab) {
        // Set the animation for the selected button to "Pressed" = true and the others to "Pressed" = false
        foreach (GameObject button in shipButtons) {
            if (button == shipTab) {
                button.GetComponent<Animator>().SetBool("Pressed", true);
            } else {
                button.GetComponent<Animator>().SetBool("Pressed", false);
            }
        }
        
    }
}
