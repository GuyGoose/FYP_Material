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
        foreach (UpgradeItem item in playerInfo.items) {
            // Create a new display item for each item in the player's inventory, If the item is a duplicate, increment the item count
            bool isDuplicate = false;
            foreach (GameObject displayItem in displayItems) {
                if (displayItem.GetComponent<DisplayItem>().item.itemName == item.itemName) {
                    displayItem.GetComponent<DisplayItem>().itemNb++;
                    displayItem.GetComponent<DisplayItem>().itemNbText.text = "x" + displayItem.GetComponent<DisplayItem>().itemNb.ToString();
                    isDuplicate = true;
                }
            }
            if (!isDuplicate) {
                GameObject displayItem = Instantiate(displayItemPrefab, displayItemParent.transform);
                displayItem.GetComponent<DisplayItem>().UpdateItemDisplay(item);
                displayItems.Add(displayItem);
            }  
        }

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
        foreach (UpgradeItem item in playerInfo.items) {
            // Create a new display item for each item in the player's inventory, If the item is a duplicate, increment the item count
            bool isDuplicate = false;
            foreach (GameObject displayItem in displayItems) {
                if (displayItem.GetComponent<DisplayItem>().item.itemName == item.itemName) {
                    displayItem.GetComponent<DisplayItem>().itemNb++;
                    displayItem.GetComponent<DisplayItem>().itemNbText.text = "x" + displayItem.GetComponent<DisplayItem>().itemNb.ToString();
                    isDuplicate = true;
                }
            }
            if (!isDuplicate) {
                GameObject displayItem = Instantiate(displayItemPrefab, displayItemParent.transform);
                displayItem.GetComponent<DisplayItem>().UpdateItemDisplay(item);
                displayItems.Add(displayItem);
            }  
        }
    }
}
