using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuildingDisplay : CardPieceDisplay
{
    private CardBuilding cardBuilding;

    // Initialize
    public void Initialize()
    {
        SetSharedInfo(cardBuilding);
        Set2ndResource(cardBuilding);
        SetCardLifebarOverlay(cardBuilding.lifebarOverlay);
        cardMight.text = cardBuilding.might.ToString();
        cardRange.text = cardBuilding.range.ToString();
    }

    // Set card
    public void SetCard(CardBuilding cardBuilding) {
        this.cardBuilding = cardBuilding;
        Initialize();
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
