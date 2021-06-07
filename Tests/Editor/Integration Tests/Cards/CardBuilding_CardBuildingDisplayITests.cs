using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;

namespace Tests.ITests.CardTests
{
    public class CardBuilding_CardBuildingDisplayITests
    {
        private CardBuildingDisplay cardBuildingDisplay;
        private Sprite unplayableBorder;
        private Sprite playableBorder;
        private Sprite highlightedBorder;

        // Create test card building display
        public static CardBuildingDisplay CreateTestCardBuildingDisplay()
        {
            CardBuilding cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
            CardBuildingDisplay cardBuildingDisplay = CardBuildingDisplayUTests.CreateTestCardBuildingDisplay();
            cardBuildingDisplay.SetCard(cardBuilding);
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

        // Test card building display created
        [Test]
        public void CardBuildingDisplayCreated()
        {
            Assert.IsNotNull(cardBuildingDisplay);
        }

        // Test sets card unit display info
        [Test]
        public void SetsCardBuildingDisplayInfo()
        {
            Assert.AreEqual(10, int.Parse(cardBuildingDisplay.cardMight.text));
            Assert.AreEqual("Test Building", cardBuildingDisplay.cardName.text);
        }

        // Test sets card unit display lifebar
        [Test]
        public void SetsLifebar()
        {
            Sprite[] lifebars = Resources.LoadAll<Sprite>(ENV.LIFEBAR_ART_RESOURCE_PATH);
            Assert.AreEqual(lifebars[9], cardBuildingDisplay.lifebarOverlay.sprite);
        }

        // Test sets card unit display resources
        [Test]
        public void SetResources()
        {
            Assert.AreEqual(10, int.Parse(cardBuildingDisplay.res1Cost.text));
            Assert.AreEqual(Resources.Load<Sprite>("Art/UI/Wood"), cardBuildingDisplay.res1.sprite);
            Assert.IsFalse(cardBuildingDisplay.res2Cost.gameObject.activeSelf);
            Assert.IsFalse(cardBuildingDisplay.res2.gameObject.activeSelf);
        }

        // Test set card playable border
        [Test]
        public void PlayableCardHasPlayableBorder()
        {
            cardBuildingDisplay.isPlayable = true;
            Assert.AreEqual(playableBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test set card unplayable border
        [Test]
        public void UnPlayableCardHasUnplayableBorder()
        {
            cardBuildingDisplay.isPlayable = false;
            Assert.AreEqual(unplayableBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test set card highlighted border
        [Test]
        public void PlayableCardCanHaveHighlightedBorder()
        {
            cardBuildingDisplay.isPlayable = true;
            cardBuildingDisplay.SetHighlighted(true);
            Assert.AreEqual(highlightedBorder, cardBuildingDisplay.cardBorder.sprite);
        }

        // Test set card highlighted border
        [Test]
        public void UnplayableCardCanNotHaveHighlightedBorder()
        {
            cardBuildingDisplay.isPlayable = false;
            cardBuildingDisplay.SetHighlighted(true);
            Assert.AreEqual(unplayableBorder, cardBuildingDisplay.cardBorder.sprite);
        }
    }
}