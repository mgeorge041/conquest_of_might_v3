using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;

namespace Tests.ITests.CardTests
{
    public class CardResource_CardResourceDisplayITests
    {
        private CardResourceDisplay cardResourceDisplay;

        // Create test card resource display
        public static CardResourceDisplay CreateTestCardResourceDisplay()
        {
            CardResource cardResource = CardResourceUTests.CreateTestFoodCardResource();
            CardResourceDisplay cardResourceDisplay = CardResourceDisplayUTests.CreateTestCardResourceDisplay();
            cardResourceDisplay.SetCard(cardResource);
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