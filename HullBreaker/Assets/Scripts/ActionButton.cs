using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

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
        ChangeImageColor(action);
        actionEnergyCostText.text = actionEnergyCost.ToString();
        currentAction = action;

        //CreateDesriptionText(action.numberOfDice, action.numberOfSides, action.valueToAdd, action.actionType, action.statusAmount);
        actionDescription = ResourceLoader.CreateDescriptionOfAction(action);
    }

    private void CreateDesriptionText(int numberOfDice, int numberOfSides, string valueToAdd, ActionType actionType, int statusAmount) {
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

        // If the action has a status effect, add it to the description
        // Effects: Waterlogged, Burning, Regeneration, Virus, Reinforced, Improved, Accuracy
        switch (currentAction.statusEffect.ToString()) {
            case "None":
                break;
            case "Waterlogged":
                actionDescription += " and inflict " + statusAmount + " Waterlogged";
                break;
            case "Burning":
                actionDescription += " and inflict " + statusAmount + " Burning";
                break;
            case "Regeneration":
                actionDescription += " and gain " + statusAmount + " Regeneration";
                break;
            case "Virus":
                actionDescription += " and inflict " + statusAmount + " Virus";
                break;
            case "Reinforced":
                actionDescription += " and gain " + statusAmount + " Reinforced";
                break;
            case "Improved":
                actionDescription += " and gain " + statusAmount + " Improved";
                break;
            case "Accuracy":
                actionDescription += " and gain " + statusAmount + " Accuracy";
                break;
            default:
                Debug.Log("Invalid status effect");
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

        // If "R" is pressed, Reset scene
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
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
        actionManager.IsValidAction(currentAction);
    }
    public void ChangeImageColor(Action action) {
        // Change the color of the action image based on the class type
        switch (action.classType) {
            case ClassInfo.ClassType.Mechanical:
            // White
            actionImage.color = new Color(1, 1, 1, 1);
            break;
            case ClassInfo.ClassType.Thermal:
            // Red
            actionImage.color = new Color(1, 0, 0, 1);
            break;
            case ClassInfo.ClassType.Bio:
            // Green
            actionImage.color = new Color(0, 1, 0, 1);
            break;
            case ClassInfo.ClassType.Chemical:
            // Pink
            actionImage.color = new Color(1, 0, 1, 1);
            break;
            case ClassInfo.ClassType.Hydro:
            // Blue
            actionImage.color = new Color(0, 0, 1, 1);
            break;
        }
    }
    
}