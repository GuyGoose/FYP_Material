using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    // The health script for the player and enemy ships

    /*
    
    Enemies and Players have:
    - maxHealth
    - currentHealth
    - currentShield

    -- UI --
    - Health Bar
    - Health Text

    --- TO BE ADDED ---
    -- Buffs and Debuffs --
    
    */

    public int maxHealth;
    public int currentHealth;
    public int currentShield;

    public Image healthBar;
    public Image shieldBar;
    public TextMeshProUGUI healthText;

    public bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupHealth(int maxHealth, int health) {
        this.maxHealth = maxHealth;
        currentHealth = health;
        currentShield = 0;
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
        if (currentShield > maxHealth) {
            currentShield = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar and health text on the UI
        // converting the (currentHealth / maxHealth) to a float of 0 to 1
        float currentHealthFill = (float)currentHealth / (float)maxHealth;
        float currentShieldFill = (float)currentShield / (float)maxHealth;
        // Gradually change the fill amount of the health bar
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealthFill, Time.deltaTime * 5f);
        shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, currentShieldFill, Time.deltaTime * 5f);
        healthText.text = currentHealth + "/" + maxHealth;

        if (currentShield > 0) {
            healthText.text += " ( + " + currentShield + " )";
        }

        // Debug.ClearDeveloperConsole();
        // Debug.Log("Health: " + currentHealth + "/" + maxHealth);
    }
    
}
