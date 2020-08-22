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
    public Tilemap movementTilemap;
    public Tilemap fogOfWarTilemap;
    public Transform cardRegion;
    public HandObject handObject;
    public MapCamera playerCamera;
    public ResourceCounter resourceCounter;
    private GameManagerObject gameManagerObject;

    public Text currentTurnText;

    // Game piece objects
    private Dictionary<GamePiece, GamePieceObject> gamePieceObjects = new Dictionary<GamePiece, GamePieceObject>();

    // Set player
    public void SetPlayer(Player player) {
        this.player = player;
        this.player.GetHand().SetCardRegion(cardRegion);
        this.player.InitializeGameSetup();
        this.player.SetMovementMap(movementTilemap);
        this.player.SetFogOfWarMap(fogOfWarTilemap);
    }

    // Set game map
    public void SetGameMap(GameMap gameMap) {
        this.gameMap = gameMap;
    }

    // Set game manager object
    public void SetGameManagerObject(GameManagerObject gameManagerObject) {
        this.gameManagerObject = gameManagerObject;
    }

    // Set up first turn
    public void SetupFirstTurn() {
        InstantiateStartingPieces();
        resourceCounter.UpdateStartingResources(player.GetResources());
    }

    // Start turn
    public void StartTurn() {
        gameObject.SetActive(true);
        player.StartTurn();
        resourceCounter.UpdateAllResources(player.GetResources());
        foreach (KeyValuePair<GamePiece, GamePieceObject> pair in gamePieceObjects) {
            if (pair.Key.HasActions()) {
                pair.Value.ShowPieceDisabled();
            }
        }
    }

    // End turn
    public void EndTurn() {
        player.EndTurn();
        gameObject.SetActive(false);
        gameManagerObject.NextTurn();
    }

    // Instantiate player pieces
    public void InstantiateStartingPieces() {
        List<GamePiece> pieces = player.GetPieces();
        for (int i = 0; i < pieces.Count; i++) {
            GamePieceObject newPieceObject = GamePieceObject.InitializeFromGamePiece(pieces[i], gameMap.tilemap.transform, player.playerId);
            newPieceObject.SetPosition(gameMap);
            gamePieceObjects[pieces[i]] = newPieceObject;
        }
    }

    // Create piece
    public GamePieceObject CreatePiece(CardPiece cardPiece) {
        GamePieceObject newPieceObject = GamePieceObject.InitializeFromCardPiece(cardPiece, gameMap.tilemap.transform, player.playerId);
        return newPieceObject;
    }

    // Create piece object
    public GamePieceObject CreatePieceObject(GamePiece piece) {
        GamePieceObject newPieceObject = GamePieceObject.InitializeFromGamePiece(piece, gameMap.tilemap.transform, player.playerId);
        return newPieceObject;
    }

    // Create and position piece object
    public void CreateAndPositionPieceObject(GamePiece newPiece) {
        GamePieceObject newPieceObject = CreatePieceObject(newPiece);
        gamePieceObjects[newPiece] = newPieceObject;
        newPieceObject.SetPosition(gameMap);
        resourceCounter.UpdateAllResources(player.GetResources());
    }

    // Update piece lifebar
    public void UpdateGamePieceObjectLifebar(GamePiece piece) {
        gamePieceObjects[piece].SetLifebarCurrentHealth();

        // Destroy piece object if dead
        if (piece.GetCurrentHealth() == 0) {
            Destroy(gamePieceObjects[piece].gameObject);
            gamePieceObjects.Remove(piece);
        }
    }

    // Attack piece
    private void AttackPiece(Vector3Int targetTileCoords) {

        // Get targeted piece and player object
        GamePiece targetPiece = gameMap.GetHexPiece(Map.ConvertTileToHexCoords(targetTileCoords));
        PlayerObject playerObject = gameManagerObject.GetPlayerObject(targetPiece.GetPlayerId());

        // Attack piece
        GamePiece attackingPiece = player.GetSelectedPiece();
        player.AttackPiece(targetPiece);

        // Update the targeted piece lifebar
        playerObject.UpdateGamePieceObjectLifebar(targetPiece);

        // Set attacking piece as disabled
        GamePieceObject gamePieceObject = gamePieceObjects[attackingPiece];
        gamePieceObject.ShowPieceDisabled();
    }

    // Lost piece
    public void LostPiece(GamePiece piece) {
        gamePieceObjects.Remove(piece);
    }

    // Move piece
    private void MovePiece(Vector3Int targetTileCoords) {
        player.MovePiece(targetTileCoords);
        gamePieceObjects[player.GetSelectedPiece()].SetPosition(gameMap);
    }

    // Play piece at location
    public void PlayCardAtTile(CardPiece cardPiece, Vector3Int tileCoords) {
        GamePiece newPiece = player.PlayCardAtTile(cardPiece, tileCoords);
        CreateAndPositionPieceObject(newPiece);
    }

    // Play selected card at location
    public void PlaySelectedCardAtTile(Vector3Int tileCoords) {
        CardPieceDisplay selectedCard = player.GetSelectedCard();
        GamePiece newPiece = player.PlaySelectedCardAtTile(tileCoords);
        CreateAndPositionPieceObject(newPiece);
        Destroy(selectedCard.gameObject);
    }

    // Show current player's turn
    public IEnumerator ShowCurrentPlayerTurn(int playerId) {
        currentTurnText.transform.parent.gameObject.SetActive(true);
        currentTurnText.text = "Player " + (playerId + 1) + " Turn";
        yield return new WaitForSeconds(3);
        currentTurnText.transform.parent.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Awake() {
        gameObject.SetActive(false);
    }

    private void Start() {
        playerCamera.SetTilemap(gameMap.tilemap);
    }

    // Update is called once per frame
    void Update() {

        // Get map tile when click on map
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector3Int tileCoords = gameMap.GetMouseTileCoords(playerCamera.camera, Input.mousePosition);
            player.SetSelectedCard(null);
            Debug.Log("got tile coords: " + tileCoords);
            
            // Click on map tile
            if (tileCoords != null) {
                GameHex gameHex = gameMap.GetHexAtTileCoords(tileCoords);
                GamePiece piece = gameHex.GetPiece();
                Debug.Log("got piece: " + piece);

                // Piece is on tile
                if (piece != null) {
                    Debug.Log("piece id: " + piece.GetPlayerId() + " player id: " + player.playerId);

                    // Piece is current player's
                    if (piece.GetPlayerId() == player.playerId && player.isTurn) {
                        
                        // Piece is already selected
                        if (piece == player.GetSelectedPiece()) {
                            Debug.Log("piece is selected piece, clearing");
                            player.ClearSelectedPiece();
                        }
                        else if (piece.HasActions()) {
                            Debug.Log("piece can move, creating map");
                            player.SetSelectedPiece(piece);
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
            Vector3Int tileCoords = gameMap.GetMouseTileCoords(playerCamera.camera, Input.mousePosition);
            Debug.Log("got tile coords: " + tileCoords);

            // Click on map tile
            if (tileCoords != null) {

                // Play card from hand
                if (player.HasSelectedCard() && player.movementMap.PlayableToTile(tileCoords)) {
                    Debug.Log("playing selected card");
                    PlaySelectedCardAtTile(tileCoords);
                }

                // Move or attack piece
                if (player.HasSelectedPiece()) {
                    Debug.Log("player has selected piece");

                    // Move to tile
                    if (player.movementMap.MoveableToTile(tileCoords)) {
                        Debug.Log("moving selected piece");
                        MovePiece(tileCoords);
                    }

                    // Attack tile
                    else if (player.movementMap.AttackableTile(tileCoords)) {
                        Debug.Log("attacking piece");
                        AttackPiece(tileCoords);
                    }
                }
            }
        }
    }
}
