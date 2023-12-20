using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Enemy Ship", menuName = "HullBreaker/Enemy_Ship")]
public class EnemyShip : ScriptableObject
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string shipName;

    [SerializeField]
    private int health;

    [SerializeField]
    private Sprite shipSprite;

    [SerializeField]
    private List<Action> shipActions = new List<Action>();

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

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public Sprite ShipSprite
    {
        get { return shipSprite; }
        set { shipSprite = value; }
    }

    public List<Action> ShipActions
    {
        get { return shipActions; }
        set { shipActions = value; }
    }

}