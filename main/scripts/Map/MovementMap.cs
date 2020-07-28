using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementMap : Map
{
    public Tile movementTile;
    public Tile attackTile;
    public Tile playableTile;
    Dictionary<Vector3Int, Tile> paintedTiles = new Dictionary<Vector3Int, Tile>();

    // Constructor
    public MovementMap() {
        CreateMap();
        SetMovementMapTiles();
    }

    // Constructor
    public MovementMap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        SetMovementMapTiles();
        CreateMap();
    }

    // Set movement and attack tiles
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

    // Get number of movement map tiles
    public int GetNumberMovementMapTiles() {
        return paintedTiles.Count;
    }

    // Clears out painted tiles
    public void ClearPaintedTiles() {
        foreach (Vector3Int tileCoords in paintedTiles.Keys) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.RefreshAllTiles();
        paintedTiles.Clear();
    }

    // Draw movement map
    public void DrawMovementMap(GamePiece piece, GameMap gameMap, FogOfWarMap fogOfWarMap) {
        ClearPaintedTiles();
        CreateMovementMap(piece, gameMap, fogOfWarMap);
        PaintMovementMap();
    }

    // Create movement map
    public void CreateMovementMap(GamePiece piece, GameMap gameMap, FogOfWarMap fogOfWarMap) {
        Vector3Int hexCoords = piece.GetGameHex().GetHexCoords();
        List<Vector3Int> visibleTileCoords = fogOfWarMap.GetVisibleTileCoords();

        if (piece.pieceType == PieceType.Unit) {
            Unit unit = (Unit)piece;
            int remainingSpeed = unit.GetRemainingSpeed();
            int range = unit.GetRange();
            int sightRange = unit.GetSightRange();

            // Get hexes in range of unit movement and set tiles
            List<GameHex> gameHexes = gameMap.GetGameHexesInRange(hexCoords, remainingSpeed + range);
            Debug.Log("game hexes length: " + gameHexes.Count);
            for (int i = 0; i < gameHexes.Count; i++) {
                GameHex gameHex = gameHexes[i];
                Debug.Log("game hex: " + gameHex);
                Debug.Log("game hex coords: " + gameHex.GetHexCoords());
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
    public void PaintMovementMap() {
        foreach (KeyValuePair<Vector3Int, Tile> pair in paintedTiles) {
            tilemap.SetTile(pair.Key, pair.Value);
        }
        tilemap.RefreshAllTiles();
    }

    // Draw playable map
    public void DrawPlayableMap(Vector3Int startTileCoords, GameMap gameMap) {
        ClearPaintedTiles();
        CreatePlayableMap(startTileCoords, gameMap);
        PaintMovementMap();
    }

    // Create playable map
    public void CreatePlayableMap(Vector3Int startTileCoords, GameMap gameMap) {

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
