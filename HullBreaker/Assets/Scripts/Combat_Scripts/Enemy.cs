using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // The enemy Ship
    public EnemyShip enemyShip;

    // Get the combat manager
    public CombatManager combatManager = CombatManager.Instance;

    // Stats
    public int enemyHealth;
    public int enemyHealthMax;
    public Image enemyHealthDisplay;
    // Visuals
    public Sprite enemySprite;
    // Info
    public string enemyName;
    public int enemyId;

    // Actions
    public List<Action> availableActions = new List<Action>();

    public int currentActionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the enemy's variables from the EnemyShip object
        enemyHealth = enemyShip.Health;
        enemyHealthMax = enemyShip.Health;
        enemySprite = enemyShip.ShipSprite;
        enemyName = enemyShip.ShipName;
        enemyId = enemyShip.Id;

        // Populate availableActions with the actions from the EnemyShip object
        foreach (Action action in enemyShip.ShipActions) {
            availableActions.Add(action);
        }

        // Set the enemy's image to the enemySprite
        GetComponent<Image>().sprite = enemySprite;

        // Get the size of the actions list and set the currentActionIndex to a random number between 0 and the size of the list
        int actionListSize = availableActions.Count;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the enemy's health display
        if (enemyHealthDisplay.fillAmount != (float)enemyHealth / (float)enemyHealthMax) {
            enemyHealthDisplay.fillAmount -= (enemyHealthDisplay.fillAmount - ((float)enemyHealth / (float)enemyHealthMax)) / 100;
        }

        // If the enemy's health is 0, destroy it
        if (enemyHealth <= 0) {
            Destroy(gameObject);
        }

        // If the enemy health goes above the max, set it to the max
        if (enemyHealth > enemyHealthMax) {
            enemyHealth = enemyHealthMax;
        }
    }

    public void SupplyAction() {
        // Add the action to the enemy's enemyActionQueue
        print("Enemy " + enemyName + " added " + availableActions[currentActionIndex].actionName + " to the enemyActionQueue");
        combatManager.enemyActionQueue.Add(availableActions[currentActionIndex]);

        // Increment the currentActionIndex
        currentActionIndex++;
        // If the currentActionIndex is greater than the size of the availableActions list, set it to 0
        if (currentActionIndex > availableActions.Count-1) {
            currentActionIndex = 0;
        }
    }
}
