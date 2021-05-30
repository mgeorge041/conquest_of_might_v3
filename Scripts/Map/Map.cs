using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    // Map variables
    public Tile defaultTile;
    public Tile highlightTile;
    public Tilemap tilemap;
    public Grid tileGrid;
    public Camera playerCamera;
    protected bool isOverTilemap;

    // Tilemap sizing
    protected int newMapRadius;
    public int mapRadius { get; set; }

    // Tilemap variables
    public Dictionary<Vector3Int, Hex> hexCoordsDict { get; private set; }
    protected Dictionary<Vector3Int, Vector3Int> tileHexCoordsDict;
    protected List<Vector3Int> removedTiles;
    protected List<Vector3Int> highlightTileCoords;

    // Initialize
    public void Initialize()
    {
        InitializeVariables<Hex>();
        CreateMap();
        PaintMap();
    }

    // Initialize variables
    protected void InitializeVariables<T>() where T : Hex
    {
        isOverTilemap = false;

        // Tilemap sizing
        newMapRadius = 5;
        mapRadius = 5;

        // Tilemap variables
        hexCoordsDict = new Dictionary<Vector3Int, Hex>();
        tileHexCoordsDict = new Dictionary<Vector3Int, Vector3Int>();
        removedTiles = new List<Vector3Int>();
        highlightTileCoords = new List<Vector3Int>();
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
    public Hex GetHexAtHexCoords(Vector3Int hexCoords)
    {
        if (hexCoordsDict.ContainsKey(hexCoords)) {
            return hexCoordsDict[hexCoords];
        }
        return null;
    }

    // Get hex from tile coords
    public Hex GetHexAtTileCoords(Vector3Int tileCoords) 
    {
        if (tileHexCoordsDict.ContainsKey(tileCoords)) {
            return hexCoordsDict[tileHexCoordsDict[tileCoords]];
        }
        return null;
    }

    // Get all hexes within a certain radius 
    public List<T> GetHexesInRange<T>(Dictionary<Vector3Int, T> hexCoordsDict, Vector3Int centerHexCoords, int radius, bool includeCenter = false) {
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
                        hexesInRange.Add((T)Convert.ChangeType(hexCoordsDict[hexCoords], typeof(T)));
                    }
                    else if (includeCenter) {
                        hexesInRange.Add((T)Convert.ChangeType(hexCoordsDict[hexCoords], typeof(T)));
                    }
                }
            }
        }
        return hexesInRange;
    }

    // Get all tile coords within a certain radius
    public List<Vector3Int> GetTileCoordsInRange(Vector3Int centerHexCoords, int radius)
    {
        List<Vector3Int> tileCoordsInRange = new List<Vector3Int>();

        for (int i = -radius; i <= radius; i++)
        {

            // Get upper and lower bounds for map columns
            int lowerBound = Math.Max(-i - radius, -radius);
            int upperBound = Math.Min(radius, -i + radius);

            for (int j = lowerBound; j <= upperBound; j++)
            {
                int z = -i - j;
                Vector3Int hexCoords = new Vector3Int(i, j, z) + centerHexCoords;
                if (hexCoords != centerHexCoords)
                {
                    tileCoordsInRange.Add(hexCoords);
                }
            }
        }
        return tileCoordsInRange;
    }

    // Highlight all hexes with a certain radius
    public void HighlightHexesInRange<T>(Dictionary<Vector3Int, T> hexCoordsDict, Vector3Int centerHexCoords, int radius) where T : Hex
    {
        List<T> hexesInRange = GetHexesInRange(hexCoordsDict, centerHexCoords, radius);
        foreach (T hex in hexesInRange)
        {
            tilemap.SetTile(hex.tileCoords, highlightTile);
            highlightTileCoords.Add(hex.tileCoords);
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
    public void UpdateMapToRadius(int newRadius)
    {
        newMapRadius = newRadius;
        CreateMap();
        DrawMap();
    }

    // Updates the map to a new size
    public void UpdateMapToRadius(float newRadius)
    {
        newMapRadius = (int)newRadius;
        CreateMap();
        DrawMap();
    }

    // Create a list of tile for the map
    public void CreateMap()
    {
        CreateMap(hexCoordsDict);
    }

    // Create a list of tile for the map
    public void CreateMap<T>(Dictionary<Vector3Int, T> hexCoordsDict) where T : Hex
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
                int distance = Hex.GetDistanceToCenterHex(newHexCoords);

                // Only add tiles in size range
                if (distance <= newMapRadius && !hexCoordsDict.ContainsKey(newHexCoords))
                {
                    hexCoordsDict.Add(newHexCoords, (T)Hex.CreateHex<T>(Resources.Load<TileData>(ENV.DEFAULT_TILE_RESOURCE_PATH), newHexCoords));
                    tileHexCoordsDict.Add(newTileCoords, newHexCoords);
                }
                else if (distance > newMapRadius)
                {
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
    public Vector3 TileToWorldCoords(Vector3Int tileCoords)
    {
        return tileGrid.CellToWorld(tileCoords);
    }

    // Get world coordinates from hex coordinates
    public Vector3 HexToWorldCoords(Vector3Int hexCoords)
    {
        return tileGrid.CellToWorld(Hex.HexToTileCoords(hexCoords));
    }

    // Get tile coordinates from world coordinates
    public Vector3Int WorldToTileCoords(Vector3 worldCoords)
    {
        return tileGrid.WorldToCell(worldCoords);
    }

    // Get hex coordinates from world coordinates
    public Vector3Int WorldToHexCoords(Vector3 worldCoords)
    {
        return Hex.TileToHexCoords(tileGrid.WorldToCell(worldCoords));
    }

    // Get hex from world position
    public Hex GetWorldPositionHex(Vector3 worldPosition)
    {
        Vector3Int tileCoords = WorldToTileCoords(worldPosition);
        return GetHexAtTileCoords(tileCoords);
    }

    // Get whether over a tilemap tile
    public bool MouseOverTile()
    {
        return isOverTilemap;
    }

    // Mouse clicks
    public void OnMouseDown()
    {
        //Debug.Log("clicked mouse on tile: " + WorldToHexCoords(playerCamera.ScreenToWorldPoint(Input.mousePosition)));
        Debug.Log("clicked mouse at mouse position: " + Input.mousePosition);
        //Debug.Log("clicked mouse at world point: " + playerCamera.ScreenToWorldPoint(Input.mousePosition));
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
        Initialize();
    }
}