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
        public static CardResource CreateTestCardResource()
        {
            CardResource newCard = Resources.Load<CardResource>("Cards/Tests/Test Resource");
            return newCard;
        }
    }
}