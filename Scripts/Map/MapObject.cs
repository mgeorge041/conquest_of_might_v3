using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapObject : MonoBehaviour
{
    protected Map map;
    protected GameMap gameMap;
    protected ActionMap actionMap;
    protected FogMap fogMap;

    public Tile defaultTile;
    public Tilemap tilemap;
    public Grid tileGrid;
    public Camera playerCamera;

    protected bool isOverTilemap = false;

    // Get map
    public Map GetMap() {
        return map;
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

    // Draw the map
    public void DrawMap() {
        ClearMap();
        PaintMap();

        /*
            int distance = map.GetDistanceToCenterHex(hexCoords);

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
        */
    }

    // Paint map tiles
    public void PaintMap() {
        foreach (Vector3Int tileCoords in map.GetTileHexCoordsDict().Keys) {
            tilemap.SetTile(tileCoords, defaultTile);
        }
        tilemap.RefreshAllTiles();
    }

    // Clear map tiles and dicts
    public void ClearMap() {
        foreach (Vector3Int tileCoords in map.GetRemovedTiles()) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.CompressBounds();
    }

    // Clear map tile at coordinates
    public void ClearMapTile(Vector3Int tileCoords) {
        tilemap.SetTile(tileCoords, defaultTile);
        tilemap.RefreshTile(tileCoords);
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
        return map.GetHexCoordsFromTileCoords(tileCoords);
    }

    // Get whether over a tilemap tile
    public bool MouseOverTile() {
        return isOverTilemap;
    }

    // Mouse enters
    public void OnMouseEnter() {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        isOverTilemap = true;
    }

    // Mouse exits
    public void OnMouseExit() {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        isOverTilemap = false;
    }
}
