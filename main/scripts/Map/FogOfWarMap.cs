using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarMap : Map
{
    public Tile fogTile;
    Dictionary<Vector3Int, Tile> visibleTiles = new Dictionary<Vector3Int, Tile>();

    // Constructor
    public FogOfWarMap() {
        CreateMap();
        SetFogTile();
    }

    // Constructor
    public FogOfWarMap(Tilemap tilemap) {
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

    // Get visible tiles
    public List<Vector3Int> GetVisibleTileCoords() {
        return new List<Vector3Int>(visibleTiles.Keys);
    }

    // Clears out visible tiles
    public void ClearVisibleTiles() {
        foreach (Vector3Int tileCoords in visibleTiles.Keys) {
            tilemap.SetTile(tileCoords, fogTile);
        }
        tilemap.RefreshAllTiles();
        visibleTiles.Clear();
    }

    // Draw fog of war map
    public void DrawFogOfWarMap(List<GamePiece> pieces) {
        ClearVisibleTiles();
        CreateFogOfWarMap(pieces);
        PaintFogOfWarMap();
    }

    // Create fog of war map
    public void CreateFogOfWarMap(List<GamePiece> pieces) {

        List<Hex> visibleHexes = new List<Hex>();
        for (int i = 0; i < pieces.Count; i++) {
            Vector3Int pieceHexCoords = pieces[i].GetGameHex().GetHexCoords();

            // Get all tiles within sight range
            List<Hex> visiblePieceHexes = GetHexesInRange(pieceHexCoords, pieces[i].GetCard().sightRange, true);
            for (int j = 0; j < visiblePieceHexes.Count; j++) {
                visibleHexes.Add(visiblePieceHexes[j]);
            }
        }

        // Set visible tiles to null
        for (int i = 0; i < visibleHexes.Count; i++) {
            Vector3Int tileCoords = ConvertHexToTileCoords(visibleHexes[i].GetHexCoords());
            visibleTiles[tileCoords] = null;
        }
    }

    // Paints fog of war map
    public void PaintFogOfWarMap() {
        foreach (KeyValuePair<Vector3Int, Tile> pair in visibleTiles) {
            tilemap.SetTile(pair.Key, pair.Value);
        }
        tilemap.RefreshAllTiles();
    }
}
