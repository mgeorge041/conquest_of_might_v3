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
        return HexToTileCoords(hexCoords);
    }

    // Converts hex coordinates to tilemap coordinates
    public static Vector3Int HexToTileCoords(Vector3Int hexCoords)
    {
        int x = hexCoords.z + (hexCoords.x - (hexCoords.x & 1)) / 2;
        int y = hexCoords.x;

        return new Vector3Int(x, y, 0);
    }

    // Converts tilemap coordinates to hex coordinates
    public static Vector3Int TileToHexCoords(Vector3Int tileCoords)
    {
        int x = tileCoords.y;
        int y = -tileCoords.x - (tileCoords.y + (tileCoords.y & 1)) / 2;
        int z = -x - y;

        return new Vector3Int(x, y, z);
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
