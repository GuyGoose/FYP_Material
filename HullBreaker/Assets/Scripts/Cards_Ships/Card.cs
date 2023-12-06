using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "HullBreaker/Card")]

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
    // Card Actions (Scriptable Objects)
    public GameObject[] cardActions;


    public Card() {

    }

    public Card(int Id, string CardName, int Cost, string CardDescription) {
        id = Id;
        cardName = CardName;
        cost = Cost;
        cardDescription = CardDescription;
    }

}
