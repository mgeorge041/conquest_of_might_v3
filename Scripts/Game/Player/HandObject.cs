using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObject : MonoBehaviour
{
    // Card displays
    public List<CardPieceDisplay> cards = new List<CardPieceDisplay>();

    // Player object
    public PlayerObject playerObject;

    // Hand
    public Transform cardRegion;

    // Display variables
    float height;
    private bool collapsing = false;

    // Add card display
    public void AddDrawnCard(CardPieceDisplay cardPieceDisplay) {
        int newCardIndex = (cardRegion.childCount + 1) / 2;
        cardPieceDisplay.transform.SetParent(cardRegion);
        cardPieceDisplay.transform.SetSiblingIndex(newCardIndex);
        cards.Add(cardPieceDisplay);
        ShowPlayableCard(cardPieceDisplay);
    }

    // Add card display
    public void AddCard(Card card) {
        if (card.cardType != CardType.Resource) {
            CardPieceDisplay newCardDisplay = (CardPieceDisplay)CardDisplay.Initialize(card, cardRegion);
            newCardDisplay.SetPlayerObject(playerObject);
            cards.Add(newCardDisplay);
            ShowPlayableCard(newCardDisplay);
        }
    }

    // Add card displays
    public void AddCards(List<CardPiece> cards) {
        for (int i = 0; i < cards.Count; i++) {
            AddCard(cards[i]);
        }
    }

    // Show whether card is playable
    private void ShowPlayableCard(CardPieceDisplay cardDisplay) {
        CardPiece cardPiece = cardDisplay.GetCardPiece();
        if (!cardPiece.isPlayable) {
            cardDisplay.SetUnplayableBorder();
        }
    }

    // Show which cards are playable
    public void ShowPlayableCards() {
        for (int i = 0; i < cards.Count; i++) {
            CardPiece cardPiece = cards[i].GetCardPiece();
            if (!cardPiece.isPlayable) {
                cards[i].SetUnplayableBorder();
            }
            else {
                cards[i].SetPlayableBorder();
            }
        }
    }

    // Show or hide hand
    public void ToggleHand() {
        collapsing = !collapsing;
        if (collapsing) {
            Hand.CollapseHand(transform, height);
        }
        else {
            Hand.ExpandHand(transform, height);
        }
    }

    // Start is called before the first frame update
    void Start() {
        height = transform.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update() {
        // Collapse hand
        if (Input.GetKeyDown(KeyCode.C)) {
            ToggleHand();
        }
    }
}