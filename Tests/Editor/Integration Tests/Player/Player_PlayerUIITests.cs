using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.CardTests;

namespace Tests.ITests.PlayerTests
{
    public class PlayerPlayerUIITests
    {
        private Player player;
        private CardResource foodResourceCard;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            foodResourceCard = CardResourceUTests.CreateTestFoodCardResource();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(player.gameObject);
        }

        // Test update resource counter on card draw
        [Test]
        public void ResourceCounterUpdatesOnResourceCardDraw()
        {
            player.DrawCard(foodResourceCard);
            Assert.AreEqual(1, player.playerUI.GetResourceCount(ResourceType.Food));
        }

        // Test resource counter does not update on piece card draw
        [Test]
        public void ResourceCounterDoesNotUpdateOnPieceCardDraw()
        {
            CardPiece newCardPiece = CardUnitUTests.CreateTestCardUnit();
            player.DrawCard(newCardPiece);
            Assert.AreEqual(0, player.playerUI.GetResourceCount(ResourceType.Food));
        }

        // Test drawn resource card does not go to hand
        [Test]
        public void DrawnResourceCardDoesNotGoToHand()
        {
            player.DrawCard(foodResourceCard);
            Assert.AreEqual(0, player.hand.cards.Count);
        }

        // Test drawn piece card goes to hand
        [Test]
        public void DrawnPieceCardGoesToHand()
        {
            CardPiece newCardPiece = CardUnitUTests.CreateTestCardUnit();
            player.DrawCard(newCardPiece);
            Assert.AreEqual(1, player.cardPieceDisplays.Count);
        }

        // Test drawn piece card creates card display object
        [Test]
        public void DrawnPieceCardCreatesCardDisplayObject()
        {
            CardPiece newCardPiece = CardUnitUTests.CreateTestCardUnit();
            player.DrawCard(newCardPiece);
            Assert.AreEqual(1, player.handTransform.childCount);
        }
    }
}