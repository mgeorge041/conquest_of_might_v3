using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests
{
    public class CardBuildingUTests
    {
        private CardBuilding cardBuilding;

        // Load a test building card
        public static CardBuilding CreateTestCardBuilding()
        {
            CardBuilding newCard = Resources.Load<CardBuilding>("Cards/Tests/Test Building");
            return newCard;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            cardBuilding = CreateTestCardBuilding();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            cardBuilding = null;
        }

        // Test card building created
        [Test]
        public void CardBuildingCreated()
        {
            Assert.IsNotNull(cardBuilding);
        }

        // Test card building details
        [Test]
        public void CardBuildingStatsCorrect()
        {
            Assert.AreEqual(10, cardBuilding.health);
            Assert.AreEqual("Test Building", cardBuilding.name);
        }

        // Test get card resources
        [Test]
        public void CardBuildingResourcesCorrect()
        {
            Dictionary<ResourceType, int> cardResources = cardBuilding.GetResourceCosts();
            Assert.AreEqual(10, cardResources[ResourceType.Wood]);
            Assert.AreEqual(1, cardResources.Keys.Count);
        }
    }
}