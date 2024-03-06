using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Contains the information regarding the action button 
    /*
    -- Action Button --
    - Action Name
    - Action Description
    - Action Energy Cost
    - Action Image
    */

    // The action name
    public string actionName;
    // The action description
    public string actionDescription;
    // The action energy cost
    public int actionEnergyCost;

    private Action currentAction;

    // -- UI Elements --
    public Image actionImage;
    public TextMeshProUGUI actionEnergyCostText;
    private Animator animator;
    public Button thisButton;
    public GameObject inActiveSymbol;
    
    // -- ToolTip --
    private ToolTipMessenger toolTipMessenger;
    public float toolTipDelay = 0.5f;
    float timer;
    bool hasMouse;
    ActionManager actionManager;

    private void Start() {
        animator = GetComponent<Animator>();
        actionManager = GameObject.Find("ActionManager").GetComponent<ActionManager>();
        toolTipMessenger = GameObject.Find("ToolTip").GetComponent<ToolTipMessenger>();

        hasMouse = false;
    }


    public void UpdateActionButton(Action action) {
        // Update the action button with the action information
        actionName = action.actionName;
        actionEnergyCost = action.actionEnergyCost;
        actionImage.sprite = action.actionImage;
        actionEnergyCostText.text = actionEnergyCost.ToString();
        CreateDesriptionText(action.numberOfDice, action.numberOfSides, action.valueToAdd, action.actionType);

        currentAction = action;
    }

    private void CreateDesriptionText(int numberOfDice, int numberOfSides, string valueToAdd, ActionType actionType) {
        // Create the description text for the action
        // Check if valueToAdd is empty or null
        switch (actionType.ToString()) {
            case "Damage":
                if (string.IsNullOrEmpty(valueToAdd)) {
                    actionDescription = "Deal " + numberOfDice + "d" + numberOfSides + " damage";
                } else {
                    actionDescription = "Deal " + numberOfDice + "d" + numberOfSides + " + " + valueToAdd + " damage";
                }
                break;
            case "Heal":
                if (string.IsNullOrEmpty(valueToAdd)) {
                    actionDescription = "Heal " + numberOfDice + "d" + numberOfSides + " health";
                } else {
                    actionDescription = "Heal " + numberOfDice + "d" + numberOfSides + " + " + valueToAdd + " health";
                }
                break;
            case "Shield":
                if (string.IsNullOrEmpty(valueToAdd)) {
                    actionDescription = "Shield " + numberOfDice + "d" + numberOfSides + " health";
                } else {
                    actionDescription = "Shield " + numberOfDice + "d" + numberOfSides + " + " + valueToAdd + " health";
                }
                break;
            default:
                Debug.Log("Invalid action type");
                break;
        }
    }

    private void Update() {
        if (hasMouse && timer < toolTipDelay) {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay) {
                toolTipMessenger.Show(actionName, actionDescription);
            }
        }

        // If the action energy cost is greater than the player's energy, disable the button
        if (actionEnergyCost > actionManager.playerEnergy) {
            thisButton.interactable = false;
            inActiveSymbol.SetActive(true);
        } else {
            thisButton.interactable = true;
            inActiveSymbol.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        timer = 0;
        hasMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        hasMouse = false;
        toolTipMessenger.Hide();
    }

    public void OnClick() {
        // // Check if the action energy cost is greater than the player's energy
        // if (actionEnergyCost > actionManager.playerEnergy) {
        //     return;
        // }
        // Execute the action
        if (actionManager.IsValidAction(currentAction)) {
            actionManager.AnimatePlayerShips(currentAction.actionType);

        }
    }

    
    
}