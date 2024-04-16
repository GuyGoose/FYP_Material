using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipButton : MonoBehaviour
{
    public Ship ship;
    public ShopController shopController;
    public Image shipImage;
    int sellPrice;
    public TextMeshProUGUI sellPriceDisplay;
    // Start is called before the first frame update
    void Start()
    {
        shopController = GameObject.FindObjectOfType<ShopController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShip(Ship ship) {
        this.ship = ship;
        shipImage.sprite = ship.shipImage;
        sellPrice = ship.basePrice / 2;
        sellPriceDisplay.text = sellPrice + "c";
    }

    public void SellShip() {
        if (shopController.playerInfo.ships.Count <= 1) {
            Debug.Log("You can't sell your last ship!");
            SoundManager.Instance.PlaySFX("Error");
            return;
        }
        // Remove the ship from the player's list of ships
        shopController.playerInfo.ships.Remove(ship);
        // Add the price of the ship to the player's credits
        shopController.playerInfo.credits += ship.basePrice/2;
        shopController.playerInfo.score += ship.basePrice/2/4;
        // Save the player's info
        shopController.playerInfo.SavePlayerInfo();

        SoundManager.Instance.PlaySFX("Sell");

        // Destroy the shop item
        Destroy(gameObject);

        Debug.Log("Sold ship: " + ship.shipName);
    }
}
