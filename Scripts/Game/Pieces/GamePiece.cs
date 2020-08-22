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
    protected Player player;
    protected bool hasActions = true;

    // Game object
    private GamePieceObject gamePieceObject;

    // Collider
    public PieceType pieceType;

    // Game hex
    protected GameHex gameHex;

    public abstract CardPiece GetCard();

    // Create piece, decrease resources, and show hand playable cards
    public static GamePiece CreatePiece(CardPiece cardPiece, Player player) {
        if (cardPiece.cardType == CardType.Unit) {
            Unit newUnit = new Unit((CardUnit)cardPiece, player);
            return newUnit;
        }
        else {
            Building newBuilding = new Building((CardBuilding)cardPiece, player);
            return newBuilding;
        }
    }

    // Create starting castle
    public static GamePiece CreateCastle(Player player) {
        CardBuilding castle = Resources.Load<CardBuilding>("Cards/Building/Castle");
        GamePiece castlePiece = CreatePiece(castle, player);
        return castlePiece;
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

    // Get player
    public Player GetPlayer() {
        return player;
    }

    // Set player
    public void SetPlayer(Player player) {
        this.player = player;
    }

    // Get player ID
    public int GetPlayerId() {
        return player.playerId;
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

    // Attack piece
    public void AttackPiece(GamePiece targetPiece) {
        int damageGiven = targetPiece.TakeDamage(targetPiece.GetMight());
        player.GetPlayerGameData().AddDamageGiven(damageGiven);
        player.ClearSelectedPiece();
        EndTurn();
    }

    // Take damage
    public int TakeDamage(int damage) {
        int damageTaken = Math.Min(damage, currentHealth);
        currentHealth -= damageTaken;
        player.GetPlayerGameData().AddDamageTaken(damageTaken);

        // Remove piece if dead
        if (currentHealth <= 0) {
            gameHex.ClearPiece();
            gameHex = null;
            player.LostPiece(this);
        }
        return damageTaken;
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
