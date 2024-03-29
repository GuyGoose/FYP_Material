using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object for the Pilot class (Player Character)

/*
-- Pilot --
- Pilot Name
- Pilot Info
- Pilot Image
- Starting Ship
- Starting Health
- Starting Energy
- Starting Credits
- Starting Inventory -- TODO when items are implemented
*/

[CreateAssetMenu(fileName = "New Pilot", menuName = "Pilot")]

public class Pilot : ScriptableObject
{
    public string pilotName;
    public string pilotInfo;
    public Sprite pilotImage;
    public Ship startingShip;
    public int startingHealth;
    public int startingEnergy;
    public int startingCredits;
}