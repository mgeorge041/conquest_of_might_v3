using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    private Player player;
    public GameMap gameMap;

    // Map objects
    public GameMapObject gameMapObject;
    public ActionMapObject actionMapObject;
    public FogMapObject fogMapObject;

    public Tilemap movementTilemap;
    public Tilemap fogOfWarTilemap;
    public Transform cardRegion;
    public Transform drawnCardRegion;
    public Button endTurnButton;
    public HandObject handObject;
    public MapCamera playerCamera;
    public ResourceCounter resourceCounter;
    private GameManagerObject gameManagerObject;
    private CardPieceDisplay selectedCardDisplay;

    public Text currentTurnText;

    // Game piece objects
    private Dictionary<GamePiece, GamePieceObject> gamePieceObjects = new Dictionary<GamePiece, GamePieceObject>();

    // Set player
    public void SetPlayer(Player player) {
        this.player = player;
        fogMapObject.SetFogMap(player.fogMap);
        fogMapObject.PaintInitialFogMap();
        actionMapObject.SetActionMap(player.actionMap);
    }

    // Get player
    public Player GetPlayer() {
        return player;
    }

    // Set game map
    public void SetGameMap(GameMap gameMap) {
        this.gameMap = gameMap;
    }

    // Set game map object
    public void SetGameMapObject(GameMapObject gameMapObject) {
        this.gameMapObject = gameMapObject;
    }

    // Set game manager object
    public void SetGameManagerObject(GameManagerObject gameManagerObject) {
        this.gameManagerObject = gameManagerObject;
    }

    // Get whether is turn
    public bool IsTurn() {
        return player.isTurn;
    }

    // Set up first turn
    public void StartFirstTurn() {
        InstantiateStartingPieces();
        player.StartFirstTurn();
        fogMapObject.PaintFogMap();
        resourceCounter.UpdateStartingResources(player.GetResources());
        InstantiateStartingCards();

        // Set camera centered over start
        playerCamera.tilemap = gameMapObject.tilemap;
        playerCamera.UpdateCameraBounds();
        playerCamera.MoveCameraToPosition(gameMapObject.GetWorldCoordsFromTileCoords(player.startTileCoords));
    }

    // Start turn
    public void StartTurn() {
        gameObject.SetActive(true);
        endTurnButton.interactable = false;

        // Show drawn card
        Card drawnCard = player.StartTurn();

        // Resource card
        if (drawnCard.cardType == CardType.Resource) {
            CardResourceDisplay newCardDisplay = (CardResourceDisplay)CardDisplay.CreateCardDisplay(drawnCard, drawnCardRegion);
            StartCoroutine(ShowDrawnCard(newCardDisplay));
        }

        // Playable card
        else {
            CardPieceDisplay newCardDisplay = (CardPieceDisplay)CardDisplay.CreateCardDisplay(drawnCard, drawnCardRegion);
            newCardDisplay.player = player;
            StartCoroutine(ShowDrawnCard(newCardDisplay));
        }
        
        // Update piece objects
        foreach (KeyValuePair<GamePiece, GamePieceObject> pair in gamePieceObjects) {
            if (pair.Key.hasActions) {
                pair.Value.ShowPieceDisabled();
            }
        }
    }

    // Show current player's turn
    public IEnumerator ShowDrawnCard(CardDisplay cardDisplay) {
        cardDisplay.SetIsDrawn();
        yield return new WaitForSeconds(2);
        
        // Add non-resource cards to hand
        if (cardDisplay.GetCard().cardType != CardType.Resource) {
            handObject.AddDrawnCard((CardPieceDisplay)cardDisplay);
        }
        else {
            Destroy(cardDisplay.gameObject);
            resourceCounter.UpdateAllResources(player.GetResources());
        }

        // Remove new turn display and enable end turn button
        currentTurnText.transform.parent.gameObject.SetActive(false);
        endTurnButton.interactable = true;
    }

    // End turn
    public void EndTurn() {
        player.EndTurn();
        gameObject.SetActive(false);
        gameManagerObject.NextTurn();
    }

    // Instantiate starting cards
    public void InstantiateStartingCards() {
        List<CardPieceDisplay> cards = player.cardPieceDisplays;
        //handObject.AddCards(cards);
    }

    // Instantiate player pieces
    public void InstantiateStartingPieces() {
        List<GamePiece> pieces = player.pieces;
        for (int i = 0; i < pieces.Count; i++) {
            GamePieceObject newPieceObject = GamePieceObject.InitializeFromGamePiece(pieces[i], gameMapObject.tilemap.transform, player.playerId);
            newPieceObject.SetPosition(gameMapObject);
            gamePieceObjects[pieces[i]] = newPieceObject;
        }
        fogMapObject.PaintFogMap();
    }

    // Create piece
    public GamePieceObject CreatePiece(CardPiece cardPiece) {
        GamePieceObject newPieceObject = GamePieceObject.InitializeFromCardPiece(cardPiece, gameMapObject.tilemap.transform, player);
        return newPieceObject;
    }

    // Create piece object
    public GamePieceObject CreatePieceObject(GamePiece piece) {
        GamePieceObject newPieceObject = GamePieceObject.InitializeFromGamePiece(piece, gameMapObject.tilemap.transform, player.playerId);
        return newPieceObject;
    }

    // Create and position piece object
    public void CreateAndPositionPieceObject(GamePiece newPiece) {
        GamePieceObject newPieceObject = CreatePieceObject(newPiece);
        gamePieceObjects[newPiece] = newPieceObject;
        newPieceObject.SetPosition(gameMapObject);
        resourceCounter.UpdateAllResources(player.GetResources());
    }

    // Update piece lifebar
    public void UpdateGamePieceObjectLifebar(GamePiece piece) {
        gamePieceObjects[piece].SetLifebarCurrentHealth();

        // Destroy piece object if dead
        if (piece.currentHealth == 0) {
            Destroy(gamePieceObjects[piece].gameObject);
            gamePieceObjects.Remove(piece);
        }
    }

    // Attack piece
    private void AttackPiece(Vector3Int targetTileCoords) {

        // Get targeted piece and player object
        GamePiece targetPiece = gameMap.GetHexPieceFromHexCoords(Hex.TileToHexCoords(targetTileCoords));
        PlayerObject playerObject = gameManagerObject.GetPlayerObject(targetPiece.GetPlayerId());

        // Attack piece
        GamePiece attackingPiece = player.selectedPiece;
        attackingPiece.AttackPiece(targetPiece);

        // Update the targeted piece lifebar
        playerObject.UpdateGamePieceObjectLifebar(targetPiece);

        // Set attacking piece as disabled
        GamePieceObject gamePieceObject = gamePieceObjects[attackingPiece];
        gamePieceObject.ShowPieceDisabled();

        // Update maps
        player.UpdateMaps(gameMap);
        PaintPlayerMaps();
    }

    // Lost piece
    public void LostPiece(GamePiece piece) {
        gamePieceObjects.Remove(piece);
    }

    // Update maps
    private void PaintPlayerMaps() {
        fogMapObject.DrawFogMap(player.pieces);
        actionMapObject.DrawActionMap(player.selectedPiece, gameMap, player.fogMap);
    }

    // Move piece
    private void MovePiece(Vector3Int targetTileCoords) {
        
        // Move piece
        gameMap.MovePiece(player.GetSelectedUnit(), targetTileCoords);
        gamePieceObjects[player.selectedPiece].SetPosition(gameMapObject);
        
        // Update maps
        //player.UpdateMaps(gameMap);
        PaintPlayerMaps();
    }

    // Play piece at location
    public void PlayCardAtTile(CardPiece cardPiece, Vector3Int tileCoords) {
        GamePiece newPiece = GamePiece.CreatePiece<GamePiece>(cardPiece, player);
        gameMap.AddPiece(newPiece, tileCoords);
        CreateAndPositionPieceObject(newPiece);
    }

    // Play selected card at location
    public void PlaySelectedCardAtTile(Vector3Int tileCoords) {
        CardPiece selectedCard = player.selectedCard.GetCardPiece();
        GamePiece newPiece = GamePiece.CreatePiece<GamePiece>(selectedCard, player);
        gameMap.AddPiece(newPiece, tileCoords);
        CreateAndPositionPieceObject(newPiece);
        Destroy(selectedCardDisplay.gameObject);

        player.PlayPiece(newPiece);
        resourceCounter.UpdateAllResources(player.GetResources());

        // Update maps
        actionMapObject.ClearPaintedTiles();
        fogMapObject.PaintFogMap();

        // Update cards in hand
        handObject.ShowPlayableCards();
    }

    // Set selected card
    public void SetSelectedCard(CardPieceDisplay newSelectedCard) {

        selectedCardDisplay = newSelectedCard;

        // Get card piece
        if (newSelectedCard != null) {
            CardPiece cardPiece = newSelectedCard.GetCardPiece();
            player.SetSelectedCard(newSelectedCard);
        }
        else {
            player.SetSelectedCard(null);
        }
        actionMapObject.PaintActionMap();
    }

    // Show current player's turn
    public IEnumerator ShowCurrentPlayerTurn(int playerId) {
        currentTurnText.transform.parent.gameObject.SetActive(true);
        currentTurnText.text = "Player " + (playerId + 1) + " Turn";
        yield return new WaitForSeconds(2);
        if (!IsTurn()) {
            currentTurnText.transform.parent.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Awake() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update() {

        // Get map tile when click on map
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector3Int tileCoords = gameMapObject.GetMouseTileCoords(playerCamera.camera, Input.mousePosition);
            player.SetSelectedCard(null);
            Debug.Log("got tile coords: " + tileCoords);
            
            // Click on map tile
            if (tileCoords != null) {
                GameHex gameHex = gameMap.GetHexAtTileCoords(tileCoords);
                GamePiece piece = gameHex.piece;
                Debug.Log("got piece: " + piece);

                // Piece is on tile
                if (piece != null) {
                    Debug.Log("piece id: " + piece.GetPlayerId() + " player id: " + player.playerId);

                    // Piece is current player's
                    if (piece.GetPlayerId() == player.playerId && player.isTurn) {
                        
                        // Piece is already selected
                        if (piece == player.selectedPiece) {
                            Debug.Log("piece is selected piece, clearing");
                            player.ClearSelectedPiece();
                            actionMapObject.PaintActionMap();
                        }
                        else if (piece.hasActions) {
                            Debug.Log("piece can move, creating map");
                            player.SetSelectedPiece(piece);
                            actionMapObject.PaintActionMap();
                        }
                    }

                    // Piece is another player's
                    // SHOW UNIT DETAILS
                }
                else {
                    player.ClearSelectedPiece();
                }
            }
        }

        // Get map tile when right click on map
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector3Int tileCoords = gameMapObject.GetMouseTileCoords(playerCamera.camera, Input.mousePosition);
            Debug.Log("got tile coords: " + tileCoords);

            // Click on map tile
            if (tileCoords != null) {

                // Play card from hand
                if (player.hasSelectedCard && player.actionMap.PlayableToTileAtTileCoords(tileCoords)) {
                    Debug.Log("playing selected card");
                    PlaySelectedCardAtTile(tileCoords);
                }

                // Move or attack piece
                if (player.HasSelectedPiece()) {
                    Debug.Log("player has selected piece");

                    // Move to tile
                    if (player.actionMap.MoveableToTileAtTileCoords(tileCoords)) {
                        Debug.Log("moving selected piece");
                        MovePiece(tileCoords);
                    }

                    // Attack tile
                    else if (player.actionMap.AttackableTileAtTileCoords(tileCoords)) {
                        Debug.Log("attacking piece");
                        AttackPiece(tileCoords);
                    }
                }
            }
        }
    }
}
