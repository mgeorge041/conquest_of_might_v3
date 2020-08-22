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
    private CardPiece selectedCard;
    private PlayerGameData gameData;

    // Player variables
    public readonly int playerId;
    private Vector3Int startTileCoords;
    public bool isTurn;

    // Maps
    public GameMap gameMap;
    public ActionMap actionMap;
    public FogMap fogMap;

    // Game manager
    private GameManager gameManager;

    // Pieces
    private GamePiece selectedPiece;
    private List<GamePiece> pieces = new List<GamePiece>();

    // Resources
    private Dictionary<ResourceType, int> resources;

    // Constructor
    public Player(int playerId, Vector3Int startTileCoords) {

        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;

        // Maps
        actionMap = new ActionMap(GameSetupData.mapRadius);
        fogMap = new FogMap(GameSetupData.mapRadius);

        Initialize();
    }

    // Constructor
    public Player(int playerId, Vector3Int startTileCoords, GameMap gameMap) {

        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;

        // Game map
        this.gameMap = gameMap;

        Initialize();
    }

    // Constructor
    public Player(int playerId, Vector3Int startTileCoords, GameMap gameMap, GameManager gameManager) {
        
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
        gameData = new PlayerGameData();
        CreateHandAndDeck();
        CreateResourceCounts();
        actionMap = new ActionMap();
        fogMap = new FogMap();
    }

    // Set tilemaps for maps
    public void SetMaps(Tilemap movementTilemap, Tilemap fogOfWarTilemap) {
        actionMap.SetTilemap(movementTilemap);
        fogMap.SetTilemap(fogOfWarTilemap);
    }

    // Initialize game setup
    public void StartFirstTurn() {
        DrawStartingHand();
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

    // Get game data
    public PlayerGameData GetPlayerGameData() {
        return gameData;
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

    // Get start tile coords
    public Vector3Int GetStartTileCoords() {
        return startTileCoords;
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
        gameData.AddDrawnCard(drawnCard);
        hand.AddCard(drawnCard);
        return drawnCard;
    }

    // Draw specific card
    public void DrawCard(Card drawnCard) {
        gameData.AddDrawnCard(drawnCard);
        hand.AddCard(drawnCard);
    }

    // Get selected card
    public CardPiece GetSelectedCard() {
        return selectedCard;
    }

    // Set selected card
    public void SetSelectedCard(CardPiece newSelectedCard) {

        // Set new selected card
        if (newSelectedCard != null) {
            selectedCard = newSelectedCard;
            hasSelectedCard = true;
            selectedPiece = null;
            actionMap.CreatePlayableMap(startTileCoords, gameMap);
        }
        else {
            selectedCard = null;
            hasSelectedCard = false;
            actionMap.ClearActionTiles();
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
            actionMap.CreateActionMap(piece, gameMap, fogMap);
        }
    }

    // Clear selected piece
    public void ClearSelectedPiece() {
        selectedPiece = null;
        actionMap.ClearActionTiles();
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

    // Play a piece to the game map
    public void PlayPiece(GamePiece playedPiece) {
        pieces.Add(playedPiece);
        DecrementPlayedCardResources(playedPiece.GetCard());
        gameData.AddPlayedCard(playedPiece.GetCard());

        // Update hand
        hand.PlayCard(selectedCard);
        SetSelectedCard(null);

        // Clear playble map
        actionMap.ClearActionTiles();
        fogMap.CreateFogMap(pieces);
    }

    // Update movement map and fog of war
    public void UpdateMaps(GameMap gameMap) {
        fogMap.CreateFogMap(pieces);
        actionMap.CreateActionMap(GetSelectedUnit(), gameMap, fogMap);
    }

    // Lost piece
    public void LostPiece(GamePiece piece) {
        pieces.Remove(piece);
        gameData.AddPiecesDefeated(1);
        fogMap.CreateFogMap(pieces);
        if (piece.GetCard().cardName == "Castle") {
            gameManager.EndGame();
        }
    }
}