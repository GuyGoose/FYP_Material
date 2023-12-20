using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Contains the buttons action and add it to the playerActionQueue in CombatManager on click and remove it on click again
// The button will display the description

public class ActionButton : MonoBehaviour {
    // The CombatManager Instance
    public CombatManager combatManager = CombatManager.Instance;
    // This button
    public UnityEngine.UI.Button button;
    // Description text
    public TextMeshProUGUI descriptionText;

    // The action
    public Action action;

    private int costToClick;
    public bool clicked = false;

    // Start is called before the first frame update
    void Start() {
        // Get the button
        button = GetComponent<UnityEngine.UI.Button>();

        // Set the combatManager to Object tagged "CombatManager"
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    void Update() {
        // Set the description text
        descriptionText.text = action.actionDescription;
        // Set the cost to click
        costToClick = action.actionEnergyCost;

        // If the player has enough energy to click the button, make it interactable
        if (combatManager.playerEnergy >= costToClick) {
            button.interactable = true;
        } else if (combatManager.playerEnergy < costToClick && clicked == false) {
            button.interactable = false;
        }

        // If Clicked is true, change the button's color
        if (clicked == true) {
            button.image.color = Color.green;
        } else if (clicked == false) {
            button.image.color = Color.white;
        }
    }

    // When the button is clicked, the action will be added to the playerActionQueue in CombatManager
    public void OnClick() {
        if (action != null)
            {
                if (clicked == false && combatManager.playerEnergy >= costToClick)
                {
                    // Remove the cost to click from the player's energy
                    combatManager.playerEnergy -= costToClick;

                    Debug.Log("Adding action");
                    combatManager.playerActionQueue.Add(action);
                    Debug.Log("Action added");
                    clicked = true;
                }
                else if (clicked == true)
                {
                    // Add the cost to click to the player's energy
                    combatManager.playerEnergy += costToClick;

                    Debug.Log("Removing action");
                    combatManager.playerActionQueue.Remove(action);
                    Debug.Log("Action removed");
                    clicked = false;
                }
            }
        else {
                Debug.LogError("Action is null. Make sure to assign a valid Action to the button.");
            }
    }
}