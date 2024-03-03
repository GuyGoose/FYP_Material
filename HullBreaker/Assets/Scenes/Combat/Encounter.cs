using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a scriptable object that contains information for enemy encounters

/*

    -- Contents --

    - Encounter Faction
    - Difficulty
    - Enemy Ships - Up to 4 sprites of enemy ships that will be used in the encounter
    - Enemy Actions - The actions the enemy can perform
    - AI - The AI that will control the enemy ships
    

*/

[CreateAssetMenu(fileName = "New Encounter", menuName = "HullBreaker/Encounter")]

public class Encounter : ScriptableObject
{
    public Faction encounterFaction;
    public int difficulty;
    public List<Ship> enemyShips = new List<Ship>();
    public List<Action> enemyActions = new List<Action>();
    public AI ai;
}

// Enums for the faction of the encounter
public enum Faction
{
    Enforcers,
    Merchants,
    Outlaws,
    Cultists,
    ExEmployees,
    HullBreakers
}

public enum AI
{
    Random,
    Aggressive,
    Defensive,
    Balanced
}
