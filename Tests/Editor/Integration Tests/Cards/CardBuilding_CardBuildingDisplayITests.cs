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
        private CardBuilding cardBuilding;
        private CardBuildingDisplay cardBuildingDisplay;

        // Setup
        [SetUp]
        public void Setup()
        {
            cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
            cardBuildingDisplay = CardBuildingDisplayUTests.CreateTestCardBuildingDisplay();
            cardBuildingDisplay.SetCard(cardBuilding);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(cardBuildingDisplay.gameObject);
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
            Assert.AreEqual(Resources.Load<Sprite>("Art/Cards/Might Icon"), cardBuildingDisplay.res1.sprite);
            Assert.IsFalse(cardBuildingDisplay.res2Cost.gameObject.activeSelf);
            Assert.IsFalse(cardBuildingDisplay.res2.gameObject.activeSelf);
        }
    }
}