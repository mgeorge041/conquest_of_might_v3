using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hex
{
    protected TileData tileData;
    protected Vector3Int hexCoords;

    // Constructor
    public Hex() {

    }

    // Constructor
    public Hex(TileData tileData, Vector3Int hexCoords) {
        this.tileData = tileData;
        this.hexCoords = hexCoords;
    }

    // Get hex coords
    public Vector3Int GetHexCoords() {
        return hexCoords;
    }

    // Get tile coords
    public Vector3Int GetTileCoords() {
        return Map.ConvertHexToTileCoords(hexCoords);
    }

    // Get tile
    public TileData GetTile()
    {
        return tileData;
    }

    // Set tile
    public void SetTile(TileData tileData)
    {
        this.tileData = tileData;
    }

    // Get move cost
    public int GetMoveCost()
    {
        return tileData.moveCost;
    }
}
