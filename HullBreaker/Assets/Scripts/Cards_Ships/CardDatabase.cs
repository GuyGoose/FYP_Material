using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDatabase : MonoBehaviour
{
    public GameObject shipDatabaseObject;
    public GameObject displayCardObject;

    // PlayerHand Container
    public GameObject playerHandContainer;

    // TextMeshProUGUI objects
    public TextMeshProUGUI DeckSizeText;
    public TextMeshProUGUI DiscardSizeText;
    public TextMeshProUGUI VoidSizeText;

    public List<Card> cardList = new List<Card>();
    public List<Card> playerDeck = new List<Card>();
    public List<Card> playerHand = new List<Card>();
    public List<Card> playerDiscard = new List<Card>();
    public List<Card> playerVoid = new List<Card>();

    private void Awake() {
        // Add all card scriptable objects contained in the Cards folder to the cardList
        Card[] cards = Resources.LoadAll<Card>("Cards");
        // Print the number of cards in the cardList
        Debug.Log("Number of cards in cardList: " + cards.Length);
        foreach (Card card in cards) {
            cardList.Add(card);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ShuffleDeck();
        DrawCard(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard(int numberOfCards) {
        // Draw the specified number of cards from the playerDeck
        for (int i = 0; i < numberOfCards; i++) {
            // If the playerDeck is empty, shuffle the playerDiscard into the playerDeck
            if (playerDeck.Count == 0) {
                ShuffleDiscardIntoDeck();
            }
            // If the playerDeck is still empty, break out of the loop
            if (playerDeck.Count == 0) {
                break;
            }
            // Draw a card from the playerDeck
            Card card = playerDeck[0];
            // Add the card to the playerHand
            playerHand.Add(card);
            // Remove the card from the playerDeck
            playerDeck.Remove(card);
            // Instantiate the card in the playerHandContainer
            GameObject cardObject = Instantiate(displayCardObject, playerHandContainer.transform);
            // Set the cardObject's card to the drawn card
            cardObject.GetComponent<DisplayCard>().card = card;

            // Update the DeckSizeText
            DeckSizeText.text = "Deck: " + playerDeck.Count;

            // Order the cards in the playerHand
            OrderCardsInHand();

            Debug.Log("Card drawn: " + card.name);
        }
        
    }

    public void DiscardCard(Card card) {
        // Add the card to the playerDiscard
        playerDiscard.Add(card);
        // Remove the card from the playerHand
        playerHand.Remove(card);
    }

    public void DiscardHand() {
        // Add all cards in the playerHand to the playerDiscard
        foreach (Card card in playerHand) {
            playerDiscard.Add(card);
        }
        // Remove all cards from the playerHand
        playerHand.Clear();
    }

    public void ShuffleDeck() {
        // Shuffle the playerDeck
        for (int i = 0; i < playerDeck.Count; i++) {
            Card temp = playerDeck[i];
            int randomIndex = Random.Range(i, playerDeck.Count);
            playerDeck[i] = playerDeck[randomIndex];
            playerDeck[randomIndex] = temp;
        }
    }

    public void ShuffleDiscardIntoDeck() {
        // Add all cards in the playerDiscard to the playerDeck
        foreach (Card card in playerDiscard) {
            playerDeck.Add(card);
        }
        // Remove all cards from the playerDiscard
        playerDiscard.Clear();
        // Shuffle the playerDeck
        ShuffleDeck();
    }

    public void OrderCardsInHand() {
        // Display the cards in the playerHand from left to right at equal intervals in the playerHandContainer
        // Get the number of cards in the playerHand
        int numberOfCards = playerHand.Count;
        // Get the width of the playerHandContainer
        float containerWidth = playerHandContainer.GetComponent<RectTransform>().rect.width;
        // Get the width of the cardObject
        float cardWidth = displayCardObject.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < numberOfCards; i++) {
            // Get the x position of the cardObject
            float xPos = (containerWidth / (numberOfCards + 1)) * (i + 1);
            // Get the y position of the cardObject
            float yPos = 0;
            // Get the z position of the cardObject
            float zPos = 0;
            // Set the position of the cardObject from the left of the playerHandContainer not the center
            Vector3 cardPosition = new Vector3(xPos - (containerWidth / 2), yPos, zPos);
            // Set the position of the cardObject
            playerHandContainer.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = cardPosition;
        }
    }
}
