using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player
{
    // Cards
    private Hand hand;
    private Deck deck;
    private bool hasSelectedCard = false;
    private CardPieceDisplay selectedCard;

    // Player variables
    public readonly int playerId;
    private Vector3Int startTileCoords;
    public bool isTurn;

    // Maps
    public GameMap gameMap;
    public MovementMap movementMap;
    public FogOfWarMap fogOfWarMap;

    // Game manager
    private GameManager gameManager;

    // Pieces
    private GamePiece selectedPiece;
    private List<GamePiece> pieces = new List<GamePiece>();

    // Resources
    private Dictionary<ResourceType, int> resources;

    // Constructor
    public Player(int playerId, GameMap gameMap, Vector3Int startTileCoords) {

        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;

        // Game map
        this.gameMap = gameMap;

        Initialize();
    }

    // Constructor
    public Player(int playerId, GameMap gameMap, Vector3Int startTileCoords, GameManager gameManager) {
        
        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;

        // Game map
        this.gameMap = gameMap;

        // Game manager
        this.gameManager = gameManager;

        Initialize();
    }

    // Initialize hand and deck
    private void CreateHandAndDeck() {
        hand = new Hand(this);
        deck = new Deck();
    }

    // Create empty resource counts
    private void CreateResourceCounts() {
        resources = new Dictionary<ResourceType, int>() {
            {ResourceType.Food, 0 },
            {ResourceType.Wood, 0 },
            {ResourceType.Mana, 0 }
        };
    }

    // Draw starting hand
    public void DrawStartingHand() {
        for (int i = 0; i < 15; i++) {
            DrawTopCard();
        }
    }

    // Initialize starting setup
    public void Initialize() {
        CreateHandAndDeck();
        CreateResourceCounts();
    }

    // Initialize game setup
    public void InitializeGameSetup() {
        DrawStartingHand();

        // Play castle at start location
        CardBuilding castle = Resources.Load<CardBuilding>("Cards/Building/Castle");
        PlayCardAtTile(castle, startTileCoords);

        // Play unit at center
        CardUnit wizard = Card.LoadTestUnitCard();
        PlayCardAtTile(wizard, startTileCoords + new Vector3Int(1, -1, 0));
    }

    // Set movement map
    public void SetMovementMap(Tilemap movementTilemap) {
        movementMap = new MovementMap(movementTilemap);
    }

    // Set fog of war map
    public void SetFogOfWarMap(Tilemap fogOfWarTilemap) {
        fogOfWarMap = new FogOfWarMap(fogOfWarTilemap);
        fogOfWarMap.DrawFogOfWarMap(pieces);
    }

    // Start turn
    public Card StartTurn() {
        isTurn = true;
        for (int i = 0; i < pieces.Count; i++) {
            if (pieces[i].pieceType == PieceType.Unit) {
                Unit unit = (Unit)pieces[i];
                unit.ResetPiece();
            }
            else {
                pieces[i].ResetPiece();
            }
        }
        return DrawTopCard();
    }

    // End turn
    public void EndTurn() {
        isTurn = false;
        ClearSelectedPiece();
        SetSelectedCard(null);
        for (int i = 0; i < pieces.Count; i++) {
            pieces[i].EndTurn();
        }
        gameManager.NextTurn();
    }

    // Decrements played card resources
    public void DecrementPlayedCardResources(CardPiece playedCard) {
        Dictionary<ResourceType, int> resourceCosts = playedCard.GetResourceCosts();
        foreach (KeyValuePair<ResourceType, int> pair in resourceCosts) {
            IncrementResource(pair.Key, -pair.Value);
        }
    }

    // Set resource count
    public void SetResource(ResourceType resourceType, int amount) {
        if (resourceType != ResourceType.None) {
            resources[resourceType] = amount;
        }
    }

    // Clear resources
    public void ClearResources() {
        CreateResourceCounts();
    }

    // Increment resource count
    public void IncrementResource(ResourceType resourceType, int amount) {
        if (resourceType != ResourceType.None) {
            resources[resourceType] = Math.Max(resources[resourceType] + amount, 0);
        }
    }

    // Get resource level
    public int GetResourceCount(ResourceType resourceType) {
        if (resourceType != ResourceType.None) {
            return resources[resourceType];
        }
        return 0;
    }

    // Get resources
    public Dictionary<ResourceType, int> GetResources() {
        return resources;
    }

    // Set hand
    public void SetHand(Hand hand) {
        this.hand = hand;
    }

    // Get hand
    public Hand GetHand() {
        return hand;
    }

    // Get deck
    public Deck GetDeck() {
        return deck;
    }

    // Get deck size
    public int GetDeckSize() {
        return deck.GetCardCount();
    }

    // Draw card
    public Card DrawTopCard() {
        Card drawnCard = deck.DrawCard();
        hand.AddCard(drawnCard);
        return drawnCard;
    }

    // Draw specific card
    public void DrawCard(Card drawnCard) {
        hand.AddCard(drawnCard);
    }

    // Get selected card
    public CardPieceDisplay GetSelectedCard() {
        return selectedCard;
    }

    // Set selected card
    public void SetSelectedCard(CardPieceDisplay newSelectedCard) {
        
        // Unhighlight previously selected card
        if (selectedCard != null) {
            selectedCard.SetHighlighted(false);
        }

        // Set new selected card
        if (newSelectedCard != null) {
            selectedCard = newSelectedCard;
            hasSelectedCard = true;
            selectedPiece = null;
            movementMap.DrawPlayableMap(startTileCoords, gameMap);
        }
        else {
            selectedCard = null;
            hasSelectedCard = false;
            movementMap.ClearPaintedTiles();
        }
    }

    // Get whether has selected card
    public bool HasSelectedCard() {
        return hasSelectedCard;
    }

    // Get selected piece
    public GamePiece GetSelectedPiece() {
        return selectedPiece;
    }

    // Set selected piece
    public void SetSelectedPiece(GamePiece piece) {
        selectedPiece = piece;
        selectedCard = null;
        if (piece != null) {
            movementMap.DrawMovementMap(piece, gameMap, fogOfWarMap);
        }
    }

    // Clear selected piece
    public void ClearSelectedPiece() {
        selectedPiece = null;
        movementMap.ClearPaintedTiles();
    }

    // Get all pieces
    public List<GamePiece> GetPieces() {
        return pieces;
    }

    // Get selected unit
    public Unit GetSelectedUnit() {
        return (Unit)selectedPiece;
    }

    // Get selected building
    public Building GetSelectedBuilding() {
        return (Building)selectedPiece;
    }

    // Get whether has selected piece
    public bool HasSelectedPiece() {
        if (selectedPiece != null) {
            return true;
        }
        return false;
    }

    // Create piece, decrease resources, and show hand playable cards
    public GamePiece CreatePiece() {
        CardPiece cardPiece = GetSelectedCard().GetCardPiece();
        return CreatePiece(cardPiece);
    }

    // Create piece, decrease resources, and show hand playable cards
    public GamePiece CreatePiece(CardPiece cardPiece) {
        if (cardPiece.cardType == CardType.Unit) {
            Unit newUnit = new Unit((CardUnit)cardPiece, playerId);
            return newUnit;
        }
        else {
            Building newBuilding = new Building((CardBuilding)cardPiece, playerId);
            return newBuilding;
        }
    }

    // Play a piece to the game map
    public void PlayCard(GamePiece playedPiece) {
        pieces.Add(playedPiece);
        DecrementPlayedCardResources(playedPiece.GetCard());

        // Update hand
        hand.PlayCard(selectedCard);
        SetSelectedCard(null);
        hand.ShowPlayableCards();

        // Clear playble map
        movementMap.ClearPaintedTiles();
    }

    // Play piece at location
    public GamePiece PlayCardAtTile(CardPiece cardPiece, Vector3Int tileCoords) {
        GamePiece newPiece = CreatePiece(cardPiece);
        gameMap.AddPiece(newPiece, tileCoords);
        pieces.Add(newPiece);
        return newPiece;
    }

    // Play selected card at location
    public GamePiece PlaySelectedCardAtTile(Vector3Int tileCoords) {
        GamePiece newPiece = CreatePiece(selectedCard.GetCardPiece());
        gameMap.AddPiece(newPiece, tileCoords);
        PlayCard(newPiece);
        return newPiece;
    }

    // Move piece
    public void MovePiece(Vector3Int targetTileCoords) {
        gameMap.MovePiece(GetSelectedUnit(), targetTileCoords);
        movementMap.DrawMovementMap(GetSelectedUnit(), gameMap, fogOfWarMap);
        fogOfWarMap.DrawFogOfWarMap(pieces);
    }

    // Attack piece
    public void AttackPiece(GamePiece targetPiece) {
        gameMap.AttackPiece(GetSelectedPiece(), targetPiece);
        GetSelectedPiece().EndTurn();
        ClearSelectedPiece();
        movementMap.ClearPaintedTiles();
    }

    // Lost piece
    public void LostPiece(GamePiece piece) {
        pieces.Remove(piece);
        fogOfWarMap.DrawFogOfWarMap(pieces);
        if (piece.GetCard().cardName == "Castle") {
            gameManager.EndGame();
        }
    }
}