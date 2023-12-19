using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Making the accessable from anywhere
    public static Player Instance { get; private set; }

    // The player's ships
    public List<Ship> playerShips = new List<Ship>();

    // Available actions
    public List<Action> availableActions = new List<Action>();

    // Player stats
    public int playerHealth;
    public int playerHealthMax;
    public int playerEnergyMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}