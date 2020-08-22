﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private int cardCount = 0;
    readonly int maxCards = 60;
    public List<Card> mainDeck = new List<Card>();
    private List<Card> backupDeck;
    public Transform cardCountLabel = null;
    public Transform drawButton = null;

    public string[] allCards;
    public Dictionary<string, Card> cardTypes;

    // Use this for initialization
    public Deck() 
    {
        Card[] cards = Resources.LoadAll<Card>("Cards/Tests/");

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < cards.Length; j++) {
                AddCard(cards[j]);
            }
        }
        backupDeck = new List<Card>(mainDeck);
        Shuffle();
    }

    // This is for a custom deck initialization
    public Deck(Dictionary<Card, int> cardsInDeck) 
    {
        foreach (Card card in cardsInDeck.Keys) {
            for (int i = 0; i < cardsInDeck[card]; i++) {
                Debug.Log("adding: " + card + " number: " + i);
                AddCard(card);
            }
        }
        backupDeck = new List<Card>(mainDeck);
        Shuffle();
    }

    // Get card count
    public int GetCardCount() {
        return cardCount;
    }

    // Add a card to the deck 
    public bool AddCard(Card card)
    {
        if (cardCount < maxCards) {
            mainDeck.Add(card);
            cardCount++;
            return true;
        }
        return false;
    }

    // Remove a card from the deck
    public void RemoveCard(Card card)
    {
        mainDeck.Remove(card);
        cardCount--;
    }

    // Reorder and shuffle the deck
    public void Shuffle()
    {
        List<Card> newDeck = new List<Card>();

        // Randomly selects a card from the deck and places into a new deck
        // Once all cards have been moved, it sets the old deck equal to the new deck
        for (int i = 0; i < mainDeck.Count; i++)
        {
            int randomInt = Random.Range(0, mainDeck.Count);
            Card cardToBeRemoved = mainDeck[randomInt];
            newDeck.Add(cardToBeRemoved);
        }
        mainDeck.Clear();
        mainDeck = newDeck;
    }

    // Gets the top card from the deck
    private Card GetTopCard() {
        Card topCard = mainDeck[0];
        RemoveCard(topCard);
        return topCard;
    }

    // Draws the top card from the deck
    public Card DrawCard()
    {
        if (cardCount > 0) {
            return GetTopCard();
        }
        else {
            // Reset and shuffle deck if out of cards
            mainDeck = new List<Card>(backupDeck);
            Shuffle();
            return GetTopCard();
        }
    }
}