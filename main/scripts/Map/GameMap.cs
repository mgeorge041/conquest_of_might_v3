using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GameMap : Map
{
    // Coords and game hexes
    private Dictionary<Vector3Int, GameHex> hexCoordsDict = new Dictionary<Vector3Int, GameHex>();
    private Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();

    // Players
    private List<Player> players;

    public Vector3Int[] neighborCoords =
    {
        new Vector3Int(-1, 1, 0),
        new Vector3Int(0, 1, -1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(0, -1, 1),
        new Vector3Int(-1, 0, 1)
    };

    // Constructor
    public GameMap() {
        CreateMap();
    }

    // Constructor
    public GameMap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        defaultTile = Resources.Load<Tile>("Tiles/Tiles/Grass");
        DrawNewMap();
    }

    // Constructor
    public GameMap(Tile defaultTile, Tilemap tilemap) {
        this.defaultTile = defaultTile;
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        DrawNewMap();
    }

    // Set players
    public void SetPlayers(List<Player> players) {
        this.players = players;
    }

    // Get hex from hex coords
    public new GameHex GetHexAtHexCoords(Vector3Int hexCoords) {
        if (hexCoordsDict.ContainsKey(hexCoords)) {
            return hexCoordsDict[hexCoords];
        }
        return null;
    }

    // Get hex from tile coords
    public new GameHex GetHexAtTileCoords(Vector3Int tileCoords) {
        if (tileHexCoordsDict.ContainsKey(tileCoords)) {
            return hexCoordsDict[tileHexCoordsDict[tileCoords]];
        }
        return null;
    }

    // Get hex from mouse position
    public GameHex GetHexAtMousePosition(Vector3 mousePosition, Camera playerCamera) {
        Vector3Int tileCoords = GetMouseTileCoords(playerCamera, mousePosition);
        return GetHexAtTileCoords(tileCoords);
    }

    // Get all editor tile outlines within a certain radius 
    public List<GameHex> GetGameHexesInRange(Vector3Int centerHexCoords, int radius) {
        List<GameHex> hexesInRange = new List<GameHex>();

        for (int i = -radius; i <= radius; i++) {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++) {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + centerHexCoords;
                if (hexCoords != centerHexCoords) {
                    if (hexCoordsDict.ContainsKey(hexCoords)) {
                        hexesInRange.Add(hexCoordsDict[hexCoords]);
                    }
                }
            }
        }
        return hexesInRange;
    }

    // Create a list of tiles for the map
    public new void CreateMap() {
        // Go to the larger of the new and old radii
        int radius = newMapRadius;
        if (newMapRadius < mapRadius) {
            radius = mapRadius;
        }

        // Draw the tilemap
        for (int i = -radius; i <= radius; i++) {
            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++) {
                // Get z coordinate
                int z = -i - j;
                Vector3Int newHexCoords = new Vector3Int(i, j, z);
                Vector3Int newTileCoords = HexToTileCoords(newHexCoords);
                int distance = GetDistanceToCenterHex(newHexCoords);

                // Only add tiles in size range
                if (distance <= newMapRadius && !hexCoordsDict.ContainsKey(newHexCoords)) {
                    hexCoordsDict.Add(newHexCoords, new GameHex(Resources.Load<TileData>("Tiles/Default"), newHexCoords));
                    tileHexCoordsDict.Add(newTileCoords, newHexCoords);
                }
            }
        }

        // Update new map size
        mapRadius = newMapRadius;
    }

    // Draw new map
    public new void DrawNewMap() {
        CreateMap();
        DrawMap();
    }

    // Draw the map
    public new void DrawMap() {
        List<Vector3Int> removeHexCoords = new List<Vector3Int>();
        List<Vector3Int> removeTileCoords = new List<Vector3Int>();

        // Draw the map
        foreach (KeyValuePair<Vector3Int, Vector3Int> pair in tileHexCoordsDict) {
            Vector3Int tileCoords = pair.Key;
            Vector3Int hexCoords = pair.Value;
            int distance = GetDistanceToCenterHex(hexCoords);

            // Remove tiles outside of new map radius
            if (distance > mapRadius) {
                tilemap.SetTile(tileCoords, null);
                if (tileHexCoordsDict.ContainsKey(tileCoords)) {
                    removeHexCoords.Add(hexCoords);
                    removeTileCoords.Add(tileCoords);
                }
            }
            else {
                Tile currentTile = (Tile)tilemap.GetTile(tileCoords);
                if (currentTile != defaultTile && currentTile != null) {
                    continue;
                }
                tilemap.SetTile(tileCoords, defaultTile);
            }
        }

        // Remove the outer tiles
        for (int i = 0; i < removeHexCoords.Count; i++) {
            hexCoordsDict.Remove(removeHexCoords[i]);
        }
        removeHexCoords.Clear();

        for (int i = 0; i < removeTileCoords.Count; i++) {
            tileHexCoordsDict.Remove(removeTileCoords[i]);
        }
        removeTileCoords.Clear();

        // Set new map size
        tilemap.CompressBounds();
        tilemap.RefreshAllTiles();
    }

    // Add and position piece object
    public bool AddAndPositionPieceObject(GamePieceObject pieceObject, Vector3Int tileCoords) {
        GamePiece piece = pieceObject.GetPiece();
        bool addedPiece = AddPiece(piece, tileCoords);
        if (addedPiece) {
            PositionPieceObject(pieceObject, tileCoords);
        }
        return addedPiece;
    }

    // Add a game piece to the map
    public bool AddPiece(GamePiece gamePiece, Vector3Int tileCoords) {
        GameHex gameHex = GetHexAtTileCoords(tileCoords);
        if (!gameHex.HasPiece()) {
            gameHex.SetPiece(gamePiece);
            gamePiece.SetGameHex(gameHex);
            return true;
        }
        return false;
    }

    // Positions a piece on the map
    private void PositionPieceObject(GamePieceObject pieceObject, Vector3Int tileCoords) {
        Vector3 tilePosition = tileGrid.CellToWorld(tileCoords);
        pieceObject.transform.position = new Vector3(tilePosition.x, tilePosition.y, -1);
    }

    // Moves a piece on the map
    public void MovePiece(Unit unit, Vector3Int targetTileCoords) {
        GameHex currentHex = unit.GetGameHex();
        GameHex targetHex = GetHexAtTileCoords(targetTileCoords);

        // Get distance traveled and update new hex
        int distance = GetDistanceHexes(currentHex, targetHex);
        unit.DecreaseSpeed(distance);
        unit.SetGameHex(targetHex);
        targetHex.SetPiece(unit);
        currentHex.ClearPiece();
    }

    // Attacks a piece on the map
    public void AttackPiece(GamePiece piece, GamePiece targetPiece) {
        bool isDead = targetPiece.TakeDamage(piece.GetMight());
        if (isDead) {
            int playerId = targetPiece.GetPlayerId();
            players[playerId].LostPiece(targetPiece);
        }
    }

    // Determine if hex has piece
    public bool HexHasPiece(Vector3Int hexCoords) {
        return GetHexAtHexCoords(hexCoords).HasPiece();
    }

    // Get hex piece
    public GamePiece GetHexPiece(Vector3Int hexCoords) {
        return GetHexAtHexCoords(hexCoords).GetPiece();
    }

    // Determine if can play on hex
    public bool CanPlayOnHex(Vector3Int tileCoords) {
        return !GetHexAtTileCoords(tileCoords).HasPiece();
    }
}
