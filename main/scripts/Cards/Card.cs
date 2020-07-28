using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Card : ScriptableObject
{
    public string cardName;
    public string cardDesc;

    public Sprite artwork;

    public Race race;
    public CardType cardType;
    public ResourceType res1;
    public int res1Cost;

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

    // Load a test unit card
    public static CardUnit LoadTestUnitCard() {
        CardUnit newCard = Resources.Load<CardUnit>("Cards/Tests/Wizard");
        return newCard;
    }

    // Load a test resource card
    public static CardResource LoadTestResourceCard() {
        CardResource newCard = Resources.Load<CardResource>("Cards/Tests/Food");
        return newCard;
    }

    // Load a card
    public static Card LoadCard(Race cardRace, CardType cardType, string cardName) {
        string cardPath = "Cards/" + cardRace.ToString() + "/" + cardType.ToString() + "/" + cardName;
        Card newCard;
        if (cardType == CardType.Unit) {
            newCard = Resources.Load<CardUnit>(cardPath);
        }
        else if (cardType == CardType.Building) {
            newCard = Resources.Load<CardBuilding>(cardPath);
        }
        else {
            newCard = Resources.Load<CardResource>(cardPath);
        }
        return newCard;
    }

    // Initialize into the parent transform
    public static CardDisplay Initialize(Card card, Transform parentTransform)
    {
        if (card.cardType == CardType.Unit)
        {
            CardUnitDisplay cardUnitDisplay = Instantiate(GetCardUnitDisplayPrefab(), parentTransform);
            cardUnitDisplay.SetCard((CardUnit)card);
            return cardUnitDisplay;
        }
        else if (card.cardType == CardType.Building)
        {
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

    /*
        //Creates a card of card type and sets the asset path
        public static Card CreateCard(CardType cardType, string assetPath)
        {
            Card newCard;

            if (cardType == CardType.Unit)
            {
                newCard = CreateInstance<CardUnit>();
                newCard = (CardUnit)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardUnit));
            }
            else if (cardType == CardType.Building)
            {
                newCard = CreateInstance<CardBuilding>();
                newCard = (CardBuilding)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardBuilding));
            }
            else if (cardType == CardType.Spell)
            {
                newCard = CreateInstance<CardSpell>();
                newCard = (CardSpell)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardSpell));
            }
            else
            {
                newCard = CreateInstance<CardResource>();
                newCard = (CardResource)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardResource));
            }

            newCard.cardType = cardType;

            return newCard;
        }
    */

}
