using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassInfo
{
    /*
    -- Class Info --
    Contains the information for each class typing in the game (Enum and damage multipliers)

    -- Class Types --
    - Thermal
    - Bio 
    - Mechanical
    - Chemical
    - Hydro

    -- Class Matchups --
    Thermal :
        - + 10% : Bio, Chemical
        - - 10% : Hydro
    Bio :
        - + 10% : Mechanical, Hydro
        - - 10% : Thermal
    Mechanical :
        - + 10% : Chemical, Thermal
        - - 10% : Bio
    Chemical :
        - + 10% : Hydro, Bio
        - - 10% : Mechanical
    Hydro :
        - + 10% : Thermal, Mechanical
        - - 10% : Chemical
    */

    public enum ClassType
    {
        Thermal,
        Bio,
        Mechanical,
        Chemical,
        Hydro
    }

    public static int GetDamageMultiplier(int valueBeforeMult, ClassType actionType, List<Ship> ships)
    {
        // Get the type of each ship and apply the damage multiplier to the value depending on the type
        foreach (Ship ship in ships)
        {
            bool isMatchup = CompareTypeMatchup(actionType, ship.classType);
            if (isMatchup)
            {
                valueBeforeMult = (int)(valueBeforeMult * 1.1f);
            }
            else
            {
                valueBeforeMult = (int)(valueBeforeMult * 0.9f);
            }
        }

        return valueBeforeMult;
    }

    public static bool CompareTypeMatchup(ClassType attacker, ClassType defender)
    {
        // Compare the attacker and defender types to see if the attacker has an advantage
        if (attacker == ClassType.Thermal)
        {
            if (defender == ClassType.Bio || defender == ClassType.Chemical)
            {
                return true;
            }
            else if (defender == ClassType.Hydro)
            {
                return false;
            }
        }
        else if (attacker == ClassType.Bio)
        {
            if (defender == ClassType.Mechanical || defender == ClassType.Hydro)
            {
                return true;
            }
            else if (defender == ClassType.Thermal)
            {
                return false;
            }
        }
        else if (attacker == ClassType.Mechanical)
        {
            if (defender == ClassType.Chemical || defender == ClassType.Thermal)
            {
                return true;
            }
            else if (defender == ClassType.Bio)
            {
                return false;
            }
        }
        else if (attacker == ClassType.Chemical)
        {
            if (defender == ClassType.Hydro || defender == ClassType.Bio)
            {
                return true;
            }
            else if (defender == ClassType.Mechanical)
            {
                return false;
            }
        }
        else if (attacker == ClassType.Hydro)
        {
            if (defender == ClassType.Thermal || defender == ClassType.Mechanical)
            {
                return true;
            }
            else if (defender == ClassType.Chemical)
            {
                return false;
            }
        }
        return false;
    }
}
