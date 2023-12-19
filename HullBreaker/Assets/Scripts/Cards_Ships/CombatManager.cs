using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    // Making the accessable from anywhere
    public static CombatManager Instance { get; private set; }

    // Get the playerShips from the Player object
    public Player player = Player.Instance;

    // The enemy's ships
    public List<EnemyShip> enemyShips = new List<EnemyShip>();
    public List<EnemyShip> enemyShipsDestroyed = new List<EnemyShip>();

    // Player and enemy action queues
    public List<Action> playerActionQueue = new List<Action>();
    public List<Action> enemyActionQueue = new List<Action>();

    // Player stats
    public int playerHealth;
    public int playerHealthMax;
    public int playerShield;
    public int playerEnergy;
    public int playerEnergyMax;

    // UI elements
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerShieldText;
    public TextMeshProUGUI playerEnergyText;

    // Combat state
    public bool playerTurn;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the player's health and energy to their values in the Player object
        playerHealth = player.playerHealth;
        playerHealthMax = player.playerHealthMax;
        playerEnergy = player.playerEnergyMax;
        playerEnergyMax = player.playerEnergyMax;

        // Set the player's turn to true
        playerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the energy text
        playerEnergyText.text = playerEnergy.ToString() + "/" + playerEnergyMax.ToString();
    }

    public void AddActionToQueue(Action action) {
        // Add the specified action to the playerActionQueue
        playerActionQueue.Add(action);
    }
    

}
