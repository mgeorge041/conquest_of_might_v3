using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GamePiece
{
    protected int health;
    protected int currentHealth;
    protected int might;
    protected int range;
    protected int sightRange;
    protected bool canAttack = false;
    protected bool canMove = true;
    protected int playerId;
    protected bool hasActions = true;

    // Game object
    private GamePieceObject gamePieceObject;

    // Collider
    public PieceType pieceType;

    // Game hex
    protected GameHex gameHex;

    public abstract CardPiece GetCard();

    // Load test unit
    public static Unit LoadTestUnit() {
        CardUnit cardUnit = Card.LoadTestUnitCard();
        Unit unit = new Unit(cardUnit, 0);
        return unit;
    }

    // Load test unit
    public static Unit LoadTestUnit(int playerId) {
        CardUnit cardUnit = Card.LoadTestUnitCard();
        Unit unit = new Unit(cardUnit, playerId);
        return unit;
    }

    // Set game piece object
    public void SetGamePieceObject(GamePieceObject gamePieceObject) {
        this.gamePieceObject = gamePieceObject;
    }

    // Get game piece object
    public GamePieceObject GetGamePieceObject() {
        return gamePieceObject;
    }

    // Get game hex
    public GameHex GetGameHex() {
        return gameHex;
    }

    // Set game hex
    public void SetGameHex(GameHex gameHex) {
        this.gameHex = gameHex;
    }

    // Get player ID
    public int GetPlayerId() {
        return playerId;
    }

    // Reset at beginning of turn
    public void ResetPiece() {
        SetCanAttack(true);
        SetCanMove(true);
        CheckHasActions();
    }

    // End piece turn
    public void EndTurn() {
        SetCanAttack(false);
        SetCanMove(false);
        CheckHasActions();
    }

    // Get whether piece has actions
    public bool HasActions() {
        return hasActions;
    }

    // Check whether piece has actions
    protected void CheckHasActions() {
        if (!canAttack && !canMove) {
            hasActions = false;
        }
        else {
            hasActions = true;
        }
    }

    // Set whether piece can attack
    public void SetCanAttack(bool canAttack) {
        this.canAttack = canAttack;
    }

    // Get whether piece can attack
    public bool CanAttack() {
        return canAttack;
    }

    // Take damage
    public bool TakeDamage(int damage) {
        currentHealth = Math.Max(currentHealth - damage, 0);
        if (currentHealth <= 0) {
            gameHex.ClearPiece();
            gameHex = null;
            return true;
        }
        return false;
    }

    // Get health
    public int GetHealth() {
        return health;
    }

    // Get current health
    public int GetCurrentHealth() {
        return currentHealth;
    }

    // Get might
    public int GetMight() {
        return might;
    }

    // Set whether piece can move
    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    // Get whether piece can move
    public bool CanMove() {
        if (pieceType == PieceType.Unit) {
            return canMove;
        }
        return false;
    }

    // Get range
    public int GetRange() {
        return range;
    }

    // Get sight range
    public int GetSightRange() {
        return sightRange;
    }

    // Set shared attributes
    public void SetSharedInfo(CardPiece cardPiece) {
        health = cardPiece.health;
        currentHealth = health;
        might = cardPiece.might;
        range = cardPiece.range;
        sightRange = cardPiece.sightRange;
    }
}
