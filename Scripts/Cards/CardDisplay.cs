using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardDisplay : MonoBehaviour
{
    // Hand
    protected PlayerObject playerObject;

    // Card data
    public Card card;
    public Image cardArt;
    public Text cardName;
    public Image res1;
    public Text res1Cost;

    // Card images
    public Image cardBorder;

    // Sets info shared by all cards
    public void SetSharedInfo(Card card) {
        cardArt.sprite = card.artwork;
        cardName.text = card.cardName;

        // Resource images
        res1.sprite = Resources.Load<Sprite>("Art/UI/" + card.res1.ToString());
        res1Cost.text = card.res1Cost.ToString();
    }

    // Destroys card
    public void Destroy() {
        Destroy(gameObject);
    }

    // Get and set card
    public abstract Card GetCard();

    // Get card unit display prefab
    private static CardUnitDisplay GetCardUnitDisplayPrefab() {
        CardUnitDisplay cardUnitDisplayPrefab = Resources.Load<CardUnitDisplay>("Prefabs/Card Unit Display");
        return cardUnitDisplayPrefab;
    }

    // Get card building display prefab
    private static CardBuildingDisplay GetCardBuildingDisplayPrefab() {
        CardBuildingDisplay cardBuildingDisplayPrefab = Resources.Load<CardBuildingDisplay>("Prefabs/Card Building Display");
        return cardBuildingDisplayPrefab;
    }

    // Get card resource display prefab
    private static CardResourceDisplay GetCardResourceDisplayPrefab() {
        CardResourceDisplay cardResourceDisplayPrefab = Resources.Load<CardResourceDisplay>("Prefabs/Card Resource Display");
        return cardResourceDisplayPrefab;
    }

    // Initialize into the parent transform
    public static CardDisplay Initialize(Card card, Transform parentTransform) {
        if (card.cardType == CardType.Unit) {
            CardUnitDisplay cardUnitDisplay = Instantiate(GetCardUnitDisplayPrefab(), parentTransform);
            cardUnitDisplay.SetCard((CardUnit)card);
            return cardUnitDisplay;
        }
        else if (card.cardType == CardType.Building) {
            CardBuildingDisplay cardBuildingDisplay = Instantiate(GetCardBuildingDisplayPrefab(), parentTransform);
            cardBuildingDisplay.SetCard((CardBuilding)card);
            return cardBuildingDisplay;
        }
        else if (card.cardType == CardType.Resource) {
            CardResourceDisplay cardResourceDisplay = Instantiate(GetCardResourceDisplayPrefab(), parentTransform);
            cardResourceDisplay.SetCard((CardResource)card);
            return cardResourceDisplay;
        }
        else {
            return null;
        }
        /*
        else if (cardType == CardType.Spell)
        {
            Debug.Log("i");
        }
        */
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
