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
    public Tile defaultTile;
    public Tilemap tilemap;
    public Grid tileGrid;
    protected bool isOverTilemap = false;
    private Dictionary<Vector3Int, Hex> hexCoordsDict = new Dictionary<Vector3Int, Hex>();
    protected Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();

    // Constructor
    public Map() { }

    // Constructor
    public Map(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        defaultTile = Resources.Load<Tile>("Tiles/Default");
        DrawNewMap();
    }

    // Constructor
    public Map(Tile defaultTile, Tilemap tilemap) {
        this.defaultTile = defaultTile;
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        DrawNewMap();
    }

    // Get map radius 
    public int GetMapRadius() {
        return mapRadius;
    }

    // Set default tile
    public void SetDefaultTile(Tile defaultTile) {
        this.defaultTile = defaultTile;
    }

    // Set tilemap
    public void SetTilemap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
    }

    // Set new map
    public void SetNewMapRadius(int mapRadius) {
        newMapRadius = mapRadius;
    }

    // Set new map radius
    public void SetMapRadius(int mapRadius) {
        this.mapRadius = mapRadius;
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

    // Get world coordinates from tilemap coordinates
    public Vector3 GetWorldCoordsFromTileCoords(Vector3Int tileCoords) {
        return tileGrid.CellToWorld(tileCoords);
    }

    // Get tilemap coordinates from mouse position
    public Vector3Int GetMouseTileCoords(Camera playerCamera, Vector3 mousePosition) {
        return tileGrid.WorldToCell(playerCamera.ScreenToWorldPoint(mousePosition));
    }

    // Get hex coordinates from mouse position
    public Vector3Int GetMouseHexCoords(Camera playerCamera, Vector3 mousePosition) {
        Vector3Int tileCoords = GetMouseTileCoords(playerCamera, mousePosition);
        return GetHexCoordsFromTileCoords(tileCoords);
    }

    // Get whether mouse is over tile
    public bool MouseOverTile(Camera playerCamera, Vector3 mousePosition) {
        if (tileHexCoordsDict.ContainsKey(GetMouseTileCoords(playerCamera, mousePosition))) {
            return true;
        }
        return false;
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

    // Updates the map and redraws
    public void RedrawMapAtSize(int newSize) {
        SetNewMapRadius(newSize);
        DrawNewMap();
    }

    // Create a list of tiles for the map
    public void CreateMap()
    {
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
            }
        }

        // Update new map radius
        mapRadius = newMapRadius;
    }

    // Clear map tiles and dicts
    public void ClearMap() {
        foreach (Vector3Int tileCoords in tileHexCoordsDict.Keys) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.CompressBounds();
        tileHexCoordsDict.Clear();
        hexCoordsDict.Clear();
    }

    // Clear map tile at coordinates
    public void ClearMapTile(Vector3Int tileCoords) {
        tilemap.SetTile(tileCoords, defaultTile);
        tilemap.RefreshTile(tileCoords);
    }

    // Clear map tiles
    public void ClearMapTiles() {
        foreach (Vector3Int tileCoords in tileHexCoordsDict.Keys) {
            tilemap.SetTile(tileCoords, null);
        }
    }

    // Draw new map
    public void DrawNewMap() {
        CreateMap();
        DrawMap();
    }

    // Draw the map
    public void DrawMap()
    {
        List<Vector3Int> removeHexCoords = new List<Vector3Int>();
        List<Vector3Int> removeTileCoords = new List<Vector3Int>();

        // Draw the map
        foreach (KeyValuePair<Vector3Int, Vector3Int> pair in tileHexCoordsDict)
        {
            Vector3Int tileCoords = pair.Key;
            Vector3Int hexCoords = pair.Value;
            int distance = GetDistanceToCenterHex(hexCoords);

            // Remove tiles outside of new map radius
            if (distance > mapRadius)
            {
                tilemap.SetTile(tileCoords, null);
                if (tileHexCoordsDict.ContainsKey(tileCoords))
                {
                    removeHexCoords.Add(hexCoords);
                    removeTileCoords.Add(tileCoords);
                }
            }
            else
            {
                Tile currentTile = (Tile)tilemap.GetTile(tileCoords);
                if (currentTile != defaultTile && currentTile != null)
                {
                    continue;
                }
                tilemap.SetTile(tileCoords, defaultTile);
            }
        }

        // Remove the outer tiles
        for (int i = 0; i < removeHexCoords.Count; i++)
        {
            hexCoordsDict.Remove(removeHexCoords[i]);
        }
        removeHexCoords.Clear();

        for (int i = 0; i < removeTileCoords.Count; i++)
        {
            tileHexCoordsDict.Remove(removeTileCoords[i]);
        }
        removeTileCoords.Clear();

        // Set new map size
        tilemap.CompressBounds();
        tilemap.RefreshAllTiles();
    }
}