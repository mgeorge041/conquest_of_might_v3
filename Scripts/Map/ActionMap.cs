using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionMap
{
    public Tile movementTile;
    public Tile attackTile;
    public Tile playableTile;
    public Dictionary<Vector3Int, Tile> paintedTiles { get; private set; }
    public List<Vector3Int> movementTiles { get; private set; }
    public List<Vector3Int> attackTiles { get; private set; }
    public List<Vector3Int> playableTiles { get; private set; }

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

    // Clears out playable tiles
    public void ClearActionTiles() {
        foreach (Vector3Int tileCoords in paintedTiles.Keys.ToList()) {
            paintedTiles[tileCoords] = null;
        }
        movementTiles.Clear();
        attackTiles.Clear();
        playableTiles.Clear();
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
            Unit unit = (Unit)piece;
            int remainingSpeed = unit.remainingSpeed;

            // Get hexes in range of unit movement and set tiles
            for (int i = 0; i < gameHexes.Count; i++)
            {
                GameHex gameHex = gameHexes[i];
                int distance = Hex.GetDistanceHexCoords(hexCoords, gameHex.hexCoords);

                // Set tile to appropriate movement tile type
                if (!gameHex.HasPiece() && distance <= remainingSpeed)
                {
                    paintedTiles[gameHex.tileCoords] = movementTile;
                    movementTiles.Add(gameHex.tileCoords);
                }
                else if (gameHex.HasPiece() && gameHex.piece.GetPlayerId() != unit.GetPlayerId())
                {
                    paintedTiles[gameHex.tileCoords] = attackTile;
                    attackTiles.Add(gameHex.tileCoords);
                }
            }
        }
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
            List<GameHex> gameHexes = gameMap.GetHexesInRange<GameHex>(hexCoords, remainingSpeed + range);
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
        List<GameHex> gameHexes = gameMap.GetHexesInRange<GameHex>(startHexCoords, 1);

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
