using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionPanel : MonoBehaviour
{
    public AddCardPanel addCardPanelPrefab;
    public DeckBuilder deckBuilder;
    public Transform addCardPanelList;
    public Transform cardDisplayPanel;

    // Card display prefabs
    private CardDisplay currentCardDisplay;
    public CardUnitDisplay cardUnitDisplayPrefab;
    public CardResourceDisplay cardResourceDisplayPrefab;
    public CardBuildingDisplay cardBuildingDisplayPrefab;
    private Dictionary<CardType, CardDisplay> cardDisplayPrefabs;

    // Show card display
    public void ShowCardDisplay(Card card) {
        try {

            // Do nothing if the card is the same
            if (currentCardDisplay.GetCard() == card) {
                return;
            }

            // Create a new card
            Destroy(currentCardDisplay.gameObject);
            currentCardDisplay = CardDisplay.Initialize(card, cardDisplayPanel);
        }
        catch {
            // Create a new card
            currentCardDisplay = CardDisplay.Initialize(card, cardDisplayPanel);
        }
    }

    // Get all cards of the race type
    public void LoadCards(string raceName) {
        Card[] cards = Resources.LoadAll<Card>("Cards/" + raceName + "/");
        Debug.Log("loading cards");
        Debug.Log("got cards: " + cards.Length);
        for (int i = 0; i < cards.Length; i++) {
            AddCardPanel newAddCardPanel = Instantiate(addCardPanelPrefab, addCardPanelList);
            newAddCardPanel.SetCard(cards[i]);
            newAddCardPanel.SetDeckBuilder(deckBuilder);
            newAddCardPanel.SetCardSelectionPanel(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cardDisplayPrefabs = new Dictionary<CardType, CardDisplay>() {
            {CardType.Unit, cardUnitDisplayPrefab },
            {CardType.Building, cardBuildingDisplayPrefab },
            {CardType.Resource, cardResourceDisplayPrefab }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
