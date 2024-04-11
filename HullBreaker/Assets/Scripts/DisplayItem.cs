using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeItem item;
    public Image itemImage;
    public string itemName;
    public TextMeshProUGUI itemNameText;
    public string itemDescription;
    public int itemValue;
    public int itemNb;
    public TextMeshProUGUI itemNbText;

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
        if (hasMouse && timer < toolTipDelay) {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay) {
                toolTipMessenger.Show(itemName, itemDescription);
            }
        }
    }

    public void UpdateItemDisplay(UpgradeItem item)
    {
        itemImage.sprite = item.itemImage;
        itemName = item.itemName;
        itemNameText.text = itemName;
        itemDescription = ProcessItemsInCombat.CreateItemDescription(item);
        itemValue = item.basePrice;
        itemNb = 1;
        itemNbText.text = "x" + itemNb.ToString();
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
