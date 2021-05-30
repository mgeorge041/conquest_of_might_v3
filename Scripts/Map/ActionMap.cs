using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionMap : Map
{
    public Tile movementTile;
    public Tile attackTile;
    public Tile playableTile;
    public Dictionary<Vector3Int, Tile> paintedTiles { get; private set; }
    public List<Vector3Int> movementTiles { get; private set; }
    public List<Vector3Int> attackTiles { get; private set; }
    public List<Vector3Int> playableTiles { get; private set; }

    // Start
    public void Start()
    {
        paintedTiles = new Dictionary<Vector3Int, Tile>();
        movementTiles = new List<Vector3Int>();
        attackTiles = new List<Vector3Int>();
        playableTiles = new List<Vector3Int>();
    }

    // Constructor
    public ActionMap() 
    {
        paintedTiles = new Dictionary<Vector3Int, Tile>();
        movementTiles = new List<Vector3Int>();
        attackTiles = new List<Vector3Int>();
        playableTiles = new List<Vector3Int>();
    }

    // Get painted tiles
    public Dictionary<Vector3Int, Tile> GetPaintedTiles() {
        return paintedTiles;
    }

    // Set tiles
    private void SetMovementMapTiles() {
        movementTile = Resources.Load<Tile>(ENV.MOVE_TILE_RESOURCE_PATH);
        attackTile = Resources.Load<Tile>(ENV.ATTACK_TILE_RESOURCE_PATH);
        playableTile = Resources.Load<Tile>(ENV.PLAY_TILE_RESOURCE_PATH);
    }

    // Get tile at tile coords
    public Tile GetPaintedTileAtTileCoords(Vector3Int tileCoords) {
        if (paintedTiles.ContainsKey(tileCoords)) {
            return paintedTiles[tileCoords];
        }
        return null;
    }

    // Get whether tile is moveable
    public bool MoveableToTileAtTileCoords(Vector3Int tileCoords) {
        if (movementTiles.Contains(tileCoords)) {
            return true;
        }
        return false;
    }

    // Get whether tile is attackable
    public bool AttackableTileAtTileCoords(Vector3Int tileCoords) {
        if (attackTiles.Contains(tileCoords)) {
            return true;
        }
        return false;
    }

    // Get whether tile is playable
    public bool PlayableToTileAtTileCoords(Vector3Int tileCoords) {
        if (GetPaintedTileAtTileCoords(tileCoords) == playableTile) {
            return true;
        }
        return false;
    }

    // Clears out playable tiles
    public void ClearActionTiles() {
        foreach (Vector3Int tileCoords in paintedTiles.Keys.ToList()) {
            paintedTiles[tileCoords] = null;
        }
        movementTiles.Clear();
        attackTiles.Clear();
        playableTiles.Clear();
        tilemap.ClearAllTiles();
        tilemap.RefreshAllTiles();
    }

    // Set attack or movement tile in tilemap
    private void SetTileInTilemap(Tile tile, Vector3Int tileCoords, List<Vector3Int> tileList)
    {
        paintedTiles[tileCoords] = tile;
        tileList.Add(tileCoords);
        Debug.Log("Setting tile at " + tileCoords + " to tile: " + tile);
        tilemap.SetTile(tileCoords, tile);
    }

    // Create action map
    public void CreateActionMap(GamePiece piece, GameMap gameMap)
    {
        ClearActionTiles();
        if (piece == null)
        {
            return;
        }
        Vector3Int hexCoords = piece.gameHex.hexCoords;
        List<GameHex> gameHexes = gameMap.GetHexesInPieceRange(piece);

        if (piece.pieceType == PieceType.Unit)
        {
            Debug.Log("Setting action map for unit");
            Unit unit = (Unit)piece;
            int remainingSpeed = unit.remainingSpeed;
            Debug.Log("Remaining unit speed: " + remainingSpeed);
            Debug.Log("Game hex count: " + gameHexes.Count);

            // Get hexes in range of unit movement and set tiles
            for (int i = 0; i < gameHexes.Count; i++)
            {
                GameHex gameHex = gameHexes[i];
                int distance = Hex.GetDistanceHexCoords(hexCoords, gameHex.hexCoords);

                // Set tile to appropriate movement tile type
                if (!gameHex.HasPiece() && distance <= remainingSpeed)
                {
                    Debug.Log("Setting movement tile");
                    SetTileInTilemap(movementTile, gameHex.tileCoords, movementTiles);
                }
                else if (gameHex.HasPiece() && gameHex.piece.GetPlayerId() != unit.GetPlayerId())
                {
                    Debug.Log("Setting attack tile");
                    SetTileInTilemap(attackTile, gameHex.tileCoords, attackTiles);
                }
            }
        }
        tilemap.RefreshAllTiles();
        Debug.Log("(1, -1, 0) tile: " + tilemap.GetTile(Hex.HexToTileCoords(new Vector3Int(1, -1, 0))));
        Debug.Log("Set movement tiles : " + movementTiles.Count);
        Debug.Log("Set attack tiles : " + attackTiles.Count);
    }

    // Create action map
    public void CreateActionMap(GamePiece piece, GameMap gameMap, FogMap fogOfWarMap) {
        ClearActionTiles();
        if (piece == null) {
            return;
        }
        Vector3Int hexCoords = piece.gameHex.hexCoords;
        List<Vector3Int> visibleTileCoords = fogOfWarMap.GetVisibleTileCoords();

        if (piece.pieceType == PieceType.Unit) {
            Unit unit = (Unit)piece;
            int remainingSpeed = unit.remainingSpeed;
            int range = unit.range;

            // Get hexes in range of unit movement and set tiles
            List<GameHex> gameHexes = gameMap.GetHexesInRange(gameMap.hexCoordsDict, hexCoords, remainingSpeed + range);
            for (int i = 0; i < gameHexes.Count; i++) {
                GameHex gameHex = gameHexes[i];
                Vector3Int tileCoords = Hex.HexToTileCoords(gameHex.hexCoords);
                int distance = Hex.GetDistanceHexCoords(hexCoords, gameHex.hexCoords);
                
                // Set tile to appropriate movement tile type
                if (!gameHex.HasPiece()) {
                    if (distance <= remainingSpeed) {
                        paintedTiles[tileCoords] = movementTile;
                    }
                }
                else {
                    if (gameHex.piece.GetPlayerId() != unit.GetPlayerId() && visibleTileCoords.Contains(tileCoords)) {
                        paintedTiles[tileCoords] = attackTile;
                    }
                }
            }
        }
    }

    // Create playable map
    public void CreatePlayableMap(Vector3Int startTileCoords, GameMap gameMap) {

        ClearActionTiles();

        // Get hexes in range of castle
        Vector3Int startHexCoords = Hex.TileToHexCoords(startTileCoords);
        List<GameHex> gameHexes = gameMap.GetHexesInRange(gameMap.hexCoordsDict, startHexCoords, 1);

        for (int i = 0; i < gameHexes.Count; i++) {
            GameHex gameHex = gameHexes[i];
            Vector3Int tileCoords = Hex.HexToTileCoords(gameHex.hexCoords);

            // Set tile to appropriate movement tile type
            if (!gameHex.HasPiece()) {
                paintedTiles[tileCoords] = playableTile;
            }
        }
    }
}
