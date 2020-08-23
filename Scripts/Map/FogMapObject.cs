using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogMapObject : MapObject
{
    // Fog map
    private FogMap fogMap;
    public Tile fogTile;

    // Set fog map
    public void SetFogMap(FogMap fogMap) {
        this.fogMap = fogMap;
    }

    // Paints initial fog of war map
    public void PaintInitialFogMap() {
        foreach (Vector3Int tileCoords in fogMap.GetTileHexCoordsDict().Keys) {
            tilemap.SetTile(tileCoords, fogTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Paints fog of war map
    public void PaintFogMap() {
        foreach (Vector3Int tileCoords in fogMap.GetVisibleTileCoords()) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.RefreshAllTiles();
    }

    // Clears out painted tiles
    public void ClearPaintedTiles() {
        foreach (Vector3Int tileCoords in fogMap.GetVisibleTileCoords()) {
            tilemap.SetTile(tileCoords, fogTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Draw fog of war map
    public void DrawFogMap(List<GamePiece> pieces) {
        ClearPaintedTiles();
        fogMap.CreateFogMap(pieces);
        PaintFogMap();
    }


}
