using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Reward : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Ship rewardShip;
    public UpgradeItem rewardItem;
    public string rewardName;
    public string rewardDescription;
    public Image rewardImage;

    // -- ToolTip --
    private ToolTipMessenger toolTipMessenger;
    public float toolTipDelay = 0.5f;
    float timer;
    bool hasMouse;

    // Start is called before the first frame update
    void Start() {
        toolTipMessenger = GameObject.Find("ToolTip").GetComponent<ToolTipMessenger>();
        hasMouse = false;
    }

    // Update is called once per frame
    void Update() {
        // Check if the reward is a Ship or a UpgradeItem
        if (rewardShip != null) {
            rewardImage.sprite = rewardShip.shipImage;
            rewardName = rewardShip.shipName;
            //rewardDescription = rewardShip.shipDescription;
        } else if (rewardItem != null) {
            rewardImage.sprite = rewardItem.itemImage;
            rewardName = rewardItem.itemName;
            rewardDescription = ProcessItemsInCombat.CreateItemDescription(rewardItem);
        }

        if (hasMouse && timer < toolTipDelay) {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay) {
                toolTipMessenger.Show(rewardName, rewardDescription);
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

    public void UpdateReward() {
        // Clear rewardShip and rewardItem
        rewardShip = null;
        rewardItem = null;
        // 30% chance to get a ship, 70% chance to get an upgrade item
        if (Random.Range(0, 100) < 30) {
            rewardShip = ResourceLoader.GetRandomShip();
        } else {
            rewardItem = ResourceLoader.GetRandomItem();
        }
    }

}
