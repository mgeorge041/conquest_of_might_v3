using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests 
{ 
    public class CardResourceUTests
    {
        // Load a test unit card
        public static CardResource CreateTestFoodCardResource()
        {
            CardResource newCard = Resources.Load<CardResource>(ENV.CARD_RESOURCE_TEST_RESOURCE_PATH);
            return newCard;
        }
    }
}