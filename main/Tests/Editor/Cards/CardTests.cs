using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardTests
    {
        // Test load test card
        [Test]
        public void LoadsTestCard() {
            Card newCard = Card.LoadTestUnitCard();
            Assert.IsNotNull(newCard);
            Assert.AreEqual(newCard.cardName, "Wizard");
        }

        // Test load test card unit
        [Test]
        public void LoadsTestCardUnit() {
            CardUnit newCard = Card.LoadTestUnitCard();
            Assert.IsNotNull(newCard);
            Assert.AreEqual(newCard.cardName, "Wizard");
            Assert.AreEqual(newCard.speed, 1);
        }

        // Test load card
        [Test]
        public void LoadsCardFromResources()
        {
            Race cardRace = Race.Human;
            CardType cardType = CardType.Unit;
            string cardName = "Wizard";
            Card card = Card.LoadCard(cardRace, cardType, cardName);
            Assert.IsNotNull(card);
            Assert.AreEqual(card.cardName, "Wizard");
        }

        // Test initialize card
        [Test]
        public void InitializesCard() {
            Card card = Card.LoadTestUnitCard();
            Transform transform = new GameObject().transform;
            CardDisplay cardDisplay = Card.Initialize(card, transform);
            Assert.IsNotNull(cardDisplay);
            Assert.AreEqual(cardDisplay.GetCard().cardName, "Wizard");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CardTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
