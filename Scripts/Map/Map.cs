using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Map<T> : MonoBehaviour where T : Hex
{
    // Map variables
    public Tile defaultTile;
    public Tile highlightTile;
    public Tilemap tilemap;
    public Grid tileGrid;
    public Camera playerCamera;
    protected bool isOverTilemap = false;

    // Tilemap sizing
    protected int newMapRadius = 5;
    protected int mapRadius = 5;

    // Tilemap variables
    protected Dictionary<Vector3Int, T> hexCoordsDict = new Dictionary<Vector3Int, T>();
    protected Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();
    protected List<Vector3Int> removedTiles = new List<Vector3Int>();
    protected List<Vector3Int> highlightTileCoords = new List<Vector3Int>();

    // Get map radius 
    public int GetMapRadius() {
        return mapRadius;
    }

    // Set new map
    public void SetNewMapRadius(int mapRadius) {
        newMapRadius = mapRadius;
    }

    // Get hex coords dict
    public Dictionary<Vector3Int, T> GetHexCoordsDict() {
        return hexCoordsDict;
    }

    // Get tile hex coords dict
    public Dictionary<Vector3Int, Vector3Int> GetTileHexCoordsDict()
    {
        return tileHexCoordsDict;
    }

    // Get whether has tile coords
    public bool HasTileCoords(Vector3Int tileCoords) {
        if (tileHexCoordsDict.ContainsKey(tileCoords)) {
            return true;
        }
        return false;
    }

    // Get hex from hex coords
    public T GetHexAtHexCoords(Vector3Int hexCoords) {
        if (hexCoordsDict.ContainsKey(hexCoords)) {
            return hexCoordsDict[hexCoords];
        }
        return null;
    }

    // Get hex from tile coords
    public T GetHexAtTileCoords(Vector3Int tileCoords) {
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

    // Get all hexes within a certain radius 
    public List<T> GetHexesInRange(Vector3Int centerHexCoords, int radius, bool includeCenter = false) {
        List<T> hexesInRange = new List<T>();

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

    // Highlight all hexes with a certain radius
    public void HighlightHexesInRange(Vector3Int centerHexCoords, int radius)
    {
        List<T> hexesInRange = GetHexesInRange(centerHexCoords, radius);
        foreach (T hex in hexesInRange)
        {
            tilemap.SetTile(hex.GetTileCoords(), highlightTile);
            highlightTileCoords.Add(hex.GetTileCoords());
        }
    }

    // Dehighlight hexes
    public void DehighlightHexes()
    {
        foreach (Vector3Int tileCoords in highlightTileCoords)
        {
            tilemap.SetTile(tileCoords, defaultTile);
        }
        highlightTileCoords.Clear();
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

    // Get removed tiles
    public List<Vector3Int> GetRemovedTiles()
    {
        return removedTiles;
    }

    // Updates the map to a new size
    public void UpdateMapToRadius(int newRadius) {
        SetNewMapRadius(newRadius);
        CreateMap();
        DrawMap();
    }

    // Updates the map to a new size
    public void UpdateMapToRadius(float newRadius)
    {
        SetNewMapRadius((int)newRadius);
        CreateMap();
        DrawMap();
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
                Vector3Int newTileCoords = Hex.HexToTileCoords(newHexCoords);
                int distance = GetDistanceToCenterHex(newHexCoords);

                // Only add tiles in size range
                if (distance <= newMapRadius && !hexCoordsDict.ContainsKey(newHexCoords))
                {
                    hexCoordsDict.Add(newHexCoords, (T)Convert.ChangeType(new Hex(Resources.Load<TileData>("Tiles/Default"), newHexCoords), typeof(T)));
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

    // Set default tile
    public void SetDefaultTile(Tile defaultTile)
    {
        this.defaultTile = defaultTile;
    }

    // Draw the map
    public void DrawMap()
    {
        ClearMap();
        PaintMap();
    }

    // Paint map tiles
    public void PaintMap()
    {
        foreach (Vector3Int tileCoords in tileHexCoordsDict.Keys)
        {
            tilemap.SetTile(tileCoords, defaultTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Clear map tiles and dicts
    public void ClearMap()
    {
        foreach (Vector3Int tileCoords in removedTiles)
        {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.CompressBounds();
    }

    // Clear map tile at coordinates
    public void ClearMapTile(Vector3Int tileCoords)
    {
        tilemap.SetTile(tileCoords, defaultTile);
        tilemap.RefreshTile(tileCoords);
    }

    // Get world coordinates from tilemap coordinates
    public Vector3 GetWorldCoordsFromTileCoords(Vector3Int tileCoords)
    {
        return tileGrid.CellToWorld(tileCoords);
    }

    // Get tilemap coordinates from mouse position
    public Vector3Int GetMouseTileCoords(Camera playerCamera, Vector3 mousePosition)
    {
        return tileGrid.WorldToCell(playerCamera.ScreenToWorldPoint(mousePosition));
    }

    // Get hex coordinates from mouse position
    public Vector3Int GetMouseHexCoords(Camera playerCamera, Vector3 mousePosition)
    {
        Vector3Int tileCoords = GetMouseTileCoords(playerCamera, mousePosition);
        return GetHexCoordsFromTileCoords(tileCoords);
    }

    // Get whether over a tilemap tile
    public bool MouseOverTile()
    {
        return isOverTilemap;
    }

    // Mouse clicks
    public void OnMouseDown()
    {
        Debug.Log("clicked mouse on tile: " + GetMouseHexCoords(playerCamera, Input.mousePosition));
    }

    // Mouse enters
    public void OnMouseEnter()
    {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        isOverTilemap = true;
    }

    // Mouse exits
    public void OnMouseExit()
    {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        isOverTilemap = false;
    }

    // Start
    void Start()
    {
        CreateMap();
        PaintMap();
    }
}