using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map
{
    // Tilemap sizing
    protected int newMapRadius = GameSetupData.mapRadius;
    protected int mapRadius = GameSetupData.mapRadius;

    // Tilemap variables
    private Dictionary<Vector3Int, Hex> hexCoordsDict = new Dictionary<Vector3Int, Hex>();
    protected Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();
    protected List<Vector3Int> removedTiles = new List<Vector3Int>();

    // Constructor
    public Map() { }

    // Get map radius 
    public int GetMapRadius() {
        return mapRadius;
    }

    // Set new map
    public void SetNewMapRadius(int mapRadius) {
        newMapRadius = mapRadius;
    }

    // Get hex coords dict
    public Dictionary<Vector3Int, Hex> GetHexCoordsDict() {
        return hexCoordsDict;
    }

    // Get whether has tile
    public bool HasTile(Vector3Int tileCoords) {
        if (tileHexCoordsDict.ContainsKey(tileCoords)) {
            return true;
        }
        return false;
    }

    // Get tile hex coords dict
    public Dictionary<Vector3Int, Vector3Int> GetTileHexCoordsDict() {
        return tileHexCoordsDict;
    }

    // Get removed tiles
    public List<Vector3Int> GetRemovedTiles() {
        return removedTiles;
    }

    // Get hex from hex coords
    public Hex GetHexAtHexCoords(Vector3Int hexCoords) {
        if (hexCoordsDict.ContainsKey(hexCoords)) {
            return hexCoordsDict[hexCoords];
        }
        return null;
    }

    // Get hex from tile coords
    public Hex GetHexAtTileCoords(Vector3Int tileCoords) {
        if (tileHexCoordsDict.ContainsKey(tileCoords)) {
            return hexCoordsDict[tileHexCoordsDict[tileCoords]];
        }
        return null;
    }

    // Get hex coords from tile coords
    public Vector3Int GetHexCoordsFromTileCoords(Vector3Int tileCoords) {
        return tileHexCoordsDict[tileCoords];
    }

    // Get distance between two hexes
    public int GetDistanceHexes(Hex hex1, Hex hex2) {
        return GetDistanceHexCoords(hex1.GetHexCoords(), hex2.GetHexCoords());
    }

    // Get distance between two hex coordinates
    public int GetDistanceHexCoords(Vector3Int hexCoords1, Vector3Int hexCoords2)
    {
        int x = Math.Abs(hexCoords1.x - hexCoords2.x);
        int y = Math.Abs(hexCoords1.y - hexCoords2.y);
        int z = Math.Abs(hexCoords1.z - hexCoords2.z);

        int distance = (x + y + z) / 2;

        return distance;
    }

    // Get the distance between the center hex and hex coordinates
    public int GetDistanceToCenterHex(Vector3Int hexCoords)
    {
        return GetDistanceHexCoords(hexCoords, Vector3Int.zero);
    }

    // Converts hex coordinates to tilemap coordinates
    public Vector3Int HexToTileCoords(Vector3Int hexCoords)
    {
        int x = hexCoords.z + (hexCoords.x - (hexCoords.x & 1)) / 2;
        int y = hexCoords.x;

        return new Vector3Int(x, y, 0);
    }

    // Converts tilemap coordinates to hex coordinates
    public Vector3Int TileToHexCoords(Vector3Int tileCoords)
    {
        int x = tileCoords.y;
        int y = -tileCoords.x - (tileCoords.y + (tileCoords.y & 1)) / 2;
        int z = -x - y;

        return new Vector3Int(x, y, z);
    }

    // Converts hex coordinates to tilemap coordinates
    public static Vector3Int ConvertHexToTileCoords(Vector3Int hexCoords)
    {
        int x = hexCoords.z + (hexCoords.x - (hexCoords.x & 1)) / 2;
        int y = hexCoords.x;

        return new Vector3Int(x, y, 0);
    }

    // Converts tilemap coordinates to hex coordinates
    public static Vector3Int ConvertTileToHexCoords(Vector3Int tileCoords)
    {
        int x = tileCoords.y;
        int y = -tileCoords.x - (tileCoords.y + (tileCoords.y & 1)) / 2;
        int z = -x - y;

        return new Vector3Int(x, y, z);
    }

    // Get all hexes within a certain radius 
    public List<Hex> GetHexesInRange(Vector3Int centerHexCoords, int radius, bool includeCenter = false) {
        List<Hex> hexesInRange = new List<Hex>();

        for (int i = -radius; i <= radius; i++) {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++) {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + centerHexCoords;
                if (hexCoordsDict.ContainsKey(hexCoords)) {
                    if (hexCoords != centerHexCoords && !includeCenter) {
                        hexesInRange.Add(hexCoordsDict[hexCoords]);
                    }
                    else if (includeCenter) {
                        hexesInRange.Add(hexCoordsDict[hexCoords]);
                    }
                }
            }
        }
        return hexesInRange;
    }

    // Get all hex coordinates within a certain radius
    public List<Vector3Int> GetHexCoordsInRange(Vector3Int centerHexCoords, int radius) {
        List<Vector3Int> hexCoordsInRange = new List<Vector3Int>();

        for (int i = -radius; i <= radius; i++) {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++) {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + centerHexCoords;
                if (hexCoords != centerHexCoords) {
                    hexCoordsInRange.Add(hexCoords);
                }
            }
        }
        return hexCoordsInRange;
    }

    // Updates the map to a new size
    public void UpdateMapToRadius(int newRadius) {
        SetNewMapRadius(newRadius);
        CreateMap();
    }

    // Create a list of tiles for the map
    public void CreateMap()
    {
        removedTiles.Clear();

        // Go to the larger of the new and old radii
        int radius = newMapRadius;
        if (newMapRadius < mapRadius)
        {
            radius = mapRadius;
        }

        // Draw the tilemap
        for (int i = -radius; i <= radius; i++)
        {
            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                // Get z coordinate
                int z = -i - j;
                Vector3Int newHexCoords = new Vector3Int(i, j, z);
                Vector3Int newTileCoords = HexToTileCoords(newHexCoords);
                int distance = GetDistanceToCenterHex(newHexCoords);

                // Only add tiles in size range
                if (distance <= newMapRadius && !hexCoordsDict.ContainsKey(newHexCoords))
                {
                    hexCoordsDict.Add(newHexCoords, new Hex(Resources.Load<TileData>("Tiles/Default"), newHexCoords));
                    tileHexCoordsDict.Add(newTileCoords, newHexCoords);
                }
                else if (distance > newMapRadius) {
                    removedTiles.Add(newTileCoords);
                    hexCoordsDict.Remove(newHexCoords);
                    tileHexCoordsDict.Remove(newTileCoords);
                }
            }
        }

        // Update new map radius
        mapRadius = newMapRadius;
    }
}