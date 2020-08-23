using UnityEngine;

public class Card : ScriptableObject
{
    public string cardName;
    public string cardDesc;

    public Sprite artwork;

    public Race race;
    public CardType cardType;
    public ResourceType res1;
    public int res1Cost;

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
