using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GamePiece
{
    private CardBuilding cardBuilding;

    // Constructor for playerless building
    public Building(CardBuilding cardBuilding) {
        this.cardBuilding = cardBuilding;
        SetSharedInfo(cardBuilding);
        pieceType = PieceType.Building;
    }

    // Constructor for player building
    public Building(CardBuilding cardBuilding, Player player) {
        this.cardBuilding = cardBuilding;
        this.player = player;
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
