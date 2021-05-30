using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Cards
    public Hand hand { get; set; }
    public Deck deck { get; private set; }
    public bool hasSelectedCard { get; private set; } = false;
    public CardPiece selectedCard { get; private set; }
    public PlayerGameData gameData { get; private set; }

    // Player variables
    public int playerId { get; private set; }
    public Vector3Int startTileCoords { get; private set; }
    public bool isTurn;

    // Maps
    public GameMap gameMap;
    public ActionMap actionMap;
    public FogMap fogMap;
    public MapCamera playerCamera;

    // UI
    public PlayerUI playerUI;

    // Game manager
    public PlayerObject playerObject;
    private GameManager gameManager;

    // Pieces
    public GamePiece selectedPiece { get; private set; }
    public List<GamePiece> pieces { get; private set; }

    // Resources
    public Dictionary<ResourceType, int> resources { get; private set; }

    // Constructor
    public Player(int playerId, Vector3Int startTileCoords) {

        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;

        // Maps
        //actionMap = new ActionMap(GameSetupData.mapRadius);
        //fogMap = new FogMap(GameSetupData.mapRadius);

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

    // Initialize starting setup
    public void Initialize()
    {
        gameData = new PlayerGameData();
        pieces = new List<GamePiece>();
        CreateHandAndDeck();
        CreateResourceCounts();
        //fogMap = new FogMap();
    }

    // Initialize player
    public void InitializeObject(int playerId, Vector3Int startTileCoords)
    {
        // Player items
        this.playerId = playerId;
        this.startTileCoords = startTileCoords;
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

    // Get deck size
    public int GetDeckSize() {
        return deck.cardCount;
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

    // Get selected piece
    public GamePiece GetSelectedPiece()
    {
        if (selectedPiece == null)
            return null;
        else if (selectedPiece.pieceType == PieceType.Unit)
            return (Unit)selectedPiece;
        else
            return (Building)selectedPiece;
    }

    // Set selected piece
    public void SetSelectedPiece(GamePiece piece) {
        if (piece == selectedPiece)
        {
            selectedPiece = null;
        }
        else
        {
            selectedPiece = piece;
        }
        selectedCard = null;
    }

    // Clear selected piece
    public void ClearSelectedPiece() {
        selectedPiece = null;
        actionMap.ClearActionTiles();
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

    // Player left clicks
    public void PlayerLeftClicksAtWorldPosition(Vector3 worldPosition)
    {
        // Get piece from click
        GamePiece piece = gameMap.GetHexPieceFromWorldPosition(worldPosition);

        // Return if piece is enemy or piece has no actions
        if (piece && (piece.GetPlayerId() != playerId || !piece.hasActions))
        {
            return;
        }

        SetSelectedPiece(piece);

        // Return if selected piece is null
        if (!selectedPiece)
        {
            actionMap.ClearActionTiles();
            return;
        }

        // Update map for piece
        if (selectedPiece.hasActions)
        {
            Debug.Log("Creating action map");
            actionMap.CreateActionMap(piece, gameMap);
        }
    }

    // Player right clicks
    public void PlayerRightClicksAtWorldPosition(Vector3 worldPosition)
    {
        // Return if no selected piece
        if (!selectedPiece)
        {
            return;
        }

        // Get clicked on hex
        GameHex targetHex = gameMap.GetWorldPositionHex(worldPosition);
        if (targetHex == null)
        {
            return;
        }

        // Move or attack piece
        if (actionMap.MoveableToTileAtTileCoords(targetHex.tileCoords))
        {
            gameMap.MovePiece((Unit)selectedPiece, targetHex.hexCoords);
            actionMap.ClearActionTiles();
            return;
        }
        if (actionMap.AttackableTileAtTileCoords(targetHex.tileCoords))
        {
            gameMap.AttackPiece(selectedPiece, targetHex.piece);
            actionMap.ClearActionTiles();
            return;
        }
    }

    // Start
    public void Start()
    {
        Initialize();
    }

    // Update
    public void Update()
    {
        // Player left clicks on game map
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("player left clicked");
            PlayerLeftClicksAtWorldPosition(playerCamera.camera.ScreenToWorldPoint(Input.mousePosition));
        }

        // Player right clicks on game map
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlayerRightClicksAtWorldPosition(playerCamera.camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}