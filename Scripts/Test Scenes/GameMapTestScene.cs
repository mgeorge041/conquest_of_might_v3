using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapTestScene : MonoBehaviour
{
    public GameMap gameMap;
    public GameObject unitPrefab;
    public MapCamera mapCamera;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newUnitGameObject = Instantiate(unitPrefab);
        Unit unit = newUnitGameObject.GetComponent<Unit>();
        gameMap.AddPiece(unit, Vector3Int.zero);
        mapCamera.tilemap = gameMap.tilemap;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
