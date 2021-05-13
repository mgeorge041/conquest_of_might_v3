using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GameMap : Map
{
    // Pieces
    private Dictionary<int, List<GamePiece>> pieces = new Dictionary<int, List<GamePiece>>();

    public Vector3Int[] neighborCoords =
    {
        new Vector3Int(-1, 1, 0),
        new Vector3Int(0, 1, -1),
        new Vector3Int(1, 0, -1),
        new Vector3Int(1, -1, 0),
        new Vector3Int(0, -1, 1),
        new Vector3Int(-1, 0, 1)
    };

    // Awake
    public void Awake()
    {
        Initialize();
    }

    // Initalize
    public new void Initialize()
    {
        InitializeVariables();
        CreateMap<GameHex>();
        PaintMap();
    }

    // Constructor with map size
    public GameMap(int mapRadius) {
        this.mapRadius = mapRadius;
        newMapRadius = mapRadius;
        CreateMap<GameHex>();
    }

    // Create map
    public void CreateMap()
    {
        CreateMap<GameHex>();
    }

    // Updates the map to a new size
    public new void UpdateMapToRadius(int newRadius)
    {
        newMapRadius = newRadius;
        CreateMap();
        DrawMap();
    }

    // Get hex from hex coords
    public new GameHex GetHexAtHexCoords(Vector3Int hexCoords) {
        return (GameHex)base.GetHexAtHexCoords(hexCoords);
    }

    // Get hex from tile coords
    public new GameHex GetHexAtTileCoords(Vector3Int tileCoords) {
        return (GameHex)base.GetHexAtTileCoords(tileCoords);
    }

    // Get hexes in unit range
    public List<GameHex> GetHexesInUnitRange(Unit unit)
    {
        int totalRange = unit.remainingSpeed + unit.range;
        return GetHexesInRange<GameHex>(unit.gameHex.hexCoords, totalRange);
    }

    // Get hexes in range
    public List<GameHex> GetHexesInPieceRange(GamePiece piece)
    {
        int totalRange = 0;
        if (piece.pieceType == PieceType.Unit)
        {
            Unit unit = (Unit)piece;
            totalRange = unit.remainingSpeed + unit.range;
        }
        else if (piece.pieceType == PieceType.Building)
        {
            totalRange = piece.range;
        }
        return GetHexesInRange<GameHex>(piece.gameHex.hexCoords, totalRange);
    }

    // Add a game piece to the map
    public bool AddPiece(GamePiece piece, Vector3Int hexCoords) {
        GameHex gameHex = GetHexAtHexCoords(hexCoords);
        if (!gameHex.HasPiece()) {

            // Associate hex with piece
            gameHex.piece = piece;
            piece.gameHex = gameHex;

            // Update lists
            //piece.player.PlayPiece(piece);

            return true;
        }
        return false;
    }

    // Moves a piece on the map
    public void MovePiece(Unit unit, Vector3Int targetHexCoords) {
        GameHex currentHex = unit.gameHex;
        GameHex targetHex = GetHexAtHexCoords(targetHexCoords);
        if (targetHex.HasPiece())
        {
            return;
        }

        // Get distance traveled and update new hex
        int distance = Hex.GetDistanceHexes(currentHex, targetHex);
        if (distance <= unit.remainingSpeed)
        {
            unit.DecreaseSpeed(distance);
            unit.gameHex = targetHex;
            targetHex.piece = unit;
            currentHex.piece = null;
            unit.player.SetSelectedPiece(null);
        }
    }

    // Attacks a piece on the map
    public void AttackPiece(GamePiece attackingPiece, GamePiece targetPiece)
    {
        // Return if either piece is null
        if (attackingPiece == null || targetPiece == null)
        {
            return;
        }

        // Return if both pieces belong to same player
        if (attackingPiece.GetPlayerId() == targetPiece.GetPlayerId())
        {
            return;
        }

        // Return if out of range
        int distance = Hex.GetDistanceHexes(attackingPiece.gameHex, targetPiece.gameHex);
        if (distance > attackingPiece.range)
        {
            return;
        }

        int damageGiven = attackingPiece.AttackPiece(targetPiece);
        attackingPiece.player.SetSelectedPiece(null);
        
        // Target piece dies
        if (targetPiece.currentHealth == 0)
        {
            targetPiece.gameHex.piece = null;
            C.Destroy(targetPiece.gameObject);
        }
    }

    // Determine if hex has piece
    public bool HexHasPiece(Vector3Int hexCoords) {
        return GetHexAtHexCoords(hexCoords).HasPiece();
    }

    // Get hex piece
    public GamePiece GetHexPiece(Vector3Int hexCoords) {
        return GetHexAtHexCoords(hexCoords).piece;
    }

    // Determine if can play on hex
    public bool CanPlayOnHex(Vector3Int hexCoords) {
        return !GetHexAtHexCoords(hexCoords).HasPiece();
    }
}
