using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // The player's ships
    public List<Ship> playerShips = new List<Ship>();
    // The enemy's ships
    public List<Ship> enemyShips = new List<Ship>();

    // The Deck and ShipDatabase objects
    public GameObject cardDatabaseObject;
    public GameObject shipDatabaseObject;

    // On Play Function (Called when the player plays a card)
    public void OnPlay(Card card) {

        // Get the card's actions
        GameObject[] cardActions = card.cardActions;

        // For each action in the card's actions
        foreach (GameObject cardAction in cardActions) {
            // Get the card action's script
            CardAction cardActionScript = cardAction.GetComponent<CardAction>();
            // Get the card action's target
            ActionTarget actionTarget = cardActionScript.actionTarget;
            // Get the card action's function
            ActionFunction actionFunction = cardActionScript.actionFunction;
            // Get the card action's value
            int actionValue = cardActionScript.actionValue;
            // Get the card action's type
            ActionType actionType = cardActionScript.actionType;

            // Target holder
            List<Ship> targetShips = new List<Ship>();
            // Clear the target ships (just in case)
            targetShips.Clear();
            
            // Get the target ships
            switch (actionTarget) {
                case ActionTarget.Self:
                    targetShips = playerShips;
                    break;
                case ActionTarget.Enemy:
                    targetShips = enemyShips;
                    break;
                case ActionTarget.AllEnemies:
                    targetShips = enemyShips;
                    break;
                case ActionTarget.AllAllies:
                    targetShips = playerShips;
                    break;
                case ActionTarget.AllShips:
                    targetShips = playerShips;
                    targetShips.AddRange(enemyShips);
                    break;
            }

            // Get the function
            // switch (actionFunction) {
            //     case ActionFunction.Damage:
            //         // Damage the target ships
            //         DamageShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Vamp_Damage:
            //         // Damage the target ships and heal the player's ships
            //         VampDamageShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Cost_Damage:
            //         // Damage the target ships based on the card's cost
            //         CostDamageShips(targetShips, actionValue, card.cost);
            //         break;
            //     case ActionFunction.Heal:
            //         // Heal the target ships
            //         HealShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Draw:
            //         // Draw the specified number of cards
            //         DrawCards(actionValue);
            //         break;
            //     case ActionFunction.Discard:
            //         // Discard the specified number of cards
            //         DiscardCards(actionValue);
            //         break;
            //     case ActionFunction.Destroy:
            //         // Destroy the target ships
            //         DestroyShips(targetShips);
            //         break;
            //     case ActionFunction.Repair:
            //         // Repair the target ships
            //         RepairShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Buff:
            //         // Buff the target ships
            //         BuffShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Debuff:
            //         // Debuff the target ships
            //         DebuffShips(targetShips, actionValue);
            //         break;
            //     case ActionFunction.Stun:
            //         // Stun the target ships
            //         StunShips(targetShips);
            //         break;
            // }

        }
    }

    // // Damage the target ships
    // public void DamageShips(List<Ship> targetShips, int damage) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Damage the ship
    //         ship.Damage(damage);
    //     }
    // }

    // // Damage the target ships and heal the player's ships
    // public void VampDamageShips(List<Ship> targetShips, int damage) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Damage the ship
    //         ship.Damage(damage);
    //     }
    //     // For each ship in the player's ships
    //     foreach (Ship ship in playerShips) {
    //         // Heal the ship
    //         ship.Heal(damage);
    //     }
    // }

    // // Damage the target ships based on the card's cost
    // public void CostDamageShips(List<Ship> targetShips, int damage, int cost) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Damage the ship
    //         ship.Damage(damage * cost);
    //     }
    // }

    // // Heal the target ships
    // public void HealShips(List<Ship> targetShips, int heal) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Heal the ship
    //         ship.Heal(heal);
    //     }
    // }

    // // Draw the specified number of cards
    // public void DrawCards(int numberOfCards) {
    //     // Draw the specified number of cards from the playerDeck
    //     for (int i = 0; i < numberOfCards; i++) {
    //         // If the playerDeck is empty, shuffle the playerDiscard into the playerDeck
    //         if (cardDatabaseObject.GetComponent<CardDatabase>().playerDeck.Count == 0) {
    //             cardDatabaseObject.GetComponent<CardDatabase>().ShuffleDiscardIntoDeck();
    //         }
    //         // If the playerDeck is still empty, break out of the loop
    //         if (cardDatabaseObject.GetComponent<CardDatabase>().playerDeck.Count == 0) {
    //             break;
    //         }
    //         // Draw a card from the playerDeck
    //         Card card = cardDatabaseObject.GetComponent<CardDatabase>().playerDeck[0];
    //         // Add the card to the playerHand
    //         cardDatabaseObject.GetComponent<CardDatabase>().playerHand.Add(card);
    //         // Remove the card from the playerDeck
    //         cardDatabaseObject.GetComponent<CardDatabase>().playerDeck.Remove(card);
    //     }
    // }

    // // Discard the specified number of cards
    // public void DiscardCards(int numberOfCards) {
    //     // Discard the specified number of cards from the playerHand
    //     for (int i = 0; i < numberOfCards; i++) {
    //         // If the playerHand is empty, break out of the loop
    //         if (cardDatabaseObject.GetComponent<CardDatabase>().playerHand.Count == 0) {
    //             break;
    //         }
    //         // Discard a card from the playerHand
    //         Card card = cardDatabaseObject.GetComponent<CardDatabase>().playerHand[0];
    //         // Add the card to the playerDiscard
    //         cardDatabaseObject.GetComponent<CardDatabase>().playerDiscard.Add(card);
    //         // Remove the card from the playerHand
    //         cardDatabaseObject.GetComponent<CardDatabase>().playerHand.Remove(card);
    //     }
    // }

    // // Destroy the target ships
    // public void DestroyShips(List<Ship> targetShips) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Destroy the ship
    //         ship.Destroy();
    //     }
    // }

    // // Repair the target ships
    // public void RepairShips(List<Ship> targetShips, int repair) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Repair the ship
    //         ship.Repair(repair);
    //     }
    // }

    // // Buff the target ships
    // public void BuffShips(List<Ship> targetShips, int buff) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Buff the ship
    //         ship.Buff(buff);
    //     }
    // }

    // // Debuff the target ships
    // public void DebuffShips(List<Ship> targetShips, int debuff) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Debuff the ship
    //         ship.Debuff(debuff);
    //     }
    // }

    // // Stun the target ships
    // public void StunShips(List<Ship> targetShips) {
    //     // For each ship in the target ships
    //     foreach (Ship ship in targetShips) {
    //         // Stun the ship
    //         ship.Stun();
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
}
