using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "HullBreaker/Item")]

public class UpgradeItem : ScriptableObject
{
    [Header("Item Information")]
    // The item index
    public int itemIndex;
    
    // The name of the item
    public string itemName;

    // The item base price (used for selling)
    public int basePrice;

    // The image of the item
    public Sprite itemImage;
    
    // Properties of the item
    [Header("Properties")]

    public Trigger trigger;
    public Effect effect;
    public Target target;
    public EnumHolder.StatusEffect statusEffect;
    public int value;
    
}

// Enums for the different effects that an item can have

public enum Trigger {
    OnStartOfPlayerTurn,
    Passive
}

public enum Target {
    Self,
    Enemy
}

public enum Effect {
    Heal,
    Damage,
    Shield,
    Energy,
    StatusEffect 
}


