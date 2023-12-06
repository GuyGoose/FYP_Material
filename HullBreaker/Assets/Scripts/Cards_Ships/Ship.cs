using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Ship", menuName = "HullBreaker/Ship")]
public class Ship : ScriptableObject
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string shipName;

    [SerializeField]
    private int health;

    [SerializeField]
    private ShipSize size;

    [SerializeField]
    private Rarity shipRarity;

    // The ships associated card(s)
    [SerializeField]
    private List<Card> shipCards;

    [SerializeField]
    private Sprite shipSprite;

    [SerializeField]
    private ShipType shipType;

    // Getters and setters for the serialized fields
    
    public int Id
    {
        get { return id; }
        private set { id = value; }
    }

    public string ShipName
    {
        get { return shipName; }
        set { shipName = value; }
    }
    
    public ShipType Type
    {
        get { return shipType; }
        set { shipType = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public ShipSize Size
    {
        get { return size; }
        set { size = value; }
    }

    public Rarity ShipRarity
    {
        get { return shipRarity; }
        set { shipRarity = value; }
    }

    public List<Card> ShipCards
    {
        get { return shipCards; }
        set { shipCards = value; }
    }

    public Sprite ShipSprite
    {
        get { return shipSprite; }
        set { shipSprite = value; }
    }
}

// Enum for ship rarity
public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

// Enum for ship types
public enum ShipType
{
    Drone,
    Battleship,
    Surveyor,
    Tanker,
    Carrier,
    Destroyer,
    Specialist
}

// Enum for ship sizes
public enum ShipSize
{
    Tiny,
    Small,
    Medium,
    Large
}