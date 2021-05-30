using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GamePiece
{
    private CardUnit cardUnit;
    public bool canMove { get; set; } = true;
    public int speed { get; private set; }
    public int remainingSpeed { get; set; }

    // Get card
    public override CardPiece GetCard() {
        return cardUnit;
    }

    // Set card
    public override void SetCard(CardPiece cardPiece)
    {
        if (cardPiece is CardUnit)
        {
            CardUnit cardUnit = (CardUnit)cardPiece;
            this.cardUnit = cardUnit;
            pieceType = PieceType.Unit;
            SetCardStats();
        }
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
    public override void ResetPiece() {
        remainingSpeed = speed;
        canAttack = true;
        canMove = true;
        CheckHasActions();
    }

    // End piece turn
    public override void EndTurn() {
        canAttack = false;
        DecreaseSpeed(remainingSpeed);
        CheckHasActions();
    }

    // Check whether piece has actions
    protected override void CheckHasActions()
    {
        if (!canAttack && !canMove)
        {
            hasActions = false;
            ShowPieceDisabled();
        }
        else
        {
            hasActions = true;
            ShowPieceDisabled();
        }
    }

    // Set information
    public void SetCardStats() {
        SetCardStats(cardUnit);
        speed = cardUnit.speed;
        pieceType = PieceType.Unit;
        remainingSpeed = speed;
    }
}
