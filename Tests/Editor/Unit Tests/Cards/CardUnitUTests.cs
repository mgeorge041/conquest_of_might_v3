using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests
{
    public class CardUnitUTests
    {
        private CardUnit cardUnit;

        // Load a test unit card
        public static CardUnit CreateTestCardUnit()
        {
            CardUnit newCard = Resources.Load<CardUnit>("Cards/Tests/Test Unit");
            return newCard;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            cardUnit = CreateTestCardUnit();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            cardUnit = null;
        }

        // Test card unit created
        [Test]
        public void CardUnitCreated()
        {
            Assert.IsNotNull(cardUnit);
        }

        // Test card unit details
        [Test]
        public void CardUnitStatsCorrect()
        {
            Assert.AreEqual(5, cardUnit.speed);
            Assert.AreEqual("Test Unit", cardUnit.name);
        }

        // Test get card resources
        [Test]
        public void CardUnitResourcesCorrect()
        {
            Dictionary<ResourceType, int> cardResources = cardUnit.GetResourceCosts();
            Assert.AreEqual(5, cardResources[ResourceType.Food]);
            Assert.AreEqual(1, cardResources.Keys.Count);
        }
    }
}