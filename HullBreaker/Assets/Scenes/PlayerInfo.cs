using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    - Current Encounter
    */

    public string playerName;
    public string currentPlanet;
    public List<Ship> ships = new List<Ship>();
    public int maxHealth;
    public int currentHealth;
    public int energy;
    public Encounter currentEncounter;
    public int currentDifficulty;
    public EnumHolder.Faction playerFaction;

    private void Awake() {
        LoadPlayerInfo();
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    public void SavePlayerInfo() {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayerInfo() {
        PlayerData data = SaveSystem.LoadPlayer();

        playerName = data.playerName;
        currentPlanet = data.currentPlanet;
        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;
        energy = data.energy;
        currentEncounter = ResourceLoader.GetEncounterByIndex(data.encounterIndex);
        currentDifficulty = data.currentDifficulty;
        playerFaction = data.playerFaction;

        // Load the player's ships
        ships = new List<Ship>();
        foreach (int i in data.shipIndexes) {
            ships.Add(ResourceLoader.GetShipByIndex(i));
        }
    }

    // Debugging
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeScene();
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            SavePlayerInfo();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            LoadPlayerInfo();
        }
    }
    public void ChangeScene() {
        SavePlayerInfo();
        UnityEngine.SceneManagement.SceneManager.LoadScene("CombatScene");
    }

}