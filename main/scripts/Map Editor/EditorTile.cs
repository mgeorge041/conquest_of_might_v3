using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EditorTile : MonoBehaviour
{
    public Button button;
    public Tile tile;
    public Image tileSprite;
    public TileData tileData;
    private SelectedTileInfo selectedTileInfo;

    // Start is called before the first frame update
    void Start()
    {
        selectedTileInfo = GameObject.Find("Selected Tile Info").GetComponent<SelectedTileInfo>();
        button.onClick.AddListener(UpdateSelectedTileInfo);
    }

    // Set tile data
    public void SetTileData(TileData tileData) {
        this.tileData = tileData;
    }

    // Get tile move cost
    public int GetTileMoveCost() {
        return tileData.moveCost;
    }

    // Get tilemap tile name
    public string GetTileName()
    {
        return tile.name;
    }

    // Get tilemap tile
    public Tile GetTile()
    {
        return tile;
    }

    // Set tilemap tile
    public void SetTile(Tile tile)
    {
        this.tile = tile;
        tileSprite.sprite = tile.sprite;
    }

    // Update selected editor tile info section
    private void UpdateSelectedTileInfo()
    {
        selectedTileInfo.UpdateSelectedTile(this);
    }

    // Get sprite
    public Sprite GetSprite()
    {
        return tile.sprite;
    }
}
