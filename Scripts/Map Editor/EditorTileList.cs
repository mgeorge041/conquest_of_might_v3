using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditorTileList : MonoBehaviour
{
    public EditorTile editorTilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Load all tiles as editor tile objects
        Sprite[] tileSprites = Resources.LoadAll<Sprite>("Art/Tiles/Game Map/");
        TileData[] tileData = Resources.LoadAll<TileData>("Tiles/Tile Data/");
        for (int i = 0; i < tileSprites.Length; i++)
        {
            EditorTile newEditorTile = Instantiate(editorTilePrefab, transform);
            Tile newTile = (Tile)ScriptableObject.CreateInstance("Tile");
            newTile.sprite = tileSprites[i];
            newTile.name = tileSprites[i].name;
            newEditorTile.SetTile(newTile);

            // Get tile data
            for (int j = 0; j < tileData.Length; j++) {
                if (tileData[j].tileName == newTile.name) {
                    newEditorTile.SetTileData(tileData[j]);
                    break;
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }
}
