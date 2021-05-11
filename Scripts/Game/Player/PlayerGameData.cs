using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameData
{
    // Drawn and played cards
    public int numDrawnCards { get; private set; }
    public int numPlayedCards { get; private set; }
    public Dictionary<CardType, int> drawnCardTypes = new Dictionary<CardType, int>(){
        {CardType.Unit, 0 },
        {CardType.Building, 0 },
        {CardType.Resource, 0 } 
    };
    public Dictionary<Race, int> drawnCardRaces = new Dictionary<Race, int>(){
        {Race.Human, 0 },
        {Race.Forest, 0 },
        {Race.Magic, 0 },
        {Race.Undead, 0 },
        {Race.None, 0 }
    };
    public Dictionary<CardType, int> playedCardTypes = new Dictionary<CardType, int>(){
        {CardType.Unit, 0 },
        {CardType.Building, 0 },
        {CardType.Resource, 0 }
    };
    public Dictionary<Race, int> playedCardRaces = new Dictionary<Race, int>(){
        {Race.Human, 0 },
        {Race.Forest, 0 },
        {Race.Magic, 0 },
        {Race.Undead, 0 },
        {Race.None, 0 }
    };
    public Dictionary<Race, int> playedPieceRaces = new Dictionary<Race, int>() {
        {Race.Human, 0 },
        {Race.Forest, 0 },
        {Race.Magic, 0 },
        {Race.Undead, 0 },
        {Race.None, 0 }
    };

    // Damage data
    public int numPiecesDefeated { get; private set; } = 0;
    public int damageGiven { get; private set; } = 0;
    public int damageTaken { get; private set; } = 0;

    // Resource data
    public Dictionary<ResourceType, int> drawnResources = new Dictionary<ResourceType, int>() {
        {ResourceType.Food, 0 },
        {ResourceType.Wood, 0 },
        {ResourceType.Mana, 0 }
    };
    public Dictionary<ResourceType, int> collectedResources = new Dictionary<ResourceType, int>() {
        {ResourceType.Food, 0 },
        {ResourceType.Wood, 0 },
        {ResourceType.Mana, 0 }
    };

    // Constructor
    public PlayerGameData() { }

    // Add drawn card data
    public void AddDrawnCard(Card card) {
        drawnCardTypes[card.cardType]++;
        drawnCardRaces[card.race]++;
        numDrawnCards++;

        if (card.cardType == CardType.Resource) {
            drawnResources[card.res1]++;
        }
    }

    // Add played card data
    public void AddPlayedCard(Card card) {
        playedCardTypes[card.cardType]++;
        playedCardRaces[card.race]++;
        numPlayedCards++;
        playedPieceRaces[card.race]++;
    }

    // Add defeated piece
    public void AddPiecesDefeated(int piecesDefeated) {
        numPiecesDefeated += piecesDefeated;
    }

    // Add damage given
    public void AddDamageGiven(int damageGiven) {
        this.damageGiven += damageGiven;
    }

    // Add damage taken
    public void AddDamageTaken(int damageTaken) {
        this.damageTaken += damageTaken;
    }

    // Add collected resource
    public void AddCollectedResource(ResourceType collectedResource) {
        collectedResources[collectedResource]++;
    }
}
