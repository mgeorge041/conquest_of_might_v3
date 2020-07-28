using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCardPanel : MonoBehaviour
{
    private Card card;

    public Image cardArt;
    public Text cardName;

    private DeckBuilder deckBuilder;
    private CardSelectionPanel cardSelectionPanel;

    // Show the card display
    public void ShowCardDisplay() {
        cardSelectionPanel.ShowCardDisplay(card);
    }

    // Add this card to the deck builder
    public void AddCardToList() {
        deckBuilder.AddCard(card);
    }

    // Remove this card from the deck builder
    public void RemoveCardFromList() {
        deckBuilder.RemoveCard(card);
    }

    // Set card selection panel
    public void SetCardSelectionPanel(CardSelectionPanel cardSelectionPanel) {
        this.cardSelectionPanel = cardSelectionPanel;
    }

    // Set deck builder
    public void SetDeckBuilder(DeckBuilder deckBuilder) {
        this.deckBuilder = deckBuilder;
    }

    // Set card
    public void SetCard(Card card) {
        this.card = card;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardArt.sprite = card.artwork;
        cardName.text = card.cardName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
