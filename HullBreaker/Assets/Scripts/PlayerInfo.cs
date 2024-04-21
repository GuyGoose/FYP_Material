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
    public List<UpgradeItem> items = new List<UpgradeItem>();
    public int maxHealth;
    public int currentHealth;
    public int energy;
    public int bonusEnergy;
    public int credits;
    public bool isBossFight;
    public Encounter currentEncounter;
    public int currentDifficulty;
    public int score;
    public int bossesDefeated;
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
        credits = data.credits;
        isBossFight = data.isBossFight;
        IsEncounterBoss(data);
        currentDifficulty = data.currentDifficulty;
        score = data.score;
        bossesDefeated = data.bossesDefeated;
        playerFaction = data.playerFaction;

        // Load the player's ships
        ships = new List<Ship>();
        foreach (int i in data.shipIndexes) {
            Debug.Log("Ship index Got by Resource Loader: " + i);
            ships.Add(ResourceLoader.GetShipByIndex(i));
        }

        // Load the player's items
        items = new List<UpgradeItem>();
        // Check if the player has any items
        if (data.itemIndexes == null) {
            return;
        }
        foreach (int i in data.itemIndexes) {
            items.Add(ResourceLoader.GetItemByIndex(i));
        }
    }

    private void IsEncounterBoss(PlayerData data) {
        if (isBossFight) {
            currentEncounter = ResourceLoader.GetBossEncounterByIndex(0);
        } else {
            currentEncounter = ResourceLoader.GetEncounterByIndex(data.encounterIndex);
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

    public void SetPlayerPilot(Pilot pilot) {
        playerName = pilot.pilotName;
        maxHealth = pilot.startingHealth;
        currentHealth = pilot.startingHealth;
        energy = pilot.startingEnergy;
        credits = pilot.startingCredits;
        isBossFight = false;
        ships = new List<Ship>();
        ships.Add(pilot.startingShip);
        items = pilot.startingInventory;
        currentDifficulty = 1;
        score = 0;
        bossesDefeated = 0;

        // Save
        SavePlayerInfo();
    }

}