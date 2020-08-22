using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    // Card variables
    private List<CardPieceDisplay> cardPieceDisplays = new List<CardPieceDisplay>();

    // Player
    private Player player;
    private Transform cardRegion;

    // Constructor
    public Hand(Transform cardRegion) {
        this.cardRegion = cardRegion;
    }

    // Constructor
    public Hand(Player player) {
        this.player = player;
    }

    // Set card region
    public void SetCardRegion(Transform cardRegion) {
        this.cardRegion = cardRegion;
    }

    // Add a card to the hand
    public void AddCard(Card card) {
        if (card != null) {
            if (card.cardType == CardType.Resource) {
                AddResourceCard(card);
            }
            else {
                AddPieceCard((CardPiece)card);
            }
        }
    }

    // Add a piece card to the hand
    public void AddPieceCard(CardPiece cardPiece) {
        CardPieceDisplay newCardDisplay = (CardPieceDisplay)Card.Initialize(cardPiece, cardRegion);
        newCardDisplay.SetPlayer(player);
        cardPieceDisplays.Add(newCardDisplay);
        ShowPlayableCard(newCardDisplay);
    }

    // Add a resource card to the hand
    public void AddResourceCard(Card card) {
        player.IncrementResource(card.res1, card.res1Cost);
        ShowPlayableCards();
    }

    // Get all cards in hand
    public List<CardPieceDisplay> GetCards() {
        return cardPieceDisplays;
    }

    // Play a card
    public void PlayCard(CardPieceDisplay selectedCard) {
        cardPieceDisplays.Remove(selectedCard);
        ShowPlayableCards();
    }

    // Show whether card is playable
    private void ShowPlayableCard(CardPieceDisplay cardDisplay) {
        CardPiece cardPiece = cardDisplay.GetCardPiece();
        if (!CardIsPlayable(cardPiece)) {
            cardDisplay.SetUnplayableBorder();
        }
    }

    // Show which cards are playable
    public void ShowPlayableCards() {
        for (int i = 0; i < cardPieceDisplays.Count; i++) {
            CardPiece cardPiece = cardPieceDisplays[i].GetCardPiece();
            if (!CardIsPlayable(cardPiece)) {
                cardPieceDisplays[i].SetUnplayableBorder();
            }
            else {
                cardPieceDisplays[i].SetPlayableBorder();
            }
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