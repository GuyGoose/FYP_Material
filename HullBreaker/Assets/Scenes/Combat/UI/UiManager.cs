using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameObject[] shipButtons;

    public ActionManager actionManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get ship buttons (tagged as "ShipButton")
        shipButtons = GameObject.FindGameObjectsWithTag("ShipTab");
        actionManager = FindObjectOfType<ActionManager>();
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
        actionManager.UpdateCurrentShip(ships[0]);
        ChangeSelectedShips(shipButtons[0]);
    }

    public void ChangeSelectedShips(GameObject shipTab) {
        // Set all ship buttons to not selected
        foreach (GameObject button in shipButtons) {
            button.GetComponent<ShipTab>().animator.SetBool("Pressed", false);
        }
        // Set the selected ship button to selected
        shipTab.GetComponent<ShipTab>().animator.SetBool("Pressed", true);
        // Set the current ship to the selected ship
        actionManager.UpdateCurrentShip(shipTab.GetComponent<ShipTab>().ship);
    }
}
