using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

// Gets the players ships and displays there actions on the action buttons
// Each ship has 4 actions. Max 4 ships
// Each action has a name, description, type, value, and target
// Takes in the number of ships and enables the correct number of action button menus
// Each action button menu has 4 buttons
// Each button has a description text
// The button will display the description
// When the button is clicked, the action will be added to the playerActionQueue in CombatManager

public class AMManager : MonoBehaviour {
    // The Player Instance
    public Player player = Player.Instance;

    // List of the four action menus
    public List<ActionMenu> actionMenus = new List<ActionMenu>();
    public int actionNumber = 0;

    // Start is called before the first frame update
    void Start() {
        foreach (ActionMenu actionMenu in actionMenus) {
            actionNumber = 0;
            // For each child of this object get the button
            foreach (Transform child in actionMenu.transform) {
                // Get the button
                UnityEngine.UI.Button button = child.GetComponent<UnityEngine.UI.Button>();
                // Get the button script
                ActionButton buttonScript = button.GetComponent<ActionButton>();
                // Add the button to the list
                actionMenu.actionButtons.Add(buttonScript);
                // Set the action
                buttonScript.action = player.playerShips[actionMenus.IndexOf(actionMenu)].shipActions[actionNumber];
                // Increment the action number
                actionNumber++;
            }
        }
    }
}