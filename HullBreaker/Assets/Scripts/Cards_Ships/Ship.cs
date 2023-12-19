using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Ship", menuName = "HullBreaker/Ship")]
public class Ship : ScriptableObject
{
    [SerializeField]
    public int id;

    [SerializeField]
    public Sprite shipSprite;

    [SerializeField]
    public List<Action> shipActions = new List<Action>();

    // Getters and setters for the serialized fields

    public int Id
    {
        get { return id; }
        private set { id = value; }
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

    public void PrintShipInfo() {
        Debug.Log("Ship ID: " + id);
        Debug.Log("Ship Sprite: " + shipSprite);
        Debug.Log("Ship Actions: " + shipActions);
    }
}
