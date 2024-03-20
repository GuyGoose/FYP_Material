using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ActionManager : MonoBehaviour
{
    // This script handles the actions of the player and enemy ships
    // It is sent one or more actions to perform and it will perform the actions on the target

    // An action is a scriptable object that contains the following: 
    // - Action Name
    // - Action Type
    // - Action Value
    // - Action Energy Cost
    // - Action Target

    // THINGS TO NOTE:
    /*
    - The amount of damage, healing, etc. is done via a DND style dice roll (e.g. 2d6 + 3)
    - Actions are recieved as a scriptable object containing the following: 
    -- Target: The target of the action (e.g. Self, Enemy)
    -- Action: The action to perform (e.g. Damage, Heal, Shield etc.)
    -- Value: The value of the action (e.g. fed in as (x,y,z) for xdy + z)

    - The Player can have up to 4 ships but the player has only 1 HP pool
    - The enemy can have any number of ships but the enemy has only 1 HP pool
    -- These ships are considered to be part of the same "fleet" and are treated as such (treated as a single entity for the purposes of actions and damage etc.)

    - A player can have up to 4 actions contained on each ship
    - An enemy can have any number of actions contained thier ship

    - It costs the player energy to perform an action and the player has a limited amount of energy
    - The enemy has unlimited energy but only predefined number of actions they can perform per turn

    - The actions are performed in the order they are recieved

    - Player actions are recieved from the UI and are executed upon a button press
    - Enemy actions are recieved from the AI and are executed upon the AI's turn in order
    */

    public GameState currentGameState;

    public Encounter encounter;
    public GameObject enemyInfo;
    public List<Ship> playerShips = new List<Ship>();
    public List<Ship> enemyShips = new List<Ship>();
    public GameObject playerInfo;
    public int playerMaxEnergy;
    public int playerEnergy;
    public TextMeshProUGUI playerEnergyText;
    private Ship currentlySelectedShip;
    public UiManager uiManager;
    public GameObject playerTurnMenu;

    public GameObject Action1, Action2, Action3, Action4;
    public List<GameObject> PlayerShipAnimation, EnemyShipAnimation;
    public GameObject PlayerValueText, EnemyValueText;
    
    // Game States (PlayerTurn, EnemyTurn)
    public enum GameState {
        PlayerTurn,
        EnemyTurn
    }

    void Awake() {
        // Load the player info
        // playerInfo.GetComponent<PlayerInfo>().LoadPlayerInfo();

        // Setting up encounter
        encounter = playerInfo.GetComponent<PlayerInfo>().currentEncounter;
        
        // Player and Enemy HP Info
        playerInfo.GetComponent<Health>().SetupHealth(playerInfo.GetComponent<PlayerInfo>().maxHealth, playerInfo.GetComponent<PlayerInfo>().currentHealth);
        enemyInfo.GetComponent<Health>().SetupHealth(encounter.baseHealth, encounter.baseHealth);
    
        // Get player info 
        // -- This also contains the player's energy and the player's ships
        playerMaxEnergy = playerInfo.GetComponent<PlayerInfo>().energy;
        playerShips = playerInfo.GetComponent<PlayerInfo>().ships;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager.SetupShipButtons(playerShips);
        enemyShips = encounter.enemyShips;
        // Setup enemy ships sprites
        List<Sprite> enemySprites = new List<Sprite>();
        for (int i = 0; i < enemyShips.Count; i++) {
            enemySprites.Add(enemyShips[i].shipImage);
        }
        uiManager.SetupEnemyShipsImages(enemySprites);

        // Start the player's turn
        StartPlayerTurn();
    }

    // IsValidAction checks if the action is valid
    public bool IsValidAction(Action action) {
        // Does the action cost more energy than the player has?
        if (action.actionEnergyCost > playerEnergy) {
            return false;
        }
        AdjustPlayerEnergy(-action.actionEnergyCost);
        ExecuteAction(action);
        return true;
    }

    // ExecuteAction performs the action on the target
    public void ExecuteAction(Action action) {
        // Get the action type
        Debug.Log("Executing action: " + action.actionName);
        GameObject target = DetermineTarget(action.actionTargets);
        Debug.Log("Target: " + target);
        switch (action.actionType) {
            case ActionType.Damage:
                // Get the value of the action
                int value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                Debug.Log("Value: " + value);
                // Deal damage to the target
                target.GetComponent<Health>().TakeDamage(value);

                if (target == playerInfo) { 
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Self, "-" + value.ToString());
                } else {
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Enemy, "-" + value.ToString());
                }
                break;
            case ActionType.Heal:
                // Get the value of the action
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                // Heal the target
                target.GetComponent<Health>().Heal(value);

                if (target == playerInfo) { 
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Self, "+" + value.ToString());
                } else {
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Enemy, "+" + value.ToString());
                }
                break;
            case ActionType.Shield:
                // Get the value of the action
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd);
                // Shield the target
                target.GetComponent<Health>().currentShield += value;

                if (target == playerInfo) { 
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Self, "+" + value.ToString());
                } else {
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Enemy, "+" + value.ToString());
                }
                break;
            default:
                Debug.Log("Invalid action type");
                break;
        }

        // Check if the enemy is defeated
        if (target == enemyInfo && enemyInfo.GetComponent<Health>().currentHealth <= 0) {
            // End the game
            StartCoroutine(EndGameForDemo());
        }

        // Check if the player is defeated
        if (target == playerInfo && playerInfo.GetComponent<Health>().currentHealth <= 0) {
            // End the game
            //StartCoroutine(EndGameForDemo());
        }
    }

    // RollDice rolls the dice and returns the value
    public int RollDice(int numberOfDice, int numberOfSides, string valueToAdd) {
        int value = 0;
        for (int i = 0; i < numberOfDice; i++) {
            value += Random.Range(1, numberOfSides + 1);
        }
        // TODO: Add the value to the roll
        //value += int.Parse(valueToAdd);
        return value;
    }

    public GameObject DetermineTarget(ActionTargets actionTarget) {
        // Determine the target of the action
        // PlayerTurn: actionTarget - Enemy = enemyShips, actionTarget - Self = playerShips
        // EnemyTurn: actionTarget - Enemy = playerShips, actionTarget - Self = enemyShips
        switch (currentGameState) {
            case GameState.PlayerTurn:
                if (actionTarget == ActionTargets.Enemy) {
                    // Animate the enemy ships taking damage
                    StartCoroutine(AnimateEnemyShips(ActionType.Damage));
                    return enemyInfo;
                } else {
                    // Animate the player ships performing an action
                    StartCoroutine(AnimatePlayerShips(ActionType.Heal));
                    return playerInfo;
                }
            case GameState.EnemyTurn:
                if (actionTarget == ActionTargets.Enemy) {
                    // Animate the player ships taking damage
                    StartCoroutine(AnimatePlayerShips(ActionType.Damage));
                    return playerInfo;
                } else {
                    // Animate the enemy ships performing an action
                    StartCoroutine(AnimateEnemyShips(ActionType.Heal));
                    return enemyInfo;
                }
        }
        return null;
    }

    public void UpdateCurrentShip(Ship ship) {
        currentlySelectedShip = ship;

        // Get the actions for the ship and update action buttons
        Action1.GetComponent<ActionButton>().UpdateActionButton(ship.actions[0]);
        Action2.GetComponent<ActionButton>().UpdateActionButton(ship.actions[1]);
        Action3.GetComponent<ActionButton>().UpdateActionButton(ship.actions[2]);
        Action4.GetComponent<ActionButton>().UpdateActionButton(ship.actions[3]);
    }

    public void StartPlayerTurn() {
        playerInfo.GetComponent<Health>().currentShield = 0;
        playerTurnMenu.GetComponent<Animator>().SetBool("isActive", true);
        playerEnergy = playerMaxEnergy;
        currentGameState = GameState.PlayerTurn;
    }

    public void StartEnemyTurn() {
        enemyInfo.GetComponent<Health>().currentShield = 0;
        playerTurnMenu.GetComponent<Animator>().SetBool("isActive", false);
        currentGameState = GameState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    // Update is called once per frame
    void Update()
    {
        // Player Energy Text (currentEnergy / maxEnergy)
        playerEnergyText.text = playerEnergy + " / " + playerMaxEnergy;
    }

    void AdjustPlayerEnergy(int energy) {
        playerEnergy += energy;
    }

    IEnumerator AnimatePlayerShips(ActionType actionType) {
        // Animate the player ships
        for (int i = 0; i < playerShips.Count; i++) {
            AnimateShip(PlayerShipAnimation[i], actionType);
            //wait for a for 0.2 seconds
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator AnimateEnemyShips(ActionType actionType) {
        // Animate the enemy ships
        for (int i = 0; i < enemyShips.Count; i++) {
            AnimateShip(EnemyShipAnimation[i], actionType);
            //wait for a for 0.2 seconds
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void AnimateShip(GameObject ship, ActionType actionType) {
        // Animate the ship
        switch (actionType) {
            case ActionType.Damage:
                // Animate the ship taking damage
                ship.GetComponent<Animator>().SetTrigger("Damaged");
                break;
            case ActionType.Heal:
                // Animate the ship being healed
                ship.GetComponent<Animator>().SetTrigger("Healed");
                break;
            case ActionType.Shield:
                // Animate the ship being shielded
                ship.GetComponent<Animator>().SetTrigger("Healed");
                break;
        }
    }

    public void DisplayActionValue(ActionTargets actionTargets, string value) {
        // If actionTargets is Enemy, display the value on the enemy value text
        // If actionTargets is Self, display the value on the player value text
        switch (actionTargets) {
            case ActionTargets.Enemy:
                // Text is in child of EnemyValueText.
                EnemyValueText.GetComponentInChildren<TextMeshProUGUI>().text = value;
                // Animate the enemy value text
                EnemyValueText.GetComponent<Animator>().SetTrigger("Value");
                break;
            case ActionTargets.Self:
                PlayerValueText.GetComponentInChildren<TextMeshProUGUI>().text = value;
                // Animate the player value text
                PlayerValueText.GetComponent<Animator>().SetTrigger("Value");
                break;
        }
    }

    IEnumerator EnemyTurn() {
        // For each enemy ship, perform an action
        for (int i = 0; i < enemyShips.Count; i++) {
            // Get the enemy ship's actions
            List<Action> actions = enemyShips[i].actions;
            // Perform the random action
            int randomAction = Random.Range(0, actions.Count);
            ExecuteAction(actions[randomAction]);
            //wait for a for 1 seconds
            yield return new WaitForSeconds(1f);
        }
        // Start the player's turn
        StartPlayerTurn();
    }

    IEnumerator EndGameForDemo() {
        playerTurnMenu.GetComponent<Animator>().SetBool("isActive", false);
        // Destroy Enemy Ships
        for (int i = 0; i < enemyShips.Count; i++) {
            EnemyShipAnimation[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        //wait for a for 1 seconds
        yield return new WaitForSeconds(1f);
        // Display Game Over
        uiManager.DisplayGameOver();
    }
}
