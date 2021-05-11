using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.CardTests;

namespace Tests.ITests.PlayerTests
{
    public class PlayerHandITests
    {
        private Player player;
        private Hand hand;
        private CardUnit cardUnit;
        private CardBuilding cardBuilding;
        private CardResource cardResource;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            hand = HandUTests.CreateTestHand(player);
            player.hand = hand;
            cardUnit = CardUnitUTests.CreateTestCardUnit();
            cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
            cardResource = CardResourceUTests.CreateTestCardResource();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            hand = null;
            cardUnit = null;
        }

        // Test add card unit to hand
        [Test]
        public void AddsCardUnitToHand()
        {
            hand.AddCard(cardUnit);
            Assert.AreEqual(1, hand.cards.Count);
            Assert.IsFalse(cardUnit.isPlayable);
        }

        // Test add card building to hand
        [Test]
        public void AddsCardBuildingToHand()
        {
            hand.AddCard(cardBuilding);
            Assert.AreEqual(1, hand.cards.Count);
            Assert.IsFalse(cardUnit.isPlayable);
        }

        // Test add card resource to hand
        [Test]
        public void AddsCardResourceToHand()
        {
            hand.AddCard(cardResource);
            Assert.AreEqual(0, hand.cards.Count);
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

    }
}