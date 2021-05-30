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
            cardUnitDisplay.SetPlayableBorder();
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background"), cardUnitDisplay.cardBorder.sprite);
        }

        // Test setting unplayable border
        [Test]
        public void SetsUnplayableBorder()
        {
            cardUnitDisplay.SetUnplayableBorder();
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background Red"), cardUnitDisplay.cardBorder.sprite);
        }

        // Test setting highlighted border
        [Test]
        public void SetsHighlightedBorder()
        {
            cardUnitDisplay.SetHighlighted(true);
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Input Background Highlight"), cardUnitDisplay.cardBorder.sprite);
        }
    }
}