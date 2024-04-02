using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    public static Encounter GetEncounterByIndex(int index) {

        Encounter[] encounters = Resources.LoadAll<Encounter>("Encounters");
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
}