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
        private CardUnit cardUnit;
        private CardUnitDisplay cardUnitDisplay;

        // Setup
        [SetUp]
        public void Setup()
        {
            cardUnit = CardUnitUTests.CreateTestCardUnit();
            cardUnitDisplay = CardUnitDisplayUTests.CreateTestCardUnitDisplay();
            cardUnitDisplay.SetCard(cardUnit);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(cardUnitDisplay.gameObject);
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
    }
}