using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardAction", menuName = "HullBreaker/CardAction", order = 0)]
public class CardAction : ScriptableObject {

    // Action Type (Attack, Defense, Utility)
    public ActionType actionType;

    // Action Functionality
    public ActionFunction actionFunction;

    // Action Target (Self, Enemy, All Enemies, All Allies, All Ships)
    public ActionTarget actionTarget;

    // Action Value
    public int actionValue;
    
}

public enum ActionType {
    Attack,
    Defense,
    Utility
}

public enum ActionFunction {
    Damage,
    Vamp_Damage,
    Cost_Damage,
    Heal,
    Draw,
    Discard,
    Destroy,
    Repair,
    Buff,
    Debuff,
    Stun
}

public enum ActionTarget {
    Self,
    Enemy,
    AllEnemies,
    AllAllies,
    AllShips
}

