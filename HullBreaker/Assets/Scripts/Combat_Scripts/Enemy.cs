using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // The enemy Ship
    public EnemyShip enemyShip;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Update the enemy's health display
        if (enemyHealthDisplay.fillAmount != (float)enemyHealth / (float)enemyHealthMax) {
            enemyHealthDisplay.fillAmount -= (enemyHealthDisplay.fillAmount - ((float)enemyHealth / (float)enemyHealthMax)) / 100;
        }

    }
}
