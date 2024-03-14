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
}