using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.CardTests;

namespace Tests.ITests.PlayerTests
{
    public class Player_CardITests
    {
        private Player player;
        private CardUnit cardUnit;
        private CardBuilding cardBuilding;
        private CardResource cardResource;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            cardUnit = CardUnitUTests.CreateTestCardUnit();
            cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
            cardResource = CardResourceUTests.CreateTestFoodCardResource();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            cardUnit = null;
            cardBuilding = null;
            cardResource = null;
            C.Destroy(player.gameObject);
        }

        // Test add card unit to hand
        [Test]
        public void AddsCardUnitToHand()
        {
            player.AddCard(cardUnit);
            Assert.AreEqual(1, player.cardPieceDisplays.Count);
        }

        // Test add card building to hand
        [Test]
        public void AddsCardBuildingToHand()
        {
            player.AddCard(cardBuilding);
            Assert.AreEqual(1, player.cardPieceDisplays.Count);
        }

        // Test add card resource to hand
        [Test]
        public void AddsCardResourceToHand()
        {
            player.AddCard(cardResource);
            Assert.AreEqual(0, player.cardPieceDisplays.Count);
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

        // Test add multiple card resource to hand
        [Test]
        public void AddsMultipleCardResourceToHand()
        {
            for (int i = 0; i < 5; i++)
            {
                player.AddCard(cardResource);
                cardResource = CardResourceUTests.CreateTestFoodCardResource();
            }
            Assert.AreEqual(5, player.GetResourceCount(ResourceType.Food));
        }

    }
}