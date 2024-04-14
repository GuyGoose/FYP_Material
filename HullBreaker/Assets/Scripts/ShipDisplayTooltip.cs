using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShipDisplayTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTipMessenger toolTipMessenger;
    public float toolTipDelay = 0.5f;
    float timer;
    bool hasMouse;
    
    public Ship ship;

    public void AssignShip(Ship newShip)
    {
        ship = newShip;
    }

    void Start() {
        toolTipMessenger = GameObject.Find("ToolTip").GetComponent<ToolTipMessenger>();
        hasMouse = false;
    }

    void Update() {
        if (hasMouse) {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay) {
                toolTipMessenger.Show(ship.shipName, ResourceLoader.CreateDescriptionOfShipActions(ship));
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        timer = 0;
        hasMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        hasMouse = false;
        toolTipMessenger.Hide();
    }
}
