using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public Ship ship;
    public int price;
    public TextMeshProUGUI shipPrice;
    public Image shipImage;
    private PlayerInfo playerInfo;
    private ShopController shopController;

    void Awake() {
         playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        shopController = GameObject.FindObjectOfType<ShopController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShip(Ship ship) {
        this.ship = ship;
        shipImage.sprite = ship.shipImage;
        int multNum = playerInfo.currentDifficulty;
        price = ship.basePrice + Random.Range(0, multNum * 50);
        shipPrice.text = price + "c";
    }

    public void BuyShip() {
        // Check if the player has enough credits
        if (playerInfo.credits >= price) {
            if (playerInfo.ships.Count >= 4) {
                Debug.Log("You already have 4 ships! Sell a ship to make room for this one.");
                shopController.DisplaySellMenu();
                return;
            }
            // Add the ship to the player's list of ships
            playerInfo.ships.Add(ship);
            // Subtract the price of the ship from the player's credits
            playerInfo.credits -= price;
            // Save the player's info
            playerInfo.SavePlayerInfo();

            SoundManager.Instance.PlaySFX("Buy");

            // Destroy the shop item
            Destroy(gameObject);
    
            Debug.Log("Bought ship: " + ship.shipName);
        } else {
            Debug.Log("Not enough credits to buy this ship!");
            SoundManager.Instance.PlaySFX("Error");
        }
    }
}
