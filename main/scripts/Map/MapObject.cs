using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapObject : MonoBehaviour
{
    protected Map map;

    public Tile defaultTile;
    public Tilemap tilemap;
    public Grid tileGrid;
    public Camera playerCamera;

    protected bool isOverTilemap = false;

    // Get map
    public Map GetMap() {
        return map;
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
