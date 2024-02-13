using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUnitInfo : MonoBehaviour
{
    public string shipName;

    public int maxHealth;
    public int currentHealth;
    public int currentShield;
    
    void Start()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                currentHealth += currentShield;
                currentShield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void GainShield(int amount)
    {
        currentShield += amount;
    }
    
}
