using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Unit", menuName = "Card Unit")]
public class CardUnit : CardPiece
{
    public int speed;

    // Load test card unit
    public static CardUnit LoadTestCardUnit()
    {
        CardUnit newCard = Resources.Load<CardUnit>(ENV.CARD_UNIT_TEST_RESOURCE_PATH);
        return newCard;
    }
}
