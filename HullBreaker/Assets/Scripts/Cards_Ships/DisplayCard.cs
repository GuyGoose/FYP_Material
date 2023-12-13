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

    // Properties
    public bool selected = false;
    public bool notSelected = true;

    // Start is called before the first frame update
    void Start()
    {
        DisplayCardInfo();
    }

    // Update is called once per frame
    void Update()
    {
        // Card Hover
        // If the mouse is over the card, increase the card's size, otherwise, return it to normal size
        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition)) {
            GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            // Bring the card to the front of the canvas
            GetComponent<RectTransform>().SetAsLastSibling();
        } else {
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            // Return the card to its original position in the canvas
            GetComponent<RectTransform>().SetAsFirstSibling();
        }

        // Card Selection
        // If THIS card is clicked, select it, otherwise, deselect it
    
        
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

    public void SelectCard() {
        // Select the card, or deselect it if it's already selected
        // If selected, move the card up, increase its size and bring it to the front of the canvas (set all other cards to not selected and move them down and decrease their size)
        if (selected) {
            DeselectCard();
        } else {
            selected = true;
            notSelected = false;
            GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            // Bring the card to the front of the canvas
            GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

    public void DeselectCard() {
        // Deselect the card, or select it if it's already deselected
        // If deselected, move the card down, decrease its size and return it to its original position in the canvas (set all other cards to selected and move them up and increase their size)
        if (notSelected) {
            SelectCard();
        } else {
            selected = false;
            notSelected = true;
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            // Return the card to its original position in the canvas
            GetComponent<RectTransform>().SetAsFirstSibling();
        }
        
    }

}
