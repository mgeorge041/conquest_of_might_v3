using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHex : Hex
{
    // Game piece and coords
    public GamePiece piece { get; set; }
    public Vector3Int[] neighborHexCoords { get; } = new Vector3Int[6];

    private Vector3Int[] neighborCoords =
    {
        new Vector3Int(-1, 1, 0),
        new Vector3Int(0, 1, -1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(0, -1, 1),
        new Vector3Int(-1, 0, 1)
    };

    // Constructor
    public GameHex(TileData tileData, Vector3Int hexCoords) {
        this.tileData = tileData;
        this.hexCoords = hexCoords;
        SetNeighborCoords();
    }

    // Set neighbor coords
    public void SetNeighborCoords() {
        for (int i = 0; i < neighborCoords.Length; i++) {
            neighborHexCoords[i] = hexCoords + neighborCoords[i];
        }
    }

    // Get neighbor by index
    public Vector3Int GetNeighborByIndex(int neighborIndex) {
        return neighborHexCoords[neighborIndex];
    }

    // Get whether has piece
    public bool HasPiece() {
        if (piece != null) {
            return true;
        }
        return false;
    }

    // Get whether piece can move
    public bool PieceCanMove() {
        return piece.canMove;
    }
}
