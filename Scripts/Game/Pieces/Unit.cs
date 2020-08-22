using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece
{
    private CardUnit cardUnit;
    private int speed;
    private int remainingSpeed;

    // Constructor for playerless unit
    public Unit(CardUnit cardUnit) {
        this.cardUnit = cardUnit;
        SetInfo();
    }

    // Constructor for player unit
    public Unit(CardUnit cardUnit, Player player) {
        this.cardUnit = cardUnit;
        this.player = player;
        SetInfo();
    }

    // Get card
    public override CardPiece GetCard() {
        return cardUnit;
    }

    // Set card
    public void SetCard(CardUnit cardUnit) {
        this.cardUnit = cardUnit;
    }

    // Get speed
    public int GetSpeed() {
        return speed;
    }

    // Get remaining speed
    public int GetRemainingSpeed() {
        return remainingSpeed;
    }

    // Decrease speed
    public void DecreaseSpeed(int speedDecrease) {
        remainingSpeed = Math.Max(remainingSpeed - speedDecrease, 0);
        CheckHasActions();
    }

    // Reset at beginning of turn
    public new void ResetPiece() {
        remainingSpeed = speed;
        SetCanAttack(true);
        SetCanMove(true);
        CheckHasActions();
    }

    // End piece turn
    public new void EndTurn() {
        SetCanAttack(false);
        DecreaseSpeed(remainingSpeed);
    }

    // Set information
    public void SetInfo() {
        SetSharedInfo(cardUnit);
        speed = cardUnit.speed;
        pieceType = PieceType.Unit;
        remainingSpeed = speed;
    }
}
