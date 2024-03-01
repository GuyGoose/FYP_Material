// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class CombatManager : MonoBehaviour
// {
//     // Making the accessable from anywhere
//     public static CombatManager Instance { get; private set; }

//     // Get the playerShips from the Player object
//     public Player player = Player.Instance;

//     // The enemy's ships
//     public List<Enemy> enemyShips = new List<Enemy>();
//     public List<Enemy> enemyShipsDestroyed = new List<Enemy>();

//     // Player and enemy action queues
//     public List<Action> playerActionQueue = new List<Action>();
//     public List<Action> enemyActionQueue = new List<Action>();

//     // Player stats
//     public int playerHealth;
//     public int playerHealthMax;
//     public Image playerHealthDisplay;
//     public int playerShield;
//     public Image playerShieldDisplay;
//     public int playerEnergy;
//     public int playerEnergyMax;

//     // UI elements
//     public TextMeshProUGUI playerHealthText;
//     public TextMeshProUGUI playerShieldText;
//     public TextMeshProUGUI playerEnergyText;

//     // Combat state
//     public bool playerTurn;
    
//     // Start is called before the first frame update
//     void Start()
//     {
//         // Set the player's health and energy to their values in the Player object
//         playerHealth = player.playerHealth;
//         playerHealthMax = player.playerHealthMax;
//         playerEnergy = player.playerEnergyMax;
//         playerEnergyMax = player.playerEnergyMax;

//         // Set the player's turn to true
//         playerTurn = true;

//         // Populate enemyShips with the Objects tagged "Enemies"
//         foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemies")) {
//             enemyShips.Add(enemy.GetComponent<Enemy>());
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Update the energy text
//         playerEnergyText.text = playerEnergy.ToString() + "/" + playerEnergyMax.ToString();

//         //// HP and Shield ////
        
//         // Update the health text
//         playerHealthText.text = playerHealth.ToString() + "/" + playerHealthMax.ToString();
//         // Update the health display
//         if (playerHealthDisplay.fillAmount != (float)playerHealth / (float)playerHealthMax) {
//             playerHealthDisplay.fillAmount -= (playerHealthDisplay.fillAmount - ((float)playerHealth / (float)playerHealthMax)) / 100;
//         }

//         // Update the shield display and text
//         if (playerShield > 0) {
//             // Fill the shield display until it's full
//             if (playerShieldDisplay.fillAmount != 1) {
//                 playerShieldDisplay.fillAmount += 0.002f;
//             }
//             playerShieldText.enabled = true;
//             playerShieldText.text = playerShield.ToString();
//         } else {
//             // Empty the shield display until it's empty
//             if (playerShieldDisplay.fillAmount != 0) {
//                 playerShieldDisplay.fillAmount -= 0.002f;
//             }
//             playerShieldText.enabled = false;
//         }

//     }

//     public void AddActionToQueue(Action action) {
//         // Add the specified action to the playerActionQueue
//         playerActionQueue.Add(action);
//     }

//     // This function is called when the player's turn ends
//     public void EndPlayerTurn() {
//         // Set the player's turn to false
//         playerTurn = false;

//         // Run through the playerActionQueue and Perform each action
//         foreach (Action action in playerActionQueue) {
//             PerformAction(action);
//         }

//         // Clear the playerActionQueue
//         playerActionQueue.Clear();

//         // Reset the clicked bool for each action button
//         foreach (ActionButton actionButton in GameObject.FindObjectsOfType<ActionButton>()) {
//             actionButton.clicked = false;
//         }

//         // Request the enemys' action for this turn
//         foreach (Enemy enemy in enemyShips) {
//             enemy.SupplyAction();
//         }

//         // Run through the enemyActionQueue and Perform each action
//         foreach (Action action in enemyActionQueue) {
//             PerformAction(action);
//         }

//         // Clear the enemyActionQueue
//         enemyActionQueue.Clear();

//         // Run StartPlayerTurn
//         StartPlayerTurn();
//     }

//     // This function is called when the player's turn starts
//     public void StartPlayerTurn() {
//         // Reset the player's energy
//         playerEnergy = playerEnergyMax;

//         // Set the player's turn to true
//         playerTurn = true;
//     }



//     //////////////////// ACTIONS ////////////////////

//     private void PerformAction(Action action) {
//         // Switch statement for the action's type
//         switch (action.actionType) {
//             case actionType.Damage:
//                 // If the action is a damage action, run the Damage function
//                 Damage(action.actionTarget, action.actionValue);
//                 break;
//             case actionType.Shield:
//                 // If the action is a shield action, run the Shield function
//                 Shield(action.actionTarget, action.actionValue);
//                 break;
//             case actionType.Repair:
//                 // If the action is a repair action, run the Repair function
//                 Repair(action.actionTarget, action.actionValue);
//                 break;
//             case actionType.Recharge:
//                 // If the action is a recharge action, run the Recharge function
//                 Recharge(action.actionTarget, action.actionValue);
//                 break;

