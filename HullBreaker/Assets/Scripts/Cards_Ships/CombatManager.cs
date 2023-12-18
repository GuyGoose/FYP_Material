using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // The player's ships
    public List<Ship> playerShips = new List<Ship>();
    public List<Ship> playerShipsInCombat = new List<Ship>();
    public List<Ship> playerShipsDestroyed = new List<Ship>();

    // The enemy's ships
    public List<Ship> enemyShips = new List<Ship>();
    public List<Ship> enemyShipsInCombat = new List<Ship>();
    public List<Ship> enemyShipsDestroyed = new List<Ship>();

    // The Deck and ShipDatabase objects
    public GameObject cardDatabaseObject;
    public GameObject shipDatabaseObject;

}
