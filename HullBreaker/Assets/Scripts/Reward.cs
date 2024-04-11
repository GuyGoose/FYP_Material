using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Reward : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject reward;
    public string rewardName;
    public string rewardDescription;
    public Image rewardImage;

    // -- ToolTip --
    private ToolTipMessenger toolTipMessenger;
    public float toolTipDelay = 0.5f;
    float timer;
    bool hasMouse;

    // Start is called before the first frame update
    void Start()
    {
        toolTipMessenger = GameObject.Find("ToolTip").GetComponent<ToolTipMessenger>();
        hasMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the reward is a Ship or a UpgradeItem
        if (reward.GetComponent<Ship>() != null) {
            rewardName = reward.GetComponent<Ship>().shipName;
            //rewardDescription = reward.GetComponent<Ship>().shipDescription;
            rewardImage.sprite = reward.GetComponent<Ship>().shipImage;
        } else if (reward.GetComponent<UpgradeItem>() != null) {
            rewardName = reward.GetComponent<UpgradeItem>().itemName;
            rewardDescription = ProcessItemsInCombat.CreateItemDescription(reward.GetComponent<UpgradeItem>());
            rewardImage.sprite = reward.GetComponent<UpgradeItem>().itemImage;
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

    public void UpdateReward(GameObject reward) {
        this.reward = reward;
    }
}
