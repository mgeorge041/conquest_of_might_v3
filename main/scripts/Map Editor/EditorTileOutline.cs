using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorTileOutline : MonoBehaviour
{
    public Sprite baseOutline;
    public Sprite highlightOutline;
    public SpriteRenderer spriteRenderer;
    private EditorTile editorTile;
    private EditorMap mapEditor;
    private bool hasEditorTile = false;
    private Vector3Int hexCoords;
    private EditorTileOutline[] neighborEditorTileOutlines;


    // Get whether editor tile outline has an editor tile
    public bool HasEditorTile()
    {
        return hasEditorTile;
    }

    // Highlight editor tile outline on mouse enter
    private void OnMouseEnter()
    {
        if (!hasEditorTile && !EventSystem.current.IsPointerOverGameObject())
        {
            spriteRenderer.sprite = highlightOutline;
        }
    }

    // Show base outline on mouse exit
    private void OnMouseExit()
    {
        if (!hasEditorTile && !EventSystem.current.IsPointerOverGameObject())
        {
            spriteRenderer.sprite = baseOutline;
        }
    }

    // Update editor tile outline based on paint mode on mouse click
    private void OnMouseDown()
    {
        /*
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            switch (mapEditor.GetPaintMode())
            {
                case MapEditor.paintModes.paint:
                    EditorTile selectedEditorTile = mapEditor.GetSelectedEditorTile();
                    editorTile = selectedEditorTile;
                    spriteRenderer.sprite = editorTile.GetSprite();
                    hasEditorTile = true;
                    break;
                case MapEditor.paintModes.erase:
                    spriteRenderer.sprite = baseOutline;
                    hasEditorTile = false;
                    break;
                default:
                    spriteRenderer.sprite = baseOutline;
                    hasEditorTile = false;
                    break;
            }
        }
        */
    }

    // Get hex coordinates
    public Vector3Int GetHexCoords()
    {
        return hexCoords;
    }

    // Set hex coordinates
    public void SetHexCoords(Vector3Int hexCoords)
    {
        this.hexCoords = hexCoords;
    }

    // Set map editor reference
    public void SetMapEditor(EditorMap mapEditor)
    {
        this.mapEditor = mapEditor;
    }


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
