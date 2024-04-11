using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayItemManager : MonoBehaviour
{
    public GameObject itemDisplayUI;
    public GameObject displayItemPrefab;
    public GameObject displayItemParent;
    public List<GameObject> displayItems;
    private PlayerInfo playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        // Get each unique item in the player's inventory and display it in the item display
        UpdateItemDisplay();

        HideItemDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        // If TAB is held down, show the item display
        if (Input.GetKey(KeyCode.Tab)) {
            ShowItemDisplay();
        } else {
            HideItemDisplay();
        }
        
    }

    public void ShowItemDisplay() {
        // Turn Canvas Group to 1 and enable raycasting
        itemDisplayUI.GetComponent<CanvasGroup>().alpha = 1;
        itemDisplayUI.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideItemDisplay() {
        // Turn Canvas Group to 0 and disable raycasting
        itemDisplayUI.GetComponent<CanvasGroup>().alpha = 0;
        itemDisplayUI.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void UpdateItemDisplay() {
        foreach (GameObject displayItem in displayItems) {
            Destroy(displayItem);
        }
        displayItems.Clear();
        List<String> uniqueItems = new List<String>();
        uniqueItems.Clear();
        foreach (UpgradeItem item in playerInfo.items) {
            // If the item is not already in the list of unique items, add it
            if (!uniqueItems.Contains(item.itemName)) {
                uniqueItems.Add(item.itemName);
                // Create a new display item for the item
                GameObject newDisplayItem = Instantiate(displayItemPrefab, displayItemParent.transform);
                newDisplayItem.GetComponent<DisplayItem>().UpdateItemDisplay(item);
                displayItems.Add(newDisplayItem);
            }
            // If the item is already in the list of unique items, find its display item and increment the count
            else {
                foreach (GameObject displayItem in displayItems) {
                    if (displayItem.GetComponent<DisplayItem>().itemName == item.itemName) {
                        displayItem.GetComponent<DisplayItem>().itemNb++;
                        displayItem.GetComponent<DisplayItem>().itemNbText.text = "x" + displayItem.GetComponent<DisplayItem>().itemNb.ToString();
                    }
                }
            }
        }
    }
}