//         }
//     }

//     private void Damage(actionTarget target, int value) {
//         // Switch statement for the action's target
//         switch (target) {
//             case actionTarget.Self:
//                 // If the target is self, damage the player
//                 // If the pplayer has shield, damage the shield and then the health
//                 if (playerShield > 0) {
//                     playerShield -= value;
//                     if (playerShield < 0) {
//                         playerHealth += playerShield;
//                         playerShield = 0;
//                     }
//                 } else {
//                     playerHealth -= value;
//                 }
//                 break;
//             case actionTarget.Enemy:
//                 // If the target is enemy, damage a random enemy
//                 enemyShips[Random.Range(0, enemyShips.Count)].enemyHealth -= value;
//                 break;
//             case actionTarget.AllEnemies:
//                 // If the target is all enemies, damage all enemies
//                 foreach (Enemy enemy in enemyShips) {
//                     enemy.enemyHealth -= value;
//                 }
//                 break;
//             case actionTarget.AllAllies:
//                 // If the target is all allies, damage all allies
//                 playerHealth -= value;
//                 break;
//             case actionTarget.All:
//                 // If the target is all, damage all
//                 playerHealth -= value;
//                 foreach (Enemy enemy in enemyShips) {
//                     enemy.enemyHealth -= value;
//                 }
//                 break;
//         }
//     }

//     private void Shield(actionTarget target, int value) {
//         // Switch statement for the action's target
//         switch (target) {
//             case actionTarget.Self:
//                 // If the target is self, shield the player
//                 playerShield += value;
//                 break;
//             // case actionTarget.Enemy:
//             //     // If the target is enemy, shield a random enemy
//             //     enemyShips[Random.Range(0, enemyShips.Count)].enemyShield += value;
//             //     break;
//             // case actionTarget.AllEnemies:
//             //     // If the target is all enemies, shield all enemies
//             //     foreach (Enemy enemy in enemyShips) {
//             //         enemy.enemyShield += value;
//             //     }
//             //     break;
//             // case actionTarget.AllAllies:
//             //     // If the target is all allies, shield all allies
//             //     playerShield += value;
//             //     break;
//             // case actionTarget.All:
//             //     // If the target is all, shield all
//             //     playerShield += value;
//             //     foreach (Enemy enemy in enemyShips) {
//             //         enemy.enemyShield += value;
//             //     }
//             //     break;
//         }
//     }

//     private void Repair(actionTarget target, int value) {
//         // Switch statement for the action's target
//         switch (target) {
//             case actionTarget.Self:
//                 // If the target is self, repair the player
//                 playerHealth += value;
//                 break;
//             case actionTarget.Enemy:
//                 // If the target is enemy, repair a random enemy
//                 enemyShips[Random.Range(0, enemyShips.Count)].enemyHealth += value;
//                 break;
//             case actionTarget.AllEnemies:
//                 // If the target is all enemies, repair all enemies
//                 foreach (Enemy enemy in enemyShips) {
//                     enemy.enemyHealth += value;
//                 }
//                 break;
//             case actionTarget.AllAllies:
//                 // If the target is all allies, repair all allies
//                 playerHealth += value;
//                 break;
//             case actionTarget.All:
//                 // If the target is all, repair all
//                 playerHealth += value;
//                 foreach (Enemy enemy in enemyShips) {
//                     enemy.enemyHealth += value;
//                 }
//                 break;
//         }
//     }

//     private void Recharge(actionTarget target, int value) {
//         // Switch statement for the action's target
//         switch (target) {
//             case actionTarget.Self:
//                 // If the target is self, recharge the player's energy
//                 playerEnergy += value;
//                 break;
//             // case actionTarget.Enemy:
//             //     // If the target is enemy, recharge a random enemy's energy
//             //     enemyShips[Random.Range(0, enemyShips.Count)].enemyEnergy += value;
//             //     break;
//             // case actionTarget.AllEnemies:
//             //     // If the target is all enemies, recharge all enemies' energy
//             //     foreach (Enemy enemy in enemyShips) {
//             //         enemy.enemyEnergy += value;
//             //     }
//             //     break;
//             // case actionTarget.AllAllies:
//             //     // If the target is all allies, recharge all allies' energy
//             //     playerEnergy += value;
//             //     break;
//             // case actionTarget.All:
//             //     // If the target is all, recharge all
//             //     playerEnergy += value;
//             //     foreach (Enemy enemy in enemyShips) {
//             //         enemy.enemyEnergy += value;
//             //     }
//             //     break;
//         }
//     }


//     //////////////////// DEBUG ////////////////////
    
//     // This function is called when the player clicks the "Damage" button
//     public void DebugDamage() {
//         // Damage the player for 25
//         Damage(actionTarget.Self, 25);
//     }

// }
