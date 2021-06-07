using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;

namespace Tests.ITests.CardTests
{
    public class CardUnit_CardUnitDisplayITests
    {
        private CardUnitDisplay cardUnitDisplay;
        private Sprite unplayableBorder;
        private Sprite playableBorder;
        private Sprite highlightedBorder;

        // Create test card unit
        public static CardUnitDisplay CreateTestCardUnitDisplay()
        {
            CardUnit cardUnit = CardUnitUTests.CreateTestCardUnit();
            CardUnitDisplay cardUnitDisplay = CardUnitDisplayUTests.CreateTestCardUnitDisplay();
            cardUnitDisplay.SetCard(cardUnit);
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

        // Test card unit display created
        [Test]
        public void CardUnitDisplayCreated()
        {
            Assert.IsNotNull(cardUnitDisplay);
        }

        // Test sets card unit display info
        [Test]
        public void SetsCardUnitDisplayInfo()
        {
            Assert.AreEqual(5, int.Parse(cardUnitDisplay.cardMight.text));
            Assert.AreEqual("Test Unit", cardUnitDisplay.cardName.text);
        }

        // Test sets card unit display lifebar
        [Test]
        public void SetsLifebar()
        {
            Sprite[] lifebars = Resources.LoadAll<Sprite>(ENV.LIFEBAR_ART_RESOURCE_PATH);
            Assert.AreEqual(cardUnitDisplay.lifebarOverlay.sprite, lifebars[5]);
        }

        // Test sets card unit display resources
        [Test]
        public void SetResources()
        {
            Assert.AreEqual(5, int.Parse(cardUnitDisplay.res1Cost.text));
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Food"), cardUnitDisplay.res1.sprite);
            Assert.IsFalse(cardUnitDisplay.res2Cost.gameObject.activeSelf);
            Assert.IsFalse(cardUnitDisplay.res2.gameObject.activeSelf);
        }

        // Test set card playable border
        [Test]
        public void PlayableCardHasPlayableBorder()
        {
            cardUnitDisplay.isPlayable = true;
            Assert.AreEqual(playableBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test set card unplayable border
        [Test]
        public void UnPlayableCardHasUnplayableBorder()
        {
            cardUnitDisplay.isPlayable = false;
            Assert.AreEqual(unplayableBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test set card highlighted border
        [Test]
        public void PlayableCardCanHaveHighlightedBorder()
        {
            cardUnitDisplay.isPlayable = true;
            cardUnitDisplay.SetHighlighted(true);
            Assert.AreEqual(highlightedBorder, cardUnitDisplay.cardBorder.sprite);
        }

        // Test set card highlighted border
        [Test]
        public void UnplayableCardCanNotHaveHighlightedBorder()
        {
            cardUnitDisplay.isPlayable = false;
            cardUnitDisplay.SetHighlighted(true);
            Assert.AreEqual(unplayableBorder, cardUnitDisplay.cardBorder.sprite);
        }
    }
}