using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorMap : Map
{
    // Tilemap variables
    private Tile selectedEditorTile;
    private Tile paintTile;
    private int currentTileRotation = 0;

    // Constructor
    public EditorMap() {
        CreateMap();
    }

    // Constructor
    public EditorMap(Tile defaultTile, Tilemap tilemap) {
        this.defaultTile = defaultTile;
        selectedEditorTile = defaultTile;
        paintTile = selectedEditorTile;
        this.tilemap = tilemap;
        tileGrid = tilemap.layoutGrid;
        DrawNewMap();
    }

    // Paint modes for drawing editor tiles
    public enum PaintMode
    {
        paint,
        erase
    }
    private PaintMode paintMode = PaintMode.erase;

    // Get current paint mode
    public Enum GetPaintMode()
    {
        return paintMode;
    }

    // Get whether is in paint mode
    public bool IsPaintMode() {
        if (paintMode == PaintMode.paint) {
            return true;
        }
        return false;
    }

    // Set current paint mode
    public void SetPaintMode(PaintMode paintMode) {
        this.paintMode = paintMode;
        if (paintMode == PaintMode.erase) {
            paintTile = defaultTile;
        }
        else {
            paintTile = selectedEditorTile;
        }
    }

    // Paint the current tile
    public void PaintTile(Vector3Int tileCoords) {
        if (selectedEditorTile != null) {
            tilemap.SetTile(tileCoords, paintTile);
            tilemap.SetTransformMatrix(tileCoords, Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, currentTileRotation))));
            tilemap.RefreshTile(tileCoords);
        }
    }

    // Set the current paint editor tile
    public void SetSelectedEditorTile(Tile editorTile)
    {
        selectedEditorTile = editorTile;
        paintTile = selectedEditorTile;
    }

    // Get the current paint editor tile
    public Tile GetSelectedEditorTile()
    {
        return selectedEditorTile;
    }

    // Get the current rotation
    public int GetCurrentTileRotation() {
        return currentTileRotation;
    }

    // Set tile rotation
    public void SetTileRotation(int tileRotation) {
        currentTileRotation = tileRotation;
    }

    // Adjust tile rotation 180 degrees
    public void AdjustTileRotation180() {
        currentTileRotation = (currentTileRotation + 180) % 360;
    }
}
