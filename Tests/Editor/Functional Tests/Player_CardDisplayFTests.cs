using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.CardTests;

namespace Tests.FTests.PlayerTests
{
    public class Player_CardDisplayFTests
    {
        private Player player;
        private CardUnit cardUnit1;
        private CardUnit cardUnit2;
        private CardPieceDisplay cardUnitDisplay1;
        private Sprite unplayableBorder;
        private Sprite playableBorder;
        private Sprite highlightedBorder;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            cardUnit1 = CardUnitUTests.CreateTestCardUnit();
            cardUnit2 = CardUnitUTests.CreateTestCardUnit();
            player.DrawCard(cardUnit1);
            cardUnitDisplay1 = player.cardPieceDisplays[0];
            Sprite[] cardBackgrounds = Resources.LoadAll<Sprite>("Art/Cards/Card Backgrounds");
            playableBorder = cardBackgrounds[1];
            unplayableBorder = Resources.Load<Sprite>(ENV.CARD_UNPLAYABLE_BORDER_RESOURCE_PATH);
            highlightedBorder = Resources.Load<Sprite>(ENV.CARD_HIGHLIGHTED_BORDER_RESOURCE_PATH);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(player.gameObject);
        }

        // Test playable card has playable highlight
        [Test]
        public void PlayableCardHasPlayableBorder()
        {
            player.SetResource(ResourceType.Food, 10);
            CardPiece cardPiece = cardUnitDisplay1.GetCardPiece();
            Assert.IsTrue(player.CardIsPlayable(cardPiece));
            Assert.AreEqual(playableBorder, cardUnitDisplay1.cardBorder.sprite);
        }

        // Test non-playable card does not have highlight
        [Test]
        public void NonPlayableCardDoesNotHaveHighlight()
        {
            CardPiece cardPiece = cardUnitDisplay1.GetCardPiece();
            Assert.IsFalse(player.CardIsPlayable(cardPiece));
            Assert.AreEqual(unplayableBorder, cardUnitDisplay1.cardBorder.sprite);
        }

        // Test playable card highlights on click
        [Test]
        public void PlayableCardIsHighlightedOnClick()
        {
            player.SetResource(ResourceType.Food, 10);
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.AreEqual(highlightedBorder, cardUnitDisplay1.cardBorder.sprite);
        }

        // Test non-playable card does not highlight on click
        [Test]
        public void NonPlayableCardDoesNotHighlightOnClick()
        {
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.AreEqual(unplayableBorder, cardUnitDisplay1.cardBorder.sprite);
        }

        // Test sets selected card on playable card click
        [Test]
        public void SetsSelectedCardOnPlayableCardClick()
        {
            player.SetResource(ResourceType.Food, 10);
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.AreEqual(cardUnitDisplay1, player.selectedCard);
        }

        // Test does not set selected card on non-playable card click
        [Test]
        public void DoesNotSetSelectedCardOnNonPlayableCardClick()
        {
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.IsNull(player.selectedCard);
        }

        // Test set new selected card on 2nd card click
        [Test]
        public void SetsSelectedCardToNextCardOn2ndCardClick()
        {
            player.SetResource(ResourceType.Food, 10);
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.IsTrue(cardUnitDisplay1.IsHighlighted());
            player.DrawCard(cardUnit2);
            CardPieceDisplay cardUnitDisplay2 = player.cardPieceDisplays[1];
            player.PlayerClickOnCardInHand(cardUnitDisplay2);
            Assert.IsTrue(cardUnitDisplay2.IsHighlighted());
            Assert.AreEqual(cardUnitDisplay2, player.selectedCard);
            Assert.AreEqual(playableBorder, cardUnitDisplay1.cardBorder.sprite);
            Assert.AreEqual(highlightedBorder, cardUnitDisplay2.cardBorder.sprite);
        }

        // Test deselects card on 2nd click
        [Test]
        public void DeselectsCardOn2ndClick()
        {
            player.SetResource(ResourceType.Food, 10);
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            player.PlayerClickOnCardInHand(cardUnitDisplay1);
            Assert.AreEqual(playableBorder, cardUnitDisplay1.cardBorder.sprite);
            Assert.IsNull(player.selectedCard);
        }

        // Test set border to playable after gaining resources
        [Test]
        public void SetsBorderPlayableAfterGainingResources()
        {
            player.SetResource(ResourceType.Food, 10);
            Assert.AreEqual(playableBorder, cardUnitDisplay1.cardBorder.sprite);
        }

        // Test set border to unplayable after losing resources
        [Test]
        public void SetsBorderUnplayableAfterLosingResources()
        {
            player.SetResource(ResourceType.Food, 10);
            player.SetResource(ResourceType.Food, 0);
            Assert.AreEqual(unplayableBorder, cardUnitDisplay1.cardBorder.sprite);
        }
    }
}