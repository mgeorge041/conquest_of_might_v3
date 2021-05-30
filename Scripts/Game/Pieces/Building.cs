using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GamePiece
{
    private CardBuilding cardBuilding;

    // Get card
    public override CardPiece GetCard() {
        return cardBuilding;
    }

    // Set card
    public override void SetCard(CardPiece cardPiece)
    {
        if (cardPiece is CardBuilding)
        {
            CardBuilding cardBuilding = (CardBuilding)cardPiece;
            this.cardBuilding = cardBuilding;
            pieceType = PieceType.Building;
            SetCardStats(cardBuilding);
        }
    }
}
