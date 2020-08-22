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
    Dictionary<Vector3Int, Tile> paintedTiles = new Dictionary<Vector3Int, Tile>();

    // Constructor
    public ActionMap() {
        CreateMap();
        SetMovementMapTiles();
    }

    // Constructor with map size
    public ActionMap(int mapRadius) {
        this.mapRadius = mapRadius;
        newMapRadius = mapRadius;
        CreateMap();
        SetMovementMapTiles();
    }

    // Constructor
    public ActionMap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        SetMovementMapTiles();
        CreateMap();
    }

    // Set tiles
    private void SetMovementMapTiles() {
        defaultTile = Resources.Load<Tile>("Tiles/Tiles/Attack Tile");
        movementTile = Resources.Load<Tile>("Tiles/Tiles/Movement Tile");
        attackTile = Resources.Load<Tile>("Tiles/Tiles/Attack Tile");
        playableTile = Resources.Load<Tile>("Tiles/Tiles/Playable Tile");
    }

    // Get tile at tile coords
    public Tile GetPaintedTileAtTileCoords(Vector3Int tileCoords) {
        if (paintedTiles.ContainsKey(tileCoords)) {
            return paintedTiles[tileCoords];
        }
        return null;
    }

    // Get whether tile is moveable
    public bool MoveableToTile(Vector3Int tileCoords) {
        if (GetPaintedTileAtTileCoords(tileCoords) == movementTile) {
            return true;
        }
        return false;
    }

    // Get whether tile is attackable
    public bool AttackableTile(Vector3Int tileCoords) {
        if (GetPaintedTileAtTileCoords(tileCoords) == attackTile) {
            return true;
        }
        return false;
    }

    // Get whether tile is playable
    public bool PlayableToTile(Vector3Int tileCoords) {
        if (GetPaintedTileAtTileCoords(tileCoords) == playableTile) {
            return true;
        }
        return false;
    }

    // Get number of action map tiles
    public int GetNumberMovementMapTiles() {
        return paintedTiles.Count;
    }

    // Clears out painted tiles
    public void ClearPaintedTiles() {
        foreach (Vector3Int tileCoords in paintedTiles.Keys) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.RefreshAllTiles();
    }

    // Clears out playable tiles
    public void ClearActionTiles() {
        foreach (Vector3Int tileCoords in paintedTiles.Keys.ToList()) {
            paintedTiles[tileCoords] = null;
        }
    }

    // Draw action map
    public void DrawActionMap(GamePiece piece, GameMap gameMap, FogMap fogOfWarMap) {
        ClearPaintedTiles();
        CreateActionMap(piece, gameMap, fogOfWarMap);
        PaintActionMap();
    }

    // Create action map
    public void CreateActionMap(GamePiece piece, GameMap gameMap, FogMap fogOfWarMap) {
        ClearActionTiles();
        if (piece == null) {
            return;
        }
        Vector3Int hexCoords = piece.GetGameHex().GetHexCoords();
        List<Vector3Int> visibleTileCoords = fogOfWarMap.GetVisibleTileCoordsList();

        if (piece.pieceType == PieceType.Unit) {
            Unit unit = (Unit)piece;
            int remainingSpeed = unit.GetRemainingSpeed();
            int range = unit.GetRange();

            // Get hexes in range of unit movement and set tiles
            List<GameHex> gameHexes = gameMap.GetGameHexesInRange(hexCoords, remainingSpeed + range);
            for (int i = 0; i < gameHexes.Count; i++) {
                GameHex gameHex = gameHexes[i];
                Vector3Int tileCoords = HexToTileCoords(gameHex.GetHexCoords());
                int distance = gameMap.GetDistanceHexCoords(hexCoords, gameHex.GetHexCoords());
                
                // Set tile to appropriate movement tile type
                if (!gameHex.HasPiece()) {
                    if (distance <= remainingSpeed) {
                        paintedTiles[tileCoords] = movementTile;
                    }
                }
                else {
                    if (gameHex.GetPiece().GetPlayerId() != unit.GetPlayerId() && visibleTileCoords.Contains(tileCoords)) {
                        paintedTiles[tileCoords] = attackTile;
                    }
                }
            }
        }
    }

    // Paints movement map
    public void PaintActionMap() {
        foreach (KeyValuePair<Vector3Int, Tile> pair in paintedTiles) {
            tilemap.SetTile(pair.Key, pair.Value);
        }
        tilemap.RefreshAllTiles();
    }

    // Draw playable map
    public void DrawPlayableMap(Vector3Int startTileCoords, GameMap gameMap) {
        ClearPaintedTiles();
        CreatePlayableMap(startTileCoords, gameMap);
        PaintActionMap();
    }

    // Create playable map
    public void CreatePlayableMap(Vector3Int startTileCoords, GameMap gameMap) {

        ClearActionTiles();

        // Get hexes in range of castle
        Vector3Int startHexCoords = ConvertTileToHexCoords(startTileCoords);
        List<GameHex> gameHexes = gameMap.GetGameHexesInRange(startHexCoords, 1);

        for (int i = 0; i < gameHexes.Count; i++) {
            GameHex gameHex = gameHexes[i];
            Vector3Int tileCoords = ConvertHexToTileCoords(gameHex.GetHexCoords());

            // Set tile to appropriate movement tile type
            if (!gameHex.HasPiece()) {
                paintedTiles[tileCoords] = playableTile;
            }
        }
    }
}
