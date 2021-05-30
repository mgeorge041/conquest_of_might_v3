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
            cardBuildingDisplay.SetPlayableBorder();
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background"), cardBuildingDisplay.cardBorder.sprite);
        }

        // Test setting unplayable border
        [Test]
        public void SetsUnplayableBorder()
        {
            cardBuildingDisplay.SetUnplayableBorder();
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background Red"), cardBuildingDisplay.cardBorder.sprite);
        }

        // Test setting highlighted border
        [Test]
        public void SetsHighlightedBorder()
        {
            cardBuildingDisplay.SetHighlighted(true);
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background Highlight"), cardBuildingDisplay.cardBorder.sprite);
        }
    }
}