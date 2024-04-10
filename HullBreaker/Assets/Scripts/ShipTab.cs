using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipTab : MonoBehaviour
{
    public Ship ship;
    public Animator animator;
    public UiManager uiManager;
    public Image shipImage;
    public GameObject shipCharacter;

    public void AssignShip(Ship newShip)
    {
        ship = newShip;
        shipImage.sprite = ship.shipImage;
        // Set the IsActive bool in the animator to true
        animator.SetBool("IsActive", true);

        // Set shipCharacter to active and set its sprite to the ship's character
        shipCharacter.SetActive(true);
        shipCharacter.GetComponent<SpriteRenderer>().sprite = ship.shipImage;
    }

    public void OnClick()
    {
        uiManager.ChangeSelectedShips(gameObject);
    }

    void Start() {
        uiManager = FindObjectOfType<UiManager>();
    }

    void Update() {

    }
}
