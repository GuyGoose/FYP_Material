using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProcessItemsInCombat
{
    public static void ProcessPassiveItems(PlayerInfo playerInfo, GameObject enemyInfo, GameObject playerInfoHealth)
    {
        foreach (UpgradeItem item in playerInfo.items)
        {
            if (item.trigger == Trigger.Passive)
            {
                Debug.Log("Processing passive item: " + item.itemName);
                switch (item.effect)
                {
                    case Effect.Heal:
                        playerInfoHealth.GetComponent<Health>().Heal(item.value);
                        break;
                    case Effect.Damage:
                        enemyInfo.GetComponent<Health>().TakeDamage(item.value);
                        break;
                    case Effect.Shield:
                        playerInfoHealth.GetComponent<Health>().Shield(item.value);
                        break;
                    case Effect.Energy:
                        playerInfo.bonusEnergy += item.value;
                        break;
                    case Effect.StatusEffect:
                        // Get the target of the status effect
                        if (item.target == Target.Self)
                        {
                            playerInfo.GetComponent<StatusEffect>().ApplyStatusEffect(item.statusEffect, item.value);
                        }
                        else if (item.target == Target.Enemy)
                        {
                            enemyInfo.GetComponent<StatusEffect>().ApplyStatusEffect(item.statusEffect, item.value);
                        }
                        break;
                }
            }
        }
    }

    public static void ProcessStartOfPlayerTurnItems(PlayerInfo playerInfo, GameObject enemyInfo, GameObject playerInfoHealth)
    {
        foreach (UpgradeItem item in playerInfo.items)
        {
            if (item.trigger == Trigger.OnStartOfPlayerTurn)
            {
                Debug.Log("Processing start of player turn item: " + item.itemName);
                switch (item.effect)
                {
                    case Effect.Heal:
                        playerInfoHealth.GetComponent<Health>().Heal(item.value);
                        break;
                    case Effect.Damage:
                        enemyInfo.GetComponent<Health>().TakeDamage(item.value);
                        break;
                    case Effect.Shield:
                        playerInfoHealth.GetComponent<Health>().Shield(item.value);
                        break;
                    case Effect.Energy:
                        playerInfo.bonusEnergy += item.value;
                        break;
                    case Effect.StatusEffect:
                        // Get the target of the status effect
                        if (item.target == Target.Self)
                        {
                            playerInfo.GetComponent<StatusEffect>().ApplyStatusEffect(item.statusEffect, item.value);
                        }
                        else if (item.target == Target.Enemy)
                        {
                            enemyInfo.GetComponent<StatusEffect>().ApplyStatusEffect(item.statusEffect, item.value);
                        }
                        break;
                }
            }
        }
    }

    public static string CreateItemDescription(UpgradeItem item)
    {
        string itemDescription = "";
        switch (item.trigger)
        {
            case Trigger.OnStartOfPlayerTurn:
                itemDescription = "At the start of your turn: ";
                break;
            case Trigger.Passive:
                itemDescription = "At the start of combat: ";
                break;
        }

        switch (item.effect)
        {
            case Effect.Heal:
                itemDescription += "Heal " + item.value + " health";
                break;
            case Effect.Damage:
                itemDescription += "Deal " + item.value + " damage";
                break;
            case Effect.Shield:
                itemDescription += "Shield " + item.value + " health";
                break;
            case Effect.Energy:
                itemDescription += "Gain " + item.value + " energy";
                break;
            case Effect.StatusEffect:
                itemDescription += "Apply " + item.value + " " + item.statusEffect.ToString() + " to ";
                if (item.target == Target.Self)
                {
                    itemDescription += "yourself";
                }
                else if (item.target == Target.Enemy)
                {
                    itemDescription += "the enemy";
                }
                break;
        }
        return itemDescription;
    }

}
