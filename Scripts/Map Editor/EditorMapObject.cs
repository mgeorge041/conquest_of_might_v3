using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorMapObject : MapObject
{
    public EditorMap editorMap;
    public Tilemap editorTilemap;
    public Camera mainCamera;
    public SelectedTileInfo selectedTileInfo;
    public Slider mapSizeSlider;

    // Tilemap variables
    private int currentTileRotation = 0;

    private Tile selectedEditorTile;
    private Tile paintTile;

    // Paint modes for drawing editor tiles
    public enum PaintMode
    {
        paint,
        erase
    }
    private PaintMode paintMode = PaintMode.erase;

    // Start
    public void Start() {
        editorMap = new EditorMap();
        DrawEditorMap();
        mapSizeSlider.value = GameSetupData.mapRadius;
    }

    // Draw the map
    public void DrawEditorMap() {
        editorMap.CreateMap();
        ClearEditorMap();
        PaintEditorMap();
    }

    // Paint map tiles
    public void PaintEditorMap() {
        foreach (Vector3Int tileCoords in editorMap.GetTileHexCoordsDict().Keys) {
            if (tilemap.GetTile(tileCoords) != null) {
                continue;
            }
            tilemap.SetTile(tileCoords, defaultTile);
        }
        tilemap.CompressBounds();
        tilemap.RefreshAllTiles();
    }

    // Clear map tiles and dicts
    public void ClearEditorMap() {
        foreach (Vector3Int tileCoords in editorMap.GetRemovedTiles()) {
            tilemap.SetTile(tileCoords, null);
        }
        tilemap.CompressBounds();
        tilemap.RefreshAllTiles();
    }

    // Set the current paint editor tile
    public void SetSelectedEditorTile(Tile editorTile) {
        selectedEditorTile = editorTile;
        paintTile = selectedEditorTile;
    }

    // Get the current paint editor tile
    public Tile GetSelectedEditorTile() {
        return selectedEditorTile;
    }

    // Set current paint mode
    public void SetPaintMode(string tileMode) {
        if (tileMode == "P") {
            SetPaintMode(PaintMode.paint);
        }
        else {
            SetPaintMode(PaintMode.erase);
        }
    }

    // Set current paint mode
    public void SetPaintMode(PaintMode paintMode) {
        this.paintMode = paintMode;

        if (paintMode == PaintMode.paint) {
            paintTile = selectedEditorTile;
        }
        else {
            paintTile = defaultTile;
        }
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

    // Paint tile to tilemap
    private void PaintTile() {
        Vector3Int tileCoords = GetMouseTileCoords(mainCamera, Input.mousePosition);
        if (!editorMap.HasTileCoords(tileCoords)) {
            return;
        }

        if (selectedEditorTile != null) {
            tilemap.SetTile(tileCoords, paintTile);
            tilemap.SetTransformMatrix(tileCoords, Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, currentTileRotation))));
            tilemap.RefreshTile(tileCoords);
        }
    }

    // Paint tile to tilemap
    public void OnMouseDown() {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        // Paint or erase tile
        PaintTile();
    }

    // Paint tile to tilemap
    private void OnMouseDrag() {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        // Paint or erase tile
        PaintTile();
    }
}
