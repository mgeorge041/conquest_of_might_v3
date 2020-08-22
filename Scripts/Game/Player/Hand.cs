using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand
{
    // Cards
    private List<CardPiece> cards = new List<CardPiece>();

    // Player
    private Player player;

    // Constructor
    public Hand(Player player) {
        this.player = player;
    }

    // Add a card to the hand
    public void AddCard(Card card) {
        if (card != null) {

            // Increment resource
            if (card.cardType == CardType.Resource) {
                player.IncrementResource(card.res1, card.res1Cost);
                SetPlayableCards();
            }

            // Add playable card
            else {
                cards.Add((CardPiece)card);
                SetPlayable((CardPiece)card);
            }
        }
    }

    // Get all cards in hand
    public List<CardPiece> GetCards() {
        return cards;
    }

    // Play a card
    public void PlayCard(CardPiece selectedCard) {
        if (selectedCard == null) {
            return;
        }
        cards.Remove(selectedCard);
        SetPlayableCards();
    }

    // Set whether card is playable
    private void SetPlayable(CardPiece cardPiece) {
        cardPiece.SetPlayable(CardIsPlayable(cardPiece));
    }

    // Set whether card is playable
    public void SetPlayableCards() {
        for (int i = 0; i < cards.Count; i++) {
            cards[i].SetPlayable(CardIsPlayable(cards[i]));
        }
    }

    // Get whether card is playable
    public bool CardIsPlayable(CardPiece cardPiece) {
        Dictionary<ResourceType, int> resourceCosts = cardPiece.GetResourceCosts();
        foreach (KeyValuePair<ResourceType, int> pair in resourceCosts) {
            if (player.GetResourceCount(pair.Key) < pair.Value) {
                return false;
            }
        }
        return true;
    }

    // Collapse hand
    public static void CollapseHand(Transform transform, float height) {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -height + 10), 5 * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, -height + 10);
    }

    // Expand hand
    public static void ExpandHand(Transform transform, float height) {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0), 5 * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, 0);
    }
}