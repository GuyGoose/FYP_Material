using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumHolder
{
    public enum Faction
    {
        Enforcers,
        Merchants,
        Outlaws,
        Cultists,
        ExEmployees,
        HullBreakers
    }

    public enum AI
    {
        Random,
        Aggressive,
        Defensive,
        Balanced,
        Progressive
    }

    public enum PlanetStatus
    {
        Hostile,
        Neutral,
        Friendly
    }
}
