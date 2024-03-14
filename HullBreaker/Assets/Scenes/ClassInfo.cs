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
        - 1.5x : Bio, Chemical
        - 0.5x : Hydro
    Bio :
        - 1.5x : Mechanical, Hydro
        - 0.5x : Thermal
    Mechanical :
        - 1.5x : Chemical, Thermal
        - 0.5x : Bio
    Chemical :
        - 1.5x : Hydro, Bio
        - 0.5x : Mechanical
    Hydro :
        - 1.5x : Thermal, Mechanical
        - 0.5x : Chemical
    */

    public enum ClassType
    {
        Thermal,
        Bio,
        Mechanical,
        Chemical,
        Hydro
    }

    public static float GetDamageMultiplier(ClassType attacker, ClassType defender)
    {
        if (attacker == ClassType.Thermal)
        {
            if (defender == ClassType.Bio || defender == ClassType.Chemical)
            {
                return 1.5f;
            }
            else if (defender == ClassType.Hydro)
            {
                return 0.5f;
            }
        }
        else if (attacker == ClassType.Bio)
        {
            if (defender == ClassType.Mechanical || defender == ClassType.Hydro)
            {
                return 1.5f;
            }
            else if (defender == ClassType.Thermal)
            {
                return 0.5f;
            }
        }
        else if (attacker == ClassType.Mechanical)
        {
            if (defender == ClassType.Chemical || defender == ClassType.Thermal)
            {
                return 1.5f;
            }
            else if (defender == ClassType.Bio)
            {
                return 0.5f;
            }
        }
        else if (attacker == ClassType.Chemical)
        {
            if (defender == ClassType.Hydro || defender == ClassType.Bio)
            {
                return 1.5f;
            }
            else if (defender == ClassType.Mechanical)
            {
                return 0.5f;
            }
        }
        else if (attacker == ClassType.Hydro)
        {
            if (defender == ClassType.Thermal || defender == ClassType.Mechanical)
            {
                return 1.5f;
            }
            else if (defender == ClassType.Chemical)
            {
                return 0.5f;
            }
        }

        return 1f;
    }
}
