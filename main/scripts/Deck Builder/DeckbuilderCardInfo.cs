using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckbuilderCardInfo : MonoBehaviour
{
    public Image cardIcon;
    public Text cardName;
    public Text cardCountLabel;
    private int cardCount = 0;

    private DeckBuilder deckBuilder;

    private Card card;

    // Remove card from deck builder
    public void RemoveCard() {
        deckBuilder.RemoveCard(card);
    }

    // Set deck builder
    public void SetDeckBuilder(DeckBuilder deckBuilder) {
        this.deckBuilder = deckBuilder;
    }

    // Set card
    public void SetCard(Card card) {
        this.card = card;
    }

    // Increment label count
    public void IncrementLabelCount(int amount) {
        cardCount += amount;
        cardCountLabel.text = cardCount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        cardIcon.sprite = card.artwork;
        cardName.text = card.cardName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
