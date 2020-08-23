using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class SelectedTileInfo : MonoBehaviour
{
    public Text selectedTileName;
    private Tile currentTile;
    public Image currentTileImage;
    public EditorMapObject editorMapObject;
    private int currentTileRotation = 0;
    public Text moveCostLabel;

    // Update new selected tile
    public void UpdateSelectedTile(EditorTile newTile)
    {
        Tile newPaintTile = newTile.GetTile();
        currentTile = newPaintTile;
        selectedTileName.text = currentTile.name;
        currentTileImage.sprite = currentTile.sprite;
        editorMapObject.SetSelectedEditorTile(currentTile);
        moveCostLabel.text = newTile.GetTileMoveCost().ToString();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentTileRotation = (currentTileRotation + 180) % 360;
            currentTileImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentTileRotation));
        }
    }
}
