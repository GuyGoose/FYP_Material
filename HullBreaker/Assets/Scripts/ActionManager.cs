using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    public Reward rewardData;

    public GameObject Action1, Action2, Action3, Action4;
    public List<GameObject> PlayerShipAnimation, EnemyShipAnimation;
    public GameObject PlayerValueText, EnemyValueText, FadeScreen;
    public GameObject SelectedShip;
    
    // Game States (PlayerTurn, EnemyTurn)
    public enum GameState {
        PlayerTurn,
        EnemyTurn
    }

    void Awake() {
        // Load the player info
        playerInfo.GetComponent<PlayerInfo>().LoadPlayerInfo();

        // Setting up encounter
        encounter = playerInfo.GetComponent<PlayerInfo>().currentEncounter;
        
        // Player and Enemy HP Info
        playerInfo.GetComponent<Health>().SetupHealth(playerInfo.GetComponent<PlayerInfo>().maxHealth, playerInfo.GetComponent<PlayerInfo>().currentHealth);

        int scoreAdjustedEnemyHealth = ResourceLoader.DynamicDifficultyCalculateScoreBonus(playerInfo.GetComponent<PlayerInfo>().score, encounter.baseHealth);
        enemyInfo.GetComponent<Health>().SetupHealth(scoreAdjustedEnemyHealth, scoreAdjustedEnemyHealth);
    
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

        // Apply the passive effects of the player's items
        ProcessItemsInCombat.ProcessPassiveItems(playerInfo.GetComponent<PlayerInfo>(), enemyInfo, playerInfo);

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
                int value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd, target, action);
                ApplyBonusDamage(value, target, action.classType);
                Debug.Log("Value: " + value);
                // Deal damage to the target
                target.GetComponent<Health>().TakeDamage(value);
                // Apply status effect
                if (action.statusEffect != EnumHolder.StatusEffect.None) {
                    // Apply the status effect
                    target.GetComponent<StatusEffect>().ApplyStatusEffect(action.statusEffect, action.statusAmount);
                }

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
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd, target, action);
                // Heal the target
                target.GetComponent<Health>().Heal(value);
                // Apply status effect
                if (action.statusEffect != EnumHolder.StatusEffect.None) {
                    // Apply the status effect
                    target.GetComponent<StatusEffect>().ApplyStatusEffect(action.statusEffect, action.statusAmount);
                }

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
                value = RollDice(action.numberOfDice, action.numberOfSides, action.valueToAdd, target, action);
                // Shield the target
                target.GetComponent<Health>().currentShield += value;
                // Apply status effect
                if (action.statusEffect != EnumHolder.StatusEffect.None) {
                    // Apply the status effect
                    target.GetComponent<StatusEffect>().ApplyStatusEffect(action.statusEffect, action.statusAmount);
                }

                if (target == playerInfo) { 
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Self, "+" + value.ToString());
                } else {
                    // Display the value of the action
                    DisplayActionValue(ActionTargets.Enemy, "+" + value.ToString());
                }
                break;
            case ActionType.StatusEffect:
                // Apply the status effect
                target.GetComponent<StatusEffect>().ApplyStatusEffect(action.statusEffect, action.statusAmount);
                break;
            default:
                Debug.Log("Invalid action type");
                break;
        }

        // Check if the enemy is defeated
        if (target == enemyInfo && enemyInfo.GetComponent<Health>().currentHealth <= 0) {
            // End the game
            StartCoroutine(EndCombat());
        }

        // Check if the player is defeated
        if (target == playerInfo && playerInfo.GetComponent<Health>().currentHealth <= 0) {
            // End the game
            StartCoroutine(GameOverCombat());
        }
    }

    // RollDice rolls the dice and returns the value
    public int RollDice(int numberOfDice, int numberOfSides, string valueToAdd, GameObject target, Action action) {

        // If target is player, check for enemy Improved or Accuracy status effects
        if (target == playerInfo && action.actionType == ActionType.Damage) {
            // Check enemy Improved status effect
            int improved = enemyInfo.GetComponent<StatusEffect>().CheckImproved();
            // If improved is greater than 0, add it to the number of dice
            if (improved > 0) {
                numberOfDice += improved;
            }
            // Check enemy Accuracy status effect
            int accuracy = enemyInfo.GetComponent<StatusEffect>().CheckAccuracy();
            // If accuracy is greater than 0, add it to the number of dice
            if (accuracy > 0) {
                numberOfSides += accuracy;
            }
        } else if (target == enemyInfo && action.actionType == ActionType.Damage) {
            // Check player Improved status effect
            int improved = playerInfo.GetComponent<StatusEffect>().CheckImproved();
            // If improved is greater than 0, add it to the number of dice
            if (improved > 0) {
                numberOfDice += improved;
            }
            // Check player Accuracy status effect
            int accuracy = playerInfo.GetComponent<StatusEffect>().CheckAccuracy();
            // If accuracy is greater than 0, add it to the number of dice
            if (accuracy > 0) {
                numberOfSides += accuracy;
            }
        }

        int value = 0;
        for (int i = 0; i < numberOfDice; i++) {
            value += Random.Range(1, numberOfSides + 1);
        }
        // TODO: Add the value to the roll
        //value += int.Parse(valueToAdd);

        // Check if the target is waterlogged if so, multiply the value by 1.25
        if (target.GetComponent<StatusEffect>().Waterlogged_nb > 0 && action.actionType == ActionType.Damage) {
            // Round the value to the nearest whole number
            value = Mathf.RoundToInt(value * 1.25f);
        }

        // Check if the user is Virus if so, divide the value by 1.5
        // If target is player, check for enemy Virus status effects
        if (target == playerInfo && action.actionType == ActionType.Damage) {
            // Check enemy isVirus 
            if (enemyInfo.GetComponent<StatusEffect>().Virus_nb > 0) {
                // Round the value to the nearest whole number
                value = Mathf.RoundToInt(value / 1.5f);
            }
        } else if (target == enemyInfo && action.actionType == ActionType.Damage) {
            // Check player isVirus 
            if (playerInfo.GetComponent<StatusEffect>().Virus_nb > 0) {
                // Round the value to the nearest whole number
                value = Mathf.RoundToInt(value / 1.5f);
            }
        }

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
        // Reset the player's energy
        playerEnergy = playerMaxEnergy;
        playerEnergy += playerInfo.GetComponent<PlayerInfo>().bonusEnergy;
        // Reset the bonus energy
        playerInfo.GetComponent<PlayerInfo>().bonusEnergy = 0;
        currentGameState = GameState.PlayerTurn;
    }

    public void StartEnemyTurn() {
        enemyInfo.GetComponent<Health>().currentShield = 0;
        // Apply the Start of Player Turn items
        ProcessItemsInCombat.ProcessStartOfPlayerTurnItems(playerInfo.GetComponent<PlayerInfo>(), enemyInfo, playerInfo);
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
        // For player and enemy ships, status effects are adjusted at the end of the turn
        playerInfo.GetComponent<StatusEffect>().AdjustAtEndOfTurn(playerInfo.GetComponent<Health>());
        
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

        enemyInfo.GetComponent<StatusEffect>().AdjustAtEndOfTurn(enemyInfo.GetComponent<Health>());
    }

    IEnumerator GameOverCombat() {
        playerTurnMenu.GetComponent<Animator>().SetBool("isActive", false);
        // Destroy Player Ships
        for (int i = 0; i < playerShips.Count; i++) {
            PlayerShipAnimation[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        //wait for a for 1 seconds
        yield return new WaitForSeconds(1f);
        // Display Game Over
        uiManager.DisplayGameOver();
    }

    IEnumerator EndCombat() {
        playerTurnMenu.GetComponent<Animator>().SetBool("isActive", false);
        // Destroy Enemy Ships
        for (int i = 0; i < enemyShips.Count; i++) {
            EnemyShipAnimation[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        //wait for a for 1 seconds
        yield return new WaitForSeconds(1f);
        // Display Game Over (Passing in each ship destroyed)
        uiManager.DisplayCombatWin(enemyShips, encounter.difficulty, playerInfo.GetComponent<PlayerInfo>().isBossFight);
    }

    public void ReturnToMapButton() {
        StartCoroutine(ReturnToMap());
    }

    public IEnumerator ReturnToMap() {
        // Save the player's information
        playerInfo.GetComponent<PlayerInfo>().SavePlayerInfo();
        // Fade in the screen (IEnumerator FadeOut)
        FadeScreen.GetComponent<FadeScreenController>().StartCoroutine("FadeOut");
        //wait for a for 2 seconds
        yield return new WaitForSeconds(2f);
        // Return to the map
        SceneManager.LoadScene("MapScene");
    }

    public void ApplyBonusDamage(int value, GameObject target, ClassInfo.ClassType classType) {
        
    }

    public void SelectShip(GameObject ship) {
        SelectedShip = ship;
    }

    public void AddCredits(int credits) {
        playerInfo.GetComponent<PlayerInfo>().credits += credits;
        playerInfo.GetComponent<PlayerInfo>().score += credits / 4;
    }

}
