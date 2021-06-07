using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests
{
    public class CardResourceDisplayUTests
    {
        private CardResourceDisplay cardResourceDisplay;

        // Create test card resource display
        public static CardResourceDisplay CreateTestCardResourceDisplay()
        {
            CardResourceDisplay cardResourceDisplay = TestFunctions.CreateClassObject<CardResourceDisplay>("Assets/Resources/Prefabs/Card Resource Display.prefab");
            return cardResourceDisplay;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            cardResourceDisplay = CreateTestCardResourceDisplay();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(cardResourceDisplay.gameObject);
        }

        // Test card resource display created
        [Test]
        public void CardResourceDisplayCreated()
        {
            Assert.IsNotNull(cardResourceDisplay);
        }
    }
}