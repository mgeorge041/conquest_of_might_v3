using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuildingDisplay : CardPieceDisplay
{
    private CardBuilding cardBuilding;

    // Set card
    public void SetCard(CardBuilding cardBuilding) {
        this.cardBuilding = cardBuilding;
    }

    // Get card
    public override CardPiece GetCardPiece() {
        return cardBuilding;
    }

    // Get card
    public override Card GetCard() {
        return cardBuilding;
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
