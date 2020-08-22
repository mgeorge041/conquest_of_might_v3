using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Tile Data", menuName ="Tile Data")]
public class TileData : ScriptableObject
{
    public string tileName;
    public Sprite tileImage;
    public int moveCost;
}
