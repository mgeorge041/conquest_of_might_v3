using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DeckTests
    {
        // Create a deck
        public static Deck CreateDeck() {
            Deck deck = new Deck();
            return deck;
        }

        // Test deck is created
        [Test]
        public void DeckCreated() {
            Deck deck = CreateDeck();
            Assert.IsNotNull(deck);
            Assert.AreEqual(20, deck.GetCardCount());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DeckTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
