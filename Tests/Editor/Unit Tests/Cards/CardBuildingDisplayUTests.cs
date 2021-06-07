using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.CardTests
{
    public class CardBuildingDisplayUTests
    {
        private CardBuildingDisplay cardBuildingDisplay;
        private Sprite unplayableBorder;
        private Sprite playableBorder;
        private Sprite highlightedBorder;

        // Create test card unit display
        public static CardBuildingDisplay CreateTestCardBuildingDisplay()
        {
            CardBuildingDisplay cardBuildingDisplay = TestFunctions.CreateClassObject<CardBuildingDisplay>(ENV.CARD_BUILDING_DISPLAY_PREFAB_FULL_PATH);
            return cardBuildingDisplay;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            cardBuildingDisplay = CreateTestCardBuildingDisplay();
            Sprite[] cardBackgrounds = Resources.LoadAll<Sprite>("Art/Cards/Card Backgrounds");
            playableBorder = cardBackgrounds[1];
            unplayableBorder = Resources.Load<Sprite>(ENV.CARD_UNPLAYABLE_BORDER_RESOURCE_PATH);
            highlightedBorder = Resources.Load<Sprite>(ENV.CARD_HIGHLIGHTED_BORDER_RESOURCE_PATH);
        }

        // End 
        [TearDown]
        public void Teardown()
        {
            C.Destroy(cardBuildingDisplay.gameObject);
        }

        // Test creates card building display
        [Test]
        public void CreatesCardUnitDisplay()
        {
            Assert.IsNotNull(cardBuildingDisplay);
        }

        // Test setting playable border
        [Test]
        public void SetsPlayableBorder()
        {
            cardBuildingDisplay.isPlayable = true;
            Assert.AreEqual(playableBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test setting unplayable border
        [Test]
        public void SetsUnplayableBorder()
        {
            cardBuildingDisplay.isPlayable = false;
            Assert.AreEqual(unplayableBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test setting highlighted border for playable card
        [Test]
        public void SetsHighlightedBorderForPlayableCard()
        {
            cardBuildingDisplay.isPlayable = true;
            cardBuildingDisplay.SetHighlighted(true);
            Assert.AreEqual(highlightedBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test does not set highlighted border for unplayable card
        [Test]
        public void DoesNotSetHighlightedBorderForUnplayableCard()
        {
            cardBuildingDisplay.isPlayable = false;
            cardBuildingDisplay.SetHighlighted(true);
            Assert.AreEqual(unplayableBorder, cardBuildingDisplay.cardBorder.sprite);
        }
    }
}