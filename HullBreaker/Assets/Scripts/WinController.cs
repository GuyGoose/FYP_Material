using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class WinController : MonoBehaviour
{
    private PlayerInfo playerInfo;
    public Image pilotImage;
    public TextMeshProUGUI pilotName;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI totalUpgrades;
    public TextMeshProUGUI bossKills;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();

        pilotName.text = playerInfo.playerName;
        finalScore.text = "Final Score: " + playerInfo.score;
        totalUpgrades.text = "Total Upgrades: " + playerInfo.items.Count;
        bossKills.text = "Boss Kills: " + playerInfo.bossesDefeated;

        Sprite pilotSprite = ResourceLoader.LoadPilotSprite(playerInfo.playerName);
        pilotImage.sprite = pilotSprite;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu() {
        DeleteMap();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ContinueGame() {
        SceneManager.LoadScene("MapScene");
    }

        public void DeleteMap()
    {
        string filePath = Application.persistentDataPath + "/mapSave.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Map deleted.");
        }
        else
        {
            Debug.LogWarning("No saved map found.");
        }
    }

}
