using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionMapObject : MapObject
{
    // Action map
    private ActionMap actionMap;

    // Set action map
    public void SetActionMap(ActionMap actionMap) {
        this.actionMap = actionMap;
    }

    // Paints movement map
    public void PaintActionMap() {
        foreach (KeyValuePair<Vector3Int, Tile> pair in actionMap.GetPaintedTiles()) {
            tilemap.SetTile(pair.Key, pair.Value);
        }
        tilemap.RefreshAllTiles();
    }

    // Clears out painted tiles
    public void ClearPaintedTiles() {
        foreach (Vector3Int tileCoords in actionMap.GetPaintedTiles().Keys) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.RefreshAllTiles();
    }

    // Draw playable map
    public void DrawPlayableMap(Vector3Int startTileCoords, GameMap gameMap) {
        ClearPaintedTiles();
        actionMap.CreatePlayableMap(startTileCoords, gameMap);
        PaintActionMap();
    }

    // Draw action map
    public void DrawActionMap(GamePiece piece, GameMap gameMap, FogMap fogOfWarMap) {
        ClearPaintedTiles();
        actionMap.CreateActionMap(piece, gameMap, fogOfWarMap);
        PaintActionMap();
    }
}
