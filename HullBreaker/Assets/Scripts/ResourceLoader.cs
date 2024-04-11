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

    // public static GameObject GetRandomReward() {
    //     // 30% chance of getting an ship, 70% chance of getting an item
    //     if (Random.Range(0, 10) < 3) {
    //         return GetRandomShip().shipImage.gameObject;
        
    // }
}