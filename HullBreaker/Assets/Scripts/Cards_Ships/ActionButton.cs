using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Gets the players ships and displays there actions on the action buttons
// Each ship has 4 actions. Max 4 ships
// Each action has a name, description, type, value, and target
// Takes in the number of ships and enables the correct number of action button menus
// Each action button menu has 4 buttons
// Each button has a description text
// The button will display the description
// When the button is clicked, the action will be added to the playerActionQueue in CombatManager

public class ActionButton : MonoBehaviour {
    // This menus ship
    public Ship ship;

    // The ship's actions
    public List<Action> shipActions = new List<Action>();

    // The action buttons
    public List<UnityEngine.UI.Button> actionButtons = new List<UnityEngine.UI.Button>();

    // Start is called before the first frame update
    void Start() {
        // Get the ship's actions
        shipActions = ship.shipActions;

        // For each child of this object get the button
        foreach (Transform child in transform) {
            actionButtons.Add(child.GetComponent<UnityEngine.UI.Button>());
        }

        // Set the action buttons to the ship's actions
        for (int i = 0; i < actionButtons.Count; i++) {
            actionButtons[i].GetComponent<Button>().action = shipActions[i];
        }
        
    }


}