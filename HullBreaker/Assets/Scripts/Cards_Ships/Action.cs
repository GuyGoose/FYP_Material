using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "HullBreaker/Action", order = 0)]
public class Action : ScriptableObject {

    // Action Name
    [SerializeField]
    public string actionName;

    // Action Description
    [SerializeField]
    public string actionDescription;

    // Action Type
    [SerializeField]
    public actionType actionType;

    // Action Value
    [SerializeField]
    public int actionValue;

    // Action Energy Cost
    [SerializeField]
    public int actionEnergyCost;

    // Action Targets
    [SerializeField]
    public actionTarget actionTarget;
    
}

public enum actionType {
    Damage,
    Shield,
    Repair,
    Recharge
}

public enum actionTarget {
    Self,
    Enemy,
    AllEnemies,
    AllAllies,
    All
}

