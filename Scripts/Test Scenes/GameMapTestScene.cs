using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapTestScene : MonoBehaviour
{
    public GameMap gameMap;
    public GameObject unitPrefab;
    public Player player;
    public Player enemyPlayer;
    private Unit unit;

    // Reset test unit
    public void ResetUnit()
    {
        unit.ResetPiece();
    }

    // Create enemy unit
    public void CreateEnemyUnit()
    {
        Unit enemyUnit;
        Vector3Int currentHexCoords = unit.gameHex.hexCoords;
        Vector3Int enemyHexCoords = currentHexCoords + new Vector3Int(1, -1, 0);
        if (gameMap.hexCoordsDict.ContainsKey(enemyHexCoords) && !gameMap.GetHexAtHexCoords(enemyHexCoords).HasPiece())
        {
            enemyUnit = GamePiece.CreatePiece<Unit>(Resources.Load<CardUnit>(ENV.CARD_UNIT_TEST_RESOURCE_PATH), enemyPlayer);
            gameMap.AddPiece(enemyUnit, enemyHexCoords);
        }
        else 
        {
            enemyHexCoords = currentHexCoords - new Vector3Int(1, -1, 0);
            if (gameMap.hexCoordsDict.ContainsKey(enemyHexCoords) && !gameMap.GetHexAtHexCoords(enemyHexCoords).HasPiece())
            {
                enemyUnit = GamePiece.CreatePiece<Unit>(Resources.Load<CardUnit>(ENV.CARD_UNIT_TEST_RESOURCE_PATH), enemyPlayer);
                gameMap.AddPiece(enemyUnit, enemyHexCoords);
            }
            else
            {
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        unit = GamePiece.CreatePiece<Unit>(Resources.Load<CardUnit>(ENV.CARD_UNIT_TEST_RESOURCE_PATH), player);
        gameMap.AddPiece(unit, Vector3Int.zero);
        player.playerCamera.tilemap = gameMap.tilemap;
        player.InitializeObject(1, Vector3Int.zero);
        enemyPlayer.InitializeObject(2, Vector3Int.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
