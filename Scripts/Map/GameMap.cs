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

    // Pieces
    private Dictionary<int, List<GamePiece>> pieces = new Dictionary<int, List<GamePiece>>();

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

    // Constructor with map size
    public GameMap(int mapRadius) {
        this.mapRadius = mapRadius;
        newMapRadius = mapRadius;
        CreateMap();
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

    // Add a game piece to the map
    public bool AddPiece(GamePiece piece, Vector3Int tileCoords) {
        GameHex gameHex = GetHexAtTileCoords(tileCoords);
        if (!gameHex.HasPiece()) {

            // Associate hex with piece
            gameHex.SetPiece(piece);
            piece.SetGameHex(gameHex);

            // Update lists
            piece.GetPlayer().PlayPiece(piece);

            return true;
        }
        return false;
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
