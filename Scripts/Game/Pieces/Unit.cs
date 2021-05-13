using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece
{
    private CardUnit cardUnit;
    public int speed { get; private set; }
    public int remainingSpeed { get; set; }

    // Constructor for playerless unit
    public Unit(CardUnit cardUnit) {
        this.cardUnit = cardUnit;
        SetCardStats();
    }

    // Constructor for player unit
    public Unit(CardUnit cardUnit, Player player) {
        this.cardUnit = cardUnit;
        this.player = player;
        SetCardStats();
    }

    // Get card
    public override CardPiece GetCard() {
        return cardUnit;
    }

    // Set card
    public void SetCard(CardUnit cardUnit) {
        this.cardUnit = cardUnit;
        SetCardStats();
    }

    // Decrease speed
    public void DecreaseSpeed(int speedDecrease) {
        remainingSpeed = Math.Max(remainingSpeed - speedDecrease, 0);
        if (remainingSpeed == 0)
        {
            canMove = false;
        }
        CheckHasActions();
    }

    // Reset at beginning of turn
    public new void ResetPiece() {
        remainingSpeed = speed;
        canAttack = true;
        canMove = true;
        CheckHasActions();
    }

    // End piece turn
    public new void EndTurn() {
        canAttack = false;
        DecreaseSpeed(remainingSpeed);
        CheckHasActions();
    }

    // Set information
    public void SetCardStats() {
        base.SetCardStats(cardUnit);
        speed = cardUnit.speed;
        pieceType = PieceType.Unit;
        remainingSpeed = speed;
    }
}
