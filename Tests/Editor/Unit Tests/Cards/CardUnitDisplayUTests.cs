using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests
{
    public class CardUnitDisplayUTests
    {
        private CardUnitDisplay cardUnitDisplay;
        private Sprite unplayableBorder;
        private Sprite playableBorder;
        private Sprite highlightedBorder;

        // Create test card unit display
        public static CardUnitDisplay CreateTestCardUnitDisplay()
        {
            CardUnitDisplay cardUnitDisplay = TestFunctions.CreateClassObject<CardUnitDisplay>("Assets/Resources/Prefabs/Card Unit Display.prefab");
            return cardUnitDisplay;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            cardUnitDisplay = CreateTestCardUnitDisplay();
            Sprite[] cardBackgrounds = Resources.LoadAll<Sprite>("Art/Cards/Card Backgrounds");
            playableBorder = cardBackgrounds[1];
            unplayableBorder = Resources.Load<Sprite>(ENV.CARD_UNPLAYABLE_BORDER_RESOURCE_PATH);
            highlightedBorder = Resources.Load<Sprite>(ENV.CARD_HIGHLIGHTED_BORDER_RESOURCE_PATH);
        }

        // End 
        [TearDown]
        public void Teardown()
        {
            C.Destroy(cardUnitDisplay.gameObject);
        }

        // Test creates card unit display
        [Test]
        public void CreatesCardUnitDisplay()
        {
            Assert.IsNotNull(cardUnitDisplay);
        }

        // Test setting playable border
        [Test]
        public void SetsPlayableBorder()
        {
            cardUnitDisplay.isPlayable = true;
            Assert.AreEqual(playableBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test setting unplayable border
        [Test]
        public void SetsUnplayableBorder()
        {
            cardUnitDisplay.isPlayable = false;
            Assert.AreEqual(unplayableBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test setting highlighted border for playable card
        [Test]
        public void SetsHighlightedBorderForPlayableCard()
        {
            cardUnitDisplay.isPlayable = true;
            cardUnitDisplay.SetHighlighted(true);
            Assert.AreEqual(highlightedBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test does not set highlighted border for unplayable card
        [Test]
        public void DoesNotSetHighlightedBorderForUnplayableCard()
        {
            cardUnitDisplay.isPlayable = false;
            cardUnitDisplay.SetHighlighted(true);
            Assert.AreEqual(unplayableBorder, cardUnitDisplay.cardBorder.sprite);
        }
    }
}