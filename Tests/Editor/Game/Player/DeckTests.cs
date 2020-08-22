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
        public static Deck CreateTestDeck() {
            Deck deck = new Deck();
            return deck;
        }

        // Test deck is created
        [Test]
        public void DeckCreated() {
            Deck deck = CreateTestDeck();
            Assert.IsNotNull(deck);
            Assert.AreEqual(20, deck.GetCardCount());
        }
    }
}
