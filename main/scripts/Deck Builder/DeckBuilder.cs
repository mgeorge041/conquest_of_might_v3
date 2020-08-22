using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour
{
    private readonly int numMaxCards = 60;
    private int numCards = 0;
    public Transform cardList;
    public DeckbuilderCardInfo deckbuilderCardInfoPrefab;
    public DeckbuilderCompositionPanel compositionPanel;
    private Dictionary<Card, int> deckBuilderCards = new Dictionary<Card, int>();
    private Dictionary<Card, DeckbuilderCardInfo> deckBuilderCardInfos = new Dictionary<Card, DeckbuilderCardInfo>();
    public Text cardCountLabel;

    // Add card to card list
    public void AddCard(Card card) {
        
        // Do nothing if deck is full
        if (numCards == numMaxCards) {
            return;
        }

        // Add new card infos
        if (deckBuilderCards.ContainsKey(card)) {

            // Increment label count for already used card
            deckBuilderCards[card]++;
            deckBuilderCardInfos[card].IncrementLabelCount(1);
        }
        else {

            // Create new prefab if new card
            DeckbuilderCardInfo newDeckBuilderCardInfo = Instantiate(deckbuilderCardInfoPrefab, cardList);
            newDeckBuilderCardInfo.SetCard(card);
            newDeckBuilderCardInfo.IncrementLabelCount(1);
            newDeckBuilderCardInfo.SetDeckBuilder(this);
            deckBuilderCards[card] = 1;
            deckBuilderCardInfos[card] = newDeckBuilderCardInfo;
        }
        numCards++;
        cardCountLabel.text = numCards.ToString();
    }

    // Remove card from card list
    public void RemoveCard(Card card) {

        // Do nothing if card not in deck
        if (!deckBuilderCards.ContainsKey(card)) {
            return;
        }

        // Remove card
        deckBuilderCards[card]--;
        if (deckBuilderCards[card] == 0) {

            // Remove if card count hits 0
            deckBuilderCards.Remove(card);
            Destroy(deckBuilderCardInfos[card].gameObject);
            deckBuilderCardInfos.Remove(card);
        }
        else {

            // Decrement label count for card
            deckBuilderCardInfos[card].IncrementLabelCount(-1);
        }
        numCards--;
        cardCountLabel.text = numCards.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
