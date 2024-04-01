using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// An action is a scriptable object that contains the following: 
// - Action Name
// --- The action description is automatically generated from the action type and value in the ActionDisplay script
// - Action Type
// - Action Value (e.g. (x,y,z) for xdy + z)
// - Action Energy Cost
// - Action Targets
// - Action Image -- For the UI

[CreateAssetMenu(fileName = "New Action", menuName = "HullBreaker/Action", order = 0)]

public class Action : ScriptableObject
{
    public string actionName;
    public ActionType actionType;
    
    // Cost is a slider from 1 to 12
    [Range(1, 12)]
    public int actionEnergyCost;
    public ActionTargets actionTargets;
    public Sprite actionImage;

    // The value of the action is a string in the format (x,y,z) for xdy + z
    // It is parsed into the following fields:
    // - x: The number of dice to roll
    // - y: The number of sides on the dice
    // - z: The value to add to the roll

    // It should have 3 inputs field in the inspector:
    // - The number of dice to roll
    // - The number of sides on the dice
    // -TODO- the value to add to the roll - This is string because it can be taken from various sources (e.g. a dice roll, a fixed value, the value of another action etc.)

    // Place an inspector header called "Action Value" above the inputs
    // The inputs should be labeled "Number of Dice", "Number of Sides", and "Value to Add"

    // Dice rolling and action management is handled by the ActionManager script, This script is just a data container

    [Header("Action Value")]
    public int numberOfDice;
    public int numberOfSides;
    public string valueToAdd;
    public ClassInfo.ClassType classType;

}

// Enums for the action types

public enum ActionType {
    Damage,
    Heal,
    Shield
}

// Enums for the action targets

public enum ActionTargets {
    Self,
    Enemy
}