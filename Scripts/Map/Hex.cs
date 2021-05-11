using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Hex
{
    public TileData tileData { get; set; }
    public Vector3Int hexCoords { get; protected set; }
    public Vector3Int tileCoords { get; protected set; }

    // Empty constructor
    public Hex()
    {

    }

    // Constructor
    public Hex(TileData tileData, Vector3Int hexCoords) {
        this.tileData = tileData;
        this.hexCoords = hexCoords;
        tileCoords = HexToTileCoords(hexCoords);
    }

    // Create hex based on type
    public static Hex CreateHex<T>(TileData defaultTile, Vector3Int newHexCoords) where T : Hex
    {
        if (typeof(T) == typeof(Hex))
        {
            Hex newHex = new Hex(defaultTile, newHexCoords);
            return newHex;
        }
        else if (typeof(T) == typeof(GameHex))
        {
            GameHex newHex = new GameHex(defaultTile, newHexCoords);
            return newHex;
        }
        return null;
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

    // Get distance between two hexes
    public static int GetDistanceHexes(Hex hex1, Hex hex2)
    {
        return GetDistanceHexCoords(hex1.hexCoords, hex2.hexCoords);
    }

    // Get distance between two hex coordinates
    public static int GetDistanceHexCoords(Vector3Int hexCoords1, Vector3Int hexCoords2)
    {
        int x = Math.Abs(hexCoords1.x - hexCoords2.x);
        int y = Math.Abs(hexCoords1.y - hexCoords2.y);
        int z = Math.Abs(hexCoords1.z - hexCoords2.z);

        int distance = (x + y + z) / 2;

        return distance;
    }

    // Get the distance between the center hex and hex coordinates
    public static int GetDistanceToCenterHex(Vector3Int hexCoords)
    {
        return GetDistanceHexCoords(hexCoords, Vector3Int.zero);
    }

    // Get move cost
    public int GetMoveCost()
    {
        return tileData.moveCost;
    }
}
