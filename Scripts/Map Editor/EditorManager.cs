using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    // Tilemap
    private EditorMap editorMap;
    public Tilemap editorTilemap;
    public SelectedTileInfo selectedTileInfo;

    // Camera
    public Camera mainCamera;

    // Paint mode buttons
    public Toggle paintToggle;
    public Toggle eraseToggle;

    // Map Size label
    public Text labelText;
    public Slider mapSizeSlider;

    // Set text
    public void SetMapSizeText(float mapSize) {
        //int mapRadius = (int)(mapSize - 1) / 2;
        labelText.text = "Map Size: " + mapSize;
        editorMap.SetNewMapRadius((int)mapSize);
    }

    // Set current paint mode
    public void SetPaintMode(string tileMode) {
        if (tileMode == "P") {
            editorMap.SetPaintMode(EditorMap.PaintMode.paint);
        }
        else {
            editorMap.SetPaintMode(EditorMap.PaintMode.erase);
        }
    }

    // Draw editor map
    public void DrawMap() {
        editorMap.DrawNewMap();
    }

    // Start is called before the first frame update
    private void Start() {
        Tile defaultTile = Resources.Load<Tile>("Tiles/Tiles/Default");
        editorMap = new EditorMap(defaultTile, editorTilemap);
        selectedTileInfo.SetEditorMap(editorMap);
        mapSizeSlider.value = GameSetupData.mapRadius;
    }

    // Update is called once per frame
    void Update() {
        // Paint mode buttons
        if (Input.GetKeyDown(KeyCode.E)) {
            eraseToggle.isOn = true;
            editorMap.SetPaintMode(EditorMap.PaintMode.erase);
        }
        else if (Input.GetKeyDown(KeyCode.F)) {
            paintToggle.isOn = true;
            editorMap.SetPaintMode(EditorMap.PaintMode.paint);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            editorMap.AdjustTileRotation180();
        }
    }
}
