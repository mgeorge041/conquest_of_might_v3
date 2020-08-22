using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckbuilderCompositionPanel : MonoBehaviour
{
    // Resources
    public Text foodCountLabel;
    public Text woodCountLabel;
    public Text manaCountLabel;
    private Dictionary<ResourceType, Text> resourceLabels;
    private Dictionary<ResourceType, int> resourceCounts;

    // Pieces
    public Text unitCountLabel;
    public Text buildingCountLabel;
    private Dictionary<PieceType, Text> pieceLabels;
    private Dictionary<PieceType, int> pieceCounts;

    // Races
    public Text humanCount;
    public Text forestCount;
    public Text magicCount;
    public Text undeadCount;
    private Dictionary<Race, Text> raceLabels;
    private Dictionary<Race, int> raceCounts;

    // Set resource label count
    public void SetResourceLabelCount(ResourceType resourceType, int amount) {
        resourceCounts[resourceType] = amount;
        resourceLabels[resourceType].text = amount.ToString();
    }

    // Increment resource label count
    public void IncrementResourceLabelCount(ResourceType resourceType, int amount) {
        resourceCounts[resourceType] += amount;
        resourceLabels[resourceType].text = resourceCounts[resourceType].ToString();
    }

    // Set piece label count
    public void SetPieceLabelCount(PieceType pieceType, int amount) {
        pieceCounts[pieceType] = amount;
        pieceLabels[pieceType].text = amount.ToString();
    }

    // Increment piece label count
    public void IncrementPieceLabelCount(PieceType pieceType, int amount) {
        pieceCounts[pieceType] += amount;
        pieceLabels[pieceType].text = pieceCounts[pieceType].ToString();
    }

    // Set race label count
    public void SetRaceLabelCount(Race raceType, int amount) {
        raceCounts[raceType] = amount;
        raceLabels[raceType].text = amount.ToString();
    }

    // Increment race label count
    public void IncrementRaceLabelCount(Race raceType, int amount) {
        raceCounts[raceType] += amount;
        raceLabels[raceType].text = raceCounts[raceType].ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize dictionaries
        resourceLabels = new Dictionary<ResourceType, Text>() {
            {ResourceType.Food, foodCountLabel },
            {ResourceType.Wood, woodCountLabel },
            {ResourceType.Mana, manaCountLabel }
        };
        resourceCounts = new Dictionary<ResourceType, int>() {
            {ResourceType.Food, 0 },
            {ResourceType.Wood, 0 },
            {ResourceType.Mana, 0 }
        };
        pieceLabels = new Dictionary<PieceType, Text>() {
            {PieceType.Unit, unitCountLabel },
            {PieceType.Building, buildingCountLabel }
        };
        pieceCounts = new Dictionary<PieceType, int>() {
            {PieceType.Unit, 0 },
            {PieceType.Building, 0 }
        };
        raceLabels = new Dictionary<Race, Text>() {
            {Race.Human, humanCount },
            {Race.Forest, forestCount },
            {Race.Magic, magicCount },
            {Race.Undead, undeadCount }
        }; 
        raceCounts = new Dictionary<Race, int>() {
            {Race.Human, 0 },
            {Race.Forest, 0 },
            {Race.Magic, 0 },
            {Race.Undead, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
