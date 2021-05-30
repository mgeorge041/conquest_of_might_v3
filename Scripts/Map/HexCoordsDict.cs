using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordsDict
{
    public Dictionary<Vector3Int, Hex> hexCoordsDict { get; private set; } = new Dictionary<Vector3Int, Hex>();

}
