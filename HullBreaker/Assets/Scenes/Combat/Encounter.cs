using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This is a scriptable object that contains information for enemy encounters

/*

    -- Contents --

    - Encounter Faction
    - Encounter Name
    - Difficulty
    - Enemy Ships - Up to 4 sprites of enemy ships that will be used in the encounter
    - Enemy Actions - The actions the enemy can perform
    - AI - The AI that will control the enemy ships
    - Base Health - The base health of the enemy ships
    - Actions Per Turn - The number of actions the enemy can perform per turn

*/

[CreateAssetMenu(fileName = "New Encounter", menuName = "HullBreaker/Encounter")]
public class Encounter : ScriptableObject
{
    public int encounterIndex;
    public EnumHolder.Faction encounterFaction;
    public string encounterName;
    public int difficulty;
    public List<Ship> enemyShips = new List<Ship>();
    public EnumHolder.AI ai;
    public int baseHealth;
    [Range(1, 8)]
    public int actionsPerTurn;
}
