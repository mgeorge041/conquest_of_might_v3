using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerHandTests
    {
        // Test drawing a resource card
        [Test]
        public void IncrementsResourceWhenCardDrawn() {
            Player player = PlayerTests.CreateTestPlayer();
            Card testResourceCard = Card.LoadTestResourceCard();

            player.DrawCard(testResourceCard);
            Assert.AreEqual(1, player.GetResourceCount(testResourceCard.res1));
        }

        // Test playing a card
        [Test]
        public void RemovesCardFromHandWhenPlayed() {
            Player player = PlayerTests.CreateTestPlayer();
            player.DrawStartingHand();
            player.SetResource(ResourceType.Food, 5);

            // Set selected card as first in hand
            CardPieceDisplay cardPieceDisplay = player.GetHand().GetCards()[0];
            player.SetSelectedCard(cardPieceDisplay);

            // Get current food count and play card
            int foodCount = player.GetResourceCount(ResourceType.Food);
            player.PlaySelectedCardAtTile(new Vector3Int(0, 0, 0));

            // Confirm card was removed from hand and resources decremented
            Assert.AreNotEqual(cardPieceDisplay, player.GetHand().GetCards()[0]);
            Assert.AreEqual(foodCount - 1, player.GetResourceCount(ResourceType.Food));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerHandTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
