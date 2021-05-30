using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.PlayerTests
{
    public class DeckUTests
    {
        private Deck deck;

        // Create a deck
        public static Deck CreateTestDeck() {
            Deck deck = new Deck();
            return deck;
        }

        // Get number of test cards
        private int GetNumberTestCards()
        {
            Card[] cards = Resources.LoadAll<Card>(ENV.CARDS_RESOURCE_PATH);
            return cards.Length;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            deck = CreateTestDeck();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            deck = null;
        }

        // Test deck is created
        [Test]
        public void DeckCreated() { 
            // Confirm deck created
            Assert.IsNotNull(deck);
            Assert.AreEqual(10 * GetNumberTestCards(), deck.cardCount);

            // Confirm backup deck created
            Assert.IsNotNull(deck.backupDeck);
            Assert.AreEqual(10 * GetNumberTestCards(), deck.cardCount);
        }

        // Test custom deck creation
        [Test]
        public void CustomDeckCreated()
        {
            // Create list of custom cards
            int numCards = 3;
            Card[] testCards = Resources.LoadAll<Card>(ENV.CARDS_RESOURCE_PATH);
            Dictionary<Card, int> deckCards = new Dictionary<Card, int>();
            foreach (Card card in testCards)
            {
                deckCards[card] = numCards;
            }

            // Create deck
            Deck deck = new Deck(deckCards);

            // Confirm deck created
            Assert.IsNotNull(deck);
            Assert.AreEqual(numCards * testCards.Length, deck.cardCount);

            // Confirm backup deck created
            Assert.IsNotNull(deck.backupDeck);
            Assert.AreEqual(numCards * testCards.Length, deck.cardCount);
        }
    }
}
