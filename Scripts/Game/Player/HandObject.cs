using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObject : MonoBehaviour
{
    // Card displays
    public List<CardPieceDisplay> cards = new List<CardPieceDisplay>();

    // Player object
    public Player player;

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
            CardPieceDisplay newCardDisplay = (CardPieceDisplay)CardDisplay.CreateCardDisplay(card, cardRegion);
            newCardDisplay.player = player;
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
        if (!cardDisplay.isPlayable) {
            cardDisplay.isPlayable = false;
        }
    }

    // Show which cards are playable
    public void ShowPlayableCards() {
        for (int i = 0; i < cards.Count; i++) {
            if (!cards[i].isPlayable) {
                cards[i].isPlayable = false;
            }
            else {
                cards[i].isPlayable = true;
            }
        }
    }

    // Collapse hand
    public static void CollapseHand(Transform transform, float height)
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -height + 10), 5 * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, -height + 10);
    }

    // Expand hand
    public static void ExpandHand(Transform transform, float height)
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0), 5 * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, 0);
    }

    // Show or hide hand
    public void ToggleHand() {
        collapsing = !collapsing;
        if (collapsing) {
            CollapseHand(transform, height);
        }
        else {
            ExpandHand(transform, height);
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