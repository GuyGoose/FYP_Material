using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    public static Encounter GetEncounterByIndex(int index) {

        Encounter[] encounters = Resources.LoadAll<Encounter>("Encounters");
        return encounters[index];
        
    }

    public static Encounter GetBossEncounterByIndex(int index) {
            
        Encounter[] encounters = Resources.LoadAll<Encounter>("BossEncounters");
        return encounters[index];

    }

    public static Ship GetShipByIndex(int index) {

        Ship[] ships = Resources.LoadAll<Ship>("Ships");
        return ships[index];
        
    }

    public static Ship GetRandomShip() {

        Ship[] ships = Resources.LoadAll<Ship>("Ships");
        return ships[Random.Range(0, ships.Length)];
        
    }

    public static Encounter GetRandomBossEncounterByDifficulty(int difficulty) {

        Encounter[] encounters = Resources.LoadAll<Encounter>("BossEncounters");
        List<Encounter> bossEncounters = new List<Encounter>();

        foreach (Encounter encounter in encounters) {
            if (encounter.difficulty == difficulty) {
                bossEncounters.Add(encounter);
            }
        }

        return bossEncounters[Random.Range(0, bossEncounters.Count)];
        
    }

    public static UpgradeItem GetItemByIndex(int index) {

        UpgradeItem[] items = Resources.LoadAll<UpgradeItem>("Items");
        return items[index];
        
    }

    public static UpgradeItem GetRandomItem() {

        UpgradeItem[] items = Resources.LoadAll<UpgradeItem>("Items");
        return items[Random.Range(0, items.Length)];
        
    }

    public static string CreateDescriptionOfShipActions(Ship ship) {

        string description = "";

        foreach (Action action in ship.actions) {
            description += action.actionName + " - " + CreateDescriptionOfAction(action) + "\n";
        }

        return description;
        
    }

    public static string CreateDescriptionOfAction(Action action) {

        string description = "";

        switch (action.actionType.ToString()) {
            case "Damage":
                if (string.IsNullOrEmpty(action.valueToAdd)) {
                    description = "Deal " + action.numberOfDice + "d" + action.numberOfSides + " damage";
                } else {
                    description = "Deal " + action.numberOfDice + "d" + action.numberOfSides + " + " + action.valueToAdd + " damage";
                }
                break;
            case "Heal":
                if (string.IsNullOrEmpty(action.valueToAdd)) {
                    description = "Heal " + action.numberOfDice + "d" + action.numberOfSides + " health";
                } else {
                    description = "Heal " + action.numberOfDice + "d" + action.numberOfSides + " + " + action.valueToAdd + " health";
                }
                break;
            case "Shield":
                if (string.IsNullOrEmpty(action.valueToAdd)) {
                    description = "Shield " + action.numberOfDice + "d" + action.numberOfSides + " health";
                } else {
                    description = "Shield " + action.numberOfDice + "d" + action.numberOfSides + " + " + action.valueToAdd + " health";
                }
                break;
            default:
                Debug.Log("Invalid action type");
                break;
        }

        // If the action has a status effect, add it to the description
        // Effects: Waterlogged, Burning, Regeneration, Virus, Reinforced, Improved, Accuracy
        switch (action.statusEffect.ToString()) {
            case "None":
                break;
            case "Waterlogged":
                description += " and inflict " + action.statusAmount + " Waterlogged";
                break;
            case "Burning":
                description += " and inflict " + action.statusAmount + " Burning";
                break;
            case "Regeneration":
                description += " and gain " + action.statusAmount + " Regeneration";
                break;
            case "Virus":
                description += " and inflict " + action.statusAmount + " Virus";
                break;
            case "Reinforced":
                description += " and gain " + action.statusAmount + " Reinforced";
                break;
            case "Improved":
                description += " and gain " + action.statusAmount + " Improved";
                break;
            case "Accuracy":
                description += " and gain " + action.statusAmount + " Accuracy";
                break;
            default:
                Debug.Log("Invalid status effect");
                break;
        }

        return description;
    }

    public static int DynamicDifficultyCalculateScoreBonus(int playerScore, int value) {

        // Take the player's and return a multiplier based on the player's score (100 = 1.1 multiplier, 1000 = 2.0 multiplier etc.)
        float scoreMult = 1 + (playerScore / 100) * 0.1f;

        // Return the value multiplied by the score multiplier rounded to the nearest integer
        return (int)Mathf.Round(value * scoreMult);
        
    }
}