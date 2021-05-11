using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceTestScene : MonoBehaviour
{
    public GameObject unitPrefab;
    public GameObject buildingPrefab;
    private Unit unit;
    private Building building;
    private CardUnit cardUnit;
    private CardBuilding cardBuilding;

    // Update unit health
    public void UpdateUnitHealth(float newHealth)
    {
        unit.health = (int)newHealth;
    }

    // Update building health
    public void UpdateBuildingHealth(float newHealth)
    {
        building.health = (int)newHealth;
    }

    // Disable/enable unit
    public void ToggleUnit()
    {
        if (unit.hasActions)
        {
            unit.EndTurn();
        }
        else
        {
            unit.ResetPiece();
        }
    }

    // Disable/enable building
    public void ToggleBuilding()
    {
        if (building.hasActions)
        {
            building.EndTurn();
        }
        else
        {
            building.ResetPiece();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create unit
        unitPrefab = Instantiate(unitPrefab);
        unit = unitPrefab.GetComponent<Unit>();
        cardUnit = CardUnit.LoadTestCardUnit();
        unit.SetCard(cardUnit);

        // Create building
        buildingPrefab = Instantiate(buildingPrefab);
        building = buildingPrefab.GetComponent<Building>();
        cardBuilding = CardBuilding.LoadTestCardBuilding();
        building.SetCard(cardBuilding);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
