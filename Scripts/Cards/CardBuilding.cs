using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Building", menuName = "Card Building")]
public class CardBuilding : CardPiece
{
    // Load a test card building
    public static CardBuilding LoadTestCardBuilding()
    {
        CardBuilding newCard = Resources.Load<CardBuilding>("Cards/Tests/Test Building");
        return newCard;
    }
}
