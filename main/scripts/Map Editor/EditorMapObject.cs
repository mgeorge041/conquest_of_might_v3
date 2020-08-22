using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorMapObject : MapObject
{
    private EditorMap editorMap;
    public Camera mainCamera;

    // Set editor map
    public void SetEditorMap(EditorMap editorMap) {
        this.editorMap = editorMap;
    }

    // Start is called before the first frame update
    private void Start() {

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
        if (EventSystem.current.IsPointerOverGameObject() || !isOverTilemap) {
            return;
        }

        // Paint or erase tile
        PaintTile();
    }

    // Paint tile to tilemap
    private void OnMouseDrag() {
        // Do nothing if over UI
        if (EventSystem.current.IsPointerOverGameObject() || !isOverTilemap) {
            return;
        }

        // Paint or erase tile
        PaintTile();
    }
}
