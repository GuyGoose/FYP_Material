using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Contains the buttons action and add it to the playerActionQueue in CombatManager on click and remove it on click again
// The button will display the description

public class Button : MonoBehaviour {

    // This button
    public UnityEngine.UI.Button button;
    // Description text
    public TextMeshProUGUI descriptionText;

    // The action
    public Action action;
    public List<Image> costNotches = new List<Image>();

    bool clicked = false;

    // Start is called before the first frame update
    void Start() {
        this.button = GetComponent<UnityEngine.UI.Button>();
        this.descriptionText = GetComponentInChildren<TextMeshProUGUI>();

        // Add notch images to the costNotches list from the Energy_Container child = to the action's cost
        for (int i = 0; i < action.actionEnergyCost; i++) {
            costNotches.Add(transform.GetChild(1).GetChild(i).GetComponent<Image>());
        }

        // Set the notch images to black
        for (int i = 0; i < costNotches.Count; i++) {
            costNotches[i].color = Color.black;
        }
    }

    void Update() {
        // Set the description text
        descriptionText.text = action.actionDescription;
    }

    public void OnClick() {
        // If the button is clicked and the player can afford the action
        if (clicked && CombatManager.Instance.playerEnergy >= action.actionEnergyCost) {
            // Remove the action from the playerActionQueue in CombatManager
            CombatManager.Instance.playerActionQueue.Remove(action);
            // Set clicked to false
            clicked = false;
            // Change the color of the notch images to yellow
            for (int i = 0; i < costNotches.Count; i++) {
                costNotches[i].color = Color.yellow;
            }
        } else {
            // Add the action to the playerActionQueue in CombatManager
            CombatManager.Instance.playerActionQueue.Add(action);
            // Set clicked to true
            clicked = true;
            // Change the color of the notch images to white
            for (int i = 0; i < costNotches.Count; i++) {
                costNotches[i].color = Color.white;
            }
        }
    }
}