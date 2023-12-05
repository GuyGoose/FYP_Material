using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public int cost;
    public int value;
    public string cardDescription;

    public Card() {

    }

    public Card(int Id, string CardName, int Cost, int Value, string CardDescription) {
        id = Id;
        cardName = CardName;
        cost = Cost;
        value = Value;
        cardDescription = CardDescription;
    }

}
