using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : ScriptableObject
{
    // Identity Info
    public int id;
    public string cardName;
    // Description
    public bool customDescription;
    public string cardDescription;
    // Card Attributes
    public int cost;
    public bool hasTarget;
    public bool voidOnUse;
    // Card Actions
    // actionType 0 = none, 1 = damage, 2 = heal, 3 = shield, 4 = draw, 5 = discard, 6 = buff, 7 = debuff
    public enum ActionTypes { None, Damage, Heal, Shield, Draw, Discard, Buff, Debuff };
    public ActionTypes actionType1;
    

    public Card() {

    }

    public Card(int Id, string CardName, int Cost, string CardDescription) {
        id = Id;
        cardName = CardName;
        cost = Cost;
        cardDescription = CardDescription;
    }

}
