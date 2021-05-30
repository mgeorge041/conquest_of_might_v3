using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogMap : Map
{

    private List<Vector3Int> visibleTileCoords = new List<Vector3Int>();

    // Constructor
    public FogMap() {
        CreateMap();
    }

    // Constructor with map size
    public FogMap(int mapRadius) {
        this.mapRadius = mapRadius;
        newMapRadius = mapRadius;
        CreateMap();
    }

    // Get visible tiles
    public List<Vector3Int> GetVisibleTileCoords() {
        return new List<Vector3Int>(visibleTileCoords);
    }

    // Clears out visible tiles
    public void ClearVisibleTiles() {
        visibleTileCoords.Clear();
    }

    // Get visible tiles in sight range
    public void GetVisibleTilesForPieces(List<GamePiece> pieces) {
        for (int i = 0; i < pieces.Count; i++) {
            Vector3Int pieceHexCoords = pieces[i].gameHex.hexCoords;

            // Get all tiles within sight range
            List<Hex> visibleHexes = GetHexesInRange<Hex>(hexCoordsDict, pieceHexCoords, pieces[i].GetCard().sightRange, true);
            for (int j = 0; j < visibleHexes.Count; j++) {
                visibleTileCoords.Add(visibleHexes[j].tileCoords);
            }
        }
    }



    // Create fog of war map
    public void CreateFogMap(List<GamePiece> pieces) {
        ClearVisibleTiles();
        GetVisibleTilesForPieces(pieces);
    }


}
