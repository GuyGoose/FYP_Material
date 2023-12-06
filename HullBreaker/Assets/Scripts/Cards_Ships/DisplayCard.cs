using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCard : MonoBehaviour
{
    // Takes in a card and displays it on the screen via the attached UI components
    public Card card;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardCost;

    public GameObject cardBack;

    // Start is called before the first frame update
    void Start()
    {
        DisplayCardInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCardInfo() {
        // Display the card's name, description, and cost
        cardName.text = card.cardName;
        
        // If the card has a custom description, display it
        if (card.customDescription) {
            cardDescription.text = card.cardDescription;
        }

        cardCost.text = card.cost.ToString();
    }

    public void FlipCard() {
        // Flip the card (over or back)
        if (cardBack.activeSelf) {
            cardBack.SetActive(false);
        } else {
            cardBack.SetActive(true);
        }
    }

}
