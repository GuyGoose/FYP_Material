using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    // This script handles the actions of the player and enemy ships
    // It is sent one or more actions to perform and it will perform the actions on the target

    // An action is a scriptable object that contains the following: 
    // - Action Name
    // - Action Type
    // - Action Value
    // - Action Energy Cost
    // - Action Target

    // THINGS TO NOTE:
    /*
    - The amount of damage, healing, etc. is done via a DND style dice roll (e.g. 2d6 + 3)
    - Actions are recieved as a scriptable object containing the following: 
    -- Target: The target of the action (e.g. Self, Enemy)
    -- Action: The action to perform (e.g. Damage, Heal, Shield etc.)
    -- Value: The value of the action (e.g. fed in as (x,y,z) for xdy + z)

    - The Player can have up to 4 ships but the player has only 1 HP pool
    - The enemy can have any number of ships but the enemy has only 1 HP pool
    -- These ships are considered to be part of the same "fleet" and are treated as such (treated as a single entity for the purposes of actions and damage etc.)

    - A player can have up to 4 actions contained on each ship
    - An enemy can have any number of actions contained thier ship

    - It costs the player energy to perform an action and the player has a limited amount of energy
    - The enemy has unlimited energy but only predefined number of actions they can perform per turn

    - The actions are performed in the order they are recieved

    - Player actions are recieved from the UI and are executed upon a button press
    - Enemy actions are recieved from the AI and are executed upon the AI's turn in order
    */

    public Encounter encounter;
    public GameObject enemyHPInfo;
    public GameObject playerHPInfo;
    public List<Ship> playerShips = new List<Ship>();
    public List<Ship> enemyShips = new List<Ship>();
    public GameObject playerInfo;
    public int playerEnergy;
    private Ship currentlySelectedShip;
    public UiManager uiManager;

    public GameObject Action1, Action2, Action3, Action4;
    //public GameObject ShipTab1, ShipTab2, ShipTab3, ShipTab4;

    // Start is called before the first frame update
    void Start()
    {
        // Get player info 
        // -- This also contains the player's energy and the player's ships
        playerEnergy = playerInfo.GetComponent<PlayerInfo>().energy;
        playerShips = playerInfo.GetComponent<PlayerInfo>().ships;

        uiManager.SetupShipButtons(playerShips);
    }

    // IsValidAction checks if the action is valid
    public bool IsValidAction(Action action) {
        // Does the action cost more energy than the player has?
        if (action.actionEnergyCost > playerEnergy) {
            return false;
        }
        ExecuteAction(action);
        return true;
    }

    // ExecuteAction performs the action on the target
    public void ExecuteAction(Action action) {
        // Get the action type
        GameObject target = DetermineTarget(action.actionTargets);
        switch (action.actionType.ToString()) {
            case "Damage":
                // Get the value of the action
                int value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                // Deal damage to the target
                target.GetComponent<Health>().TakeDamage(value);
                break;
            case "Heal":
                // Get the value of the action
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                // Heal the target
                target.GetComponent<Health>().Heal(value);
                break;
            case "Shield":
                // Get the value of the action
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                // Shield the target
                target.GetComponent<Health>().currentShield += value;
                break;
            default:
                Debug.Log("Invalid action type");
                break;
        }
    }

    // RollDice rolls the dice and returns the value
    public int RollDice(int numberOfDice, int numberOfSides, string valueToAdd) {
        int value = 0;
        for (int i = 0; i < numberOfDice; i++) {
            value += Random.Range(1, numberOfSides + 1);
        }
        // TODO: Add the value to the roll
        //value += int.Parse(valueToAdd);
        return value;
    }

    public GameObject DetermineTarget(ActionTargets actionTarget) {
        // Determine the target of the action
        switch (actionTarget.ToString()) {
            case "Self":
                return playerHPInfo;
            case "Enemy":
                return enemyHPInfo;
            default:
                Debug.Log("Invalid action target");
                return null;
        }
    }

    public void UpdateCurrentShip(Ship ship) {
        currentlySelectedShip = ship;

        // Get the actions for the ship and update action buttons
        Action1.GetComponent<ActionButton>().UpdateActionButton(ship.actions[0]);
        Action2.GetComponent<ActionButton>().UpdateActionButton(ship.actions[1]);
        Action3.GetComponent<ActionButton>().UpdateActionButton(ship.actions[2]);
        Action4.GetComponent<ActionButton>().UpdateActionButton(ship.actions[3]);
    }

}
