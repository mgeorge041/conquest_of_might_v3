using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardTests
    {
        // Load a test unit card
        public static CardUnit LoadTestUnitCard() {
            CardUnit newCard = Resources.Load<CardUnit>("Cards/Tests/Wizard");
            return newCard;
        }

        // Load a test resource card
        public static CardResource LoadTestResourceCard() {
            CardResource newCard = Resources.Load<CardResource>("Cards/Tests/Food");
            return newCard;
        }

        // Test load test card
        [Test]
        public void LoadsTestCard() {
            Card newCard = LoadTestUnitCard();
            Assert.IsNotNull(newCard);
            Assert.AreEqual(newCard.cardName, "Wizard");
        }

        // Test load test card unit
        [Test]
        public void LoadsTestCardUnit() {
            CardUnit newCard = LoadTestUnitCard();
            Assert.IsNotNull(newCard);
            Assert.AreEqual(newCard.cardName, "Wizard");
            Assert.AreEqual(newCard.speed, 2);
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
            Card card = LoadTestUnitCard();
            Transform transform = new GameObject().transform;
            CardDisplay cardDisplay = CardDisplay.Initialize(card, transform);
            Assert.IsNotNull(cardDisplay);
            Assert.AreEqual(cardDisplay.GetCard().cardName, "Wizard");
        }
    }
}
