using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogMap : Map
{
    public Tile fogTile;
    private List<Vector3Int> visibleTiles = new List<Vector3Int>();

    // Constructor
    public FogMap() {
        CreateMap();
        SetFogTile();
    }

    // Constructor with map size
    public FogMap(int mapRadius) {
        this.mapRadius = mapRadius;
        newMapRadius = mapRadius;
        CreateMap();
        SetFogTile();
    }

    // Constructor
    public FogMap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        SetFogTile();
        DrawNewMap();
    }

    // Set fog tile
    private void SetFogTile() {
        defaultTile = Resources.Load<Tile>("Tiles/Tiles/Fog Tile");
        fogTile = Resources.Load<Tile>("Tiles/Tiles/Fog Tile");
    }

    // Set tilemap
    public new void SetTilemap(Tilemap tilemap) {
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        foreach (Vector3Int tileCoords in tileHexCoordsDict.Keys) {
            tilemap.SetTile(tileCoords, fogTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Get visible tiles
    public List<Vector3Int> GetVisibleTileCoordsList() {
        return new List<Vector3Int>(visibleTiles);
    }

    // Clears out painted tiles
    public void ClearPaintedTiles() {
        for (int i = 0; i < visibleTiles.Count; i++) {
            tilemap.SetTile(visibleTiles[i], fogTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Clears out visible tiles
    public void ClearVisibleTiles() {
        visibleTiles.Clear();
    }

    // Get visible tiles in sight range
    public void GetVisibleTilesForPieces(List<GamePiece> pieces) {
        for (int i = 0; i < pieces.Count; i++) {
            Vector3Int pieceHexCoords = pieces[i].GetGameHex().GetHexCoords();

            // Get all tiles within sight range
            List<Hex> visibleHexes = GetHexesInRange(pieceHexCoords, pieces[i].GetCard().sightRange, true);
            for (int j = 0; j < visibleHexes.Count; j++) {
                visibleTiles.Add(visibleHexes[j].GetTileCoords());
            }
        }
    }

    // Draw fog of war map
    public void DrawFogMap(List<GamePiece> pieces) {
        ClearPaintedTiles();
        CreateFogMap(pieces);
        PaintFogMap();
    }

    // Create fog of war map
    public void CreateFogMap(List<GamePiece> pieces) {
        ClearVisibleTiles();
        GetVisibleTilesForPieces(pieces);
    }

    // Paints fog of war map
    public void PaintFogMap() {
        for (int i = 0; i < visibleTiles.Count; i++) {
            tilemap.SetTile(visibleTiles[i], null);
        }
        tilemap.RefreshAllTiles();
    }
}
