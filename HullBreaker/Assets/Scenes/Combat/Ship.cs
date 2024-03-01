using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A is a scriptable object that contains the information for a ship and the actions it can perform

[CreateAssetMenu(fileName = "New Ship", menuName = "HullBreaker/Ship")]

public class Ship : ScriptableObject
{
    // The name of the ship
    public string shipName;

    // A List of (up to 4) actions that the ship can perform
    public List<Action> actions = new List<Action>();

    // The image of the ship
    public Sprite shipImage;
}

