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
        shipButtons = GameObject.FindGameObjectsWithTag("ShipButton");


        // DEBUG: set anim bool "IsAvailable" to true for all ship buttons
        foreach (GameObject shipButton in shipButtons) {
            shipButton.GetComponent<Animator>().SetBool("IsAvailable", true);
        }

        // Set the first ship button to "Selected" = true and the others to "Selected" = false
        shipButtons[0].GetComponent<Animator>().SetBool("Selected", true);
        for (int i = 1; i < shipButtons.Length; i++) {
            shipButtons[i].GetComponent<Animator>().SetBool("Selected", false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShipButtonSelected(GameObject button) {
        // Set the animation for the selected button to "Selected" = true and the others to "Selected" = false
        foreach (GameObject shipButton in shipButtons) {
            if (shipButton == button) {
                shipButton.GetComponent<Animator>().SetBool("Selected", true);
            } else {
                shipButton.GetComponent<Animator>().SetBool("Selected", false);
            }
        }
        
    }
}
