using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDatabase : MonoBehaviour
{
    public GameObject cardDatabaseObject;

    public List<Ship> shipList = new List<Ship>();
    
    public List<Ship> playerShips = new List<Ship>();
    public List<Ship> playerShipsDestroyed = new List<Ship>();

    public List<Ship> enemyShips = new List<Ship>();
    public List<Ship> enemyShipsDestroyed = new List<Ship>();

    private void Awake() {
        // Add all ship scriptable objects contained in the Ships folder to the shipList
        Ship[] ships = Resources.LoadAll<Ship>("Ships");
        // Print the number of ships in the shipList
        Debug.Log("Number of ships in shipList: " + ships.Length);
        foreach (Ship ship in ships) {
            shipList.Add(ship);
        }

        // Add random ships from the shipList to the playerShips list
        for (int i = 0; i < 5; i++) {
            int randomIndex = Random.Range(0, shipList.Count);
            playerShips.Add(shipList[randomIndex]);
        }

        // Add all cards associated with each ship in the playerShips list to the playerDeck list in the CardDatabase
        foreach (Ship ship in playerShips) {
            foreach (Card card in ship.ShipCards) {
                cardDatabaseObject.GetComponent<CardDatabase>().playerDeck.Add(card);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
