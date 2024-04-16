using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public List<Ship> shipsForSale = new List<Ship>();
    public List<Ship> playerShips = new List<Ship>();
    public List<ShopItem> shopItems = new List<ShopItem>();

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerCredits;
    public TextMeshProUGUI playerHealth;

    public GameObject sellMenu;
    public GameObject shipsToSellPanel;
    public GameObject shipButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        playerShips = playerInfo.ships;

        playerName.text = playerInfo.playerName;

        HideSellMenu();

        // Set the shop items (Random ships from the resource loader)
        foreach (ShopItem shopItem in shopItems) {
            Ship thisShip = ResourceLoader.GetRandomShip();
            shopItem.SetShip(thisShip);
        }

        // Play music
        SoundManager.Instance.PlayMusic("Shop");
    }

    // Update is called once per frame
    void Update()
    {
        // Set the player's name, credits, and health
        playerHealth.text = "HP: " + playerInfo.currentHealth + "/" + playerInfo.maxHealth;
        playerCredits.text = "Credits: " + playerInfo.credits + "c";
    }

    public void DisplaySellMenu() {
        // Clear the ships to sell panel
        foreach (Transform child in shipsToSellPanel.transform) {
            Destroy(child.gameObject);
        }

        // Add the player's ships to the ships to sell panel
        foreach (Ship ship in playerShips) {
            GameObject newButton = Instantiate(shipButtonPrefab, shipsToSellPanel.transform);
            newButton.GetComponent<ShipButton>().SetShip(ship);
        }

        sellMenu.SetActive(true);

        SoundManager.Instance.PlaySFX("UI_In");
    }

    public void HideSellMenu() {
        sellMenu.SetActive(false);
        SoundManager.Instance.PlaySFX("UI_Out");
    }

    public void ReturnToMap() {
        playerInfo.SavePlayerInfo();
        SoundManager.Instance.PlaySFX("UI_Out");
        SceneManager.LoadScene("MapScene");
    }
    
}
