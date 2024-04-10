using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FactionInfo 
{
    /*
    -- Faction Info --
    Contains the information for each faction in the game - Mainly used for dialogue storage and storing information relating to faction relations

    Each faction have a have specific relations with the player (ex-employee)
    */

    // Relations 
    public static Dictionary<EnumHolder.Faction, int> factionRelations = new Dictionary<EnumHolder.Faction, int> {
        {EnumHolder.Faction.Enforcers, -100},
        {EnumHolder.Faction.Merchants, 50},
        {EnumHolder.Faction.Outlaws, -50},
        {EnumHolder.Faction.Cultists, 0},
        {EnumHolder.Faction.ExEmployees, 100},	
        {EnumHolder.Faction.HullBreakers, -100}
    };
}
