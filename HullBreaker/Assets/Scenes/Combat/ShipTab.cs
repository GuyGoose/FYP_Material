using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipTab : MonoBehaviour
{
    public Ship ship;
    public Animator animator;
    public ActionManager actionManager;
    public UiManager uiManager;
    public Image shipImage;

    public void AssignShip(Ship newShip)
    {
        ship = newShip;
        shipImage.sprite = ship.shipImage;
        // Set the IsActive bool in the animator to true
        animator.SetBool("IsActive", true);
    }

    public void OnSelect() {
        actionManager.UpdateCurrentShip(ship);
        uiManager.ChangeSelectedShips(gameObject);
        Debug.Log("Selected " + ship.shipName);
    }

    void Start() {
        actionManager = FindObjectOfType<ActionManager>();
        uiManager = FindObjectOfType<UiManager>();
    }

    void Update() {
        if (ship != null)
        {
            Debug.Log(ship.shipName);
        }
    }
}
