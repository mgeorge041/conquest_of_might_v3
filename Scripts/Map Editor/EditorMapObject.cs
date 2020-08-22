using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorMapObject : MapObject
{
    public Tilemap editorTilemap;
    private EditorMap editorMap;
    public Camera mainCamera;
    public SelectedTileInfo selectedTileInfo;

    // Set editor map
    public void SetEditorMap(EditorMap editorMap) {
        this.editorMap = editorMap;
    }

    // Start is called before the first frame update
    private void Start() {
        editorMap = new EditorMap(editorTilemap);
        selectedTileInfo.SetEditorMap(editorMap);
    }

    // Update is called once per frame
    void Update() {

    }

    // Paint tile to tilemap
    private void PaintTile() {
        Vector3Int tileCoords = editorMap.GetMouseTileCoords(mainCamera, Input.mousePosition);
        if (!editorMap.HasTile(tileCoords)) {
            return;
        }
        editorMap.PaintTile(tileCoords);
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
