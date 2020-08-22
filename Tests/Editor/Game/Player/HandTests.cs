using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HandTests
    {
        // Create hand 
        public static Hand CreateHand() {
            Player player = PlayerTests.CreateTestPlayer();
            Hand hand = new Hand(player);
            return hand;
        }

        // Test hand adds unit card
        [Test]
        public void AddsUnitCard() {
            Hand hand = CreateHand();
            Card newCard = CardTests.LoadTestUnitCard();
            hand.AddCard(newCard);

            // Confirm added card is the wizard
            List<CardPiece> cards = hand.GetCards();
            Assert.AreEqual(1, cards.Count);
            Assert.AreEqual(CardTests.LoadTestUnitCard().cardName, cards[0].cardName);        }

        // Test hand adds resource card
        [Test]
        public void AddsResourceCard() {
            Hand hand = CreateHand();
            Card newCard = CardTests.LoadTestResourceCard();
            hand.AddCard(newCard);

            // Confirm added card is food
            List<CardPiece> cards = hand.GetCards();
            Assert.AreEqual(0, cards.Count);
        }

        // Test hand collapses
        [Test]
        public void HandCollapses() {
            Hand hand = CreateHand();

            // Set initial transform position
            Transform handObject = new GameObject().transform;
            float height = 270;
            Vector3 newPosition = new Vector3(0, height);
            handObject.position = newPosition;
            Assert.AreEqual(handObject.position, newPosition);

            // Confirm collapses to correct location
            Hand.CollapseHand(handObject, height);
            Assert.AreEqual(handObject.position.y, -260);
        }

        // Test hand expands
        public void HandExpands() {
            Hand hand = CreateHand();

            // Set initial transform position
            Transform handObject = new GameObject().transform;
            float height = 270;
            Vector3 newPosition = new Vector3(0, height);
            handObject.position = newPosition;
            Assert.AreEqual(handObject.position, newPosition);

            // Confirm collapses then expands back to correct location
            Hand.CollapseHand(handObject, height);
            Hand.ExpandHand(handObject, height);
            Assert.AreEqual(handObject.position.y, height);
        }







        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator HandTestsWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
