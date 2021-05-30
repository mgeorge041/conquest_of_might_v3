using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapTestScene : MonoBehaviour
{
    public Map testMap;
    private int hightlightHexRange = 1; 
    private bool isHighlighted = false;

    // Set highlight hex range
    public void SetHighlightHexRange(float range)
    {
        hightlightHexRange = (int)range;
    }

    // Highlight test map hexes in range
    public void HighlightHexesInRange(Vector3Int centerHexCoords, int range)
    {
        testMap.HighlightHexesInRange(testMap.hexCoordsDict, centerHexCoords, range);
    }

    // Mouse clicks
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Highlight hexes in range
            if (!isHighlighted)
            {
                Vector3Int hexCoords = testMap.WorldToHexCoords(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                HighlightHexesInRange(hexCoords, hightlightHexRange);
                isHighlighted = true;
            }

            // Dehighlight hexes
            else
            {
                testMap.DehighlightHexes();
                isHighlighted = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
