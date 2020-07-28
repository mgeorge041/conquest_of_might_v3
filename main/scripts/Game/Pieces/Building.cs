using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GamePiece
{
    private CardBuilding cardBuilding;

    public Building(CardBuilding cardBuilding, int playerId) {
        this.cardBuilding = cardBuilding;
        this.playerId = playerId;
        SetSharedInfo(cardBuilding);
        pieceType = PieceType.Building;
    }

    // Get card
    public override CardPiece GetCard() {
        return cardBuilding;
    }

    // Set card
    public void SetCard(CardBuilding cardBuilding) {
        this.cardBuilding = cardBuilding;
    }
}
