using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    // Tilemap
    public EditorMapObject editorMapObject;

    // Paint mode buttons
    public Toggle paintToggle;
    public Toggle eraseToggle;

    // Map Size label
    public Text labelText;

    // Set text
    public void SetMapSizeText(float mapRadius) {
        labelText.text = "Map Size: " + mapRadius;
        editorMapObject.editorMap.mapRadius = (int)mapRadius;
    }

    // Update is called once per frame
    void Update() {
        // Paint mode buttons
        if (Input.GetKeyDown(KeyCode.E)) {
            eraseToggle.isOn = true;
            editorMapObject.SetPaintMode(EditorMapObject.PaintMode.erase);
        }
        else if (Input.GetKeyDown(KeyCode.F)) {
            paintToggle.isOn = true;
            editorMapObject.SetPaintMode(EditorMapObject.PaintMode.paint);
        }

        // Rotation of tile
        if (Input.GetKeyDown(KeyCode.R)) {
            editorMapObject.AdjustTileRotation180();
        }
    }
}
