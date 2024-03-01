using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Contains the information regarding the player
    /*
    -- Player Info --
    - Player Name
    - Current Planet
    - Ships
    - Current Health
    - Energy
    */

    public string playerName;
    public string currentPlanet;
    public List<Ship> ships = new List<Ship>();
    public int currentHealth;
    public int energy;

    // Start is called before the first frame update
    private void Start() {
        
    }
}
