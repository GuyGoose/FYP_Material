using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // The health script for the player and enemy ships

    /*
    
    Enemies and Players have:
    - maxHealth
    - currentHealth
    - currentShield

    --- TO BE ADDED ---
    -- Buffs and Debuffs --
    
    */

    public int maxHealth;
    public int currentHealth;
    public int currentShield;

    public bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TakeDamage(int damage) {
        // If the ship has shields, take damage from the shields first
        if (currentShield > 0) {
            currentShield -= damage;
            if (currentShield < 0) {
                currentHealth += currentShield;
                currentShield = 0;
            }
        } else {
            currentHealth -= damage;
        }


        // Check if the ship is destroyed
        if (currentHealth <= 0) {
            if (isPlayer) {
                // Game Over
                Debug.Log("Game Over");
            } else {
                // Enemy defeated
                Debug.Log("Enemy " + this.gameObject.name + " defeated");
            }
        }
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public void Shield(int shieldAmount) {
        currentShield += shieldAmount;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
