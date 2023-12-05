using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public GameObject shipDatabaseObject;

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
}
