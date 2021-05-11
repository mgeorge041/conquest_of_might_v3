using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapTestScene : MonoBehaviour
{
    public GameMap gameMap;
    public GameObject unitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting test scene");
        GameObject newUnitGameObject = Instantiate(unitPrefab);
        Unit unit = newUnitGameObject.GetComponent<Unit>();
        gameMap.AddPiece(unit, Vector3Int.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
