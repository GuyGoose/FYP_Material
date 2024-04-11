using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    // Save the player's information to a file and store it in a JSON format

    /*

    -- Player Info --
    For saving the players information between scenes

    public string playerName;
    public string currentPlanet;
    public List<Ship> ships = new List<Ship>();
    public int maxHealth;
    public int currentHealth;
    public int energy;
    public Encounter currentEncounter;
    public int currentDifficulty;
    public EnumHolder.Faction playerFaction;

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
        currentEncounter = data.currentEncounter;
        currentDifficulty = data.currentDifficulty;
        playerFaction = data.playerFaction;
    }
    */

    public static void SavePlayer(PlayerInfo player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        data.playerName = player.playerName;
        data.currentPlanet = player.currentPlanet;
        data.maxHealth = player.maxHealth;
        data.currentHealth = player.currentHealth;
        data.energy = player.energy;
        data.credits = player.credits;
        data.isBossFight = player.isBossFight;
        data.encounterIndex = player.currentEncounter.encounterIndex;
        data.currentDifficulty = player.currentDifficulty;
        data.playerFaction = player.playerFaction;

        data.shipIndexes.Clear();
        foreach (Ship s in player.ships) {
            data.shipIndexes.Add(s.shipIndex);
        }

        data.itemIndexes.Clear();
        foreach (UpgradeItem i in player.items) {
            data.itemIndexes.Add(i.itemIndex);
        }

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Player info saved to: " + path);
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            Debug.Log("Player info loaded from: " + path);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    
    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Player info deleted from: " + path);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    
}

[Serializable]
public class PlayerData
{
    public string playerName;
    public string currentPlanet;
    public List<int> shipIndexes = new List<int>();
    public List<int> itemIndexes = new List<int>();
    public int maxHealth;
    public int currentHealth;
    public int energy;
    public int credits;
    public bool isBossFight;
    public int encounterIndex;
    public int currentDifficulty;
    public EnumHolder.Faction playerFaction;
}

