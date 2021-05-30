using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests;

namespace Tests.UTests.PlayerTests
{
    public class PlayerHandTests
    {
        // Test drawing a resource card
        [Test]
        public void IncrementsResourceWhenCardDrawn() {
            Player player = PlayerUTests.CreateTestPlayer();
            Card testResourceCard = Tests.CardTests.LoadTestResourceCard();

            player.DrawCard(testResourceCard);
            Assert.AreEqual(1, player.GetResourceCount(testResourceCard.res1));
        }

        // Test playing a card
        [Test]
        public void RemovesCardFromHandWhenPlayed() {
            Player player = PlayerUTests.CreateTestPlayer();
            player.DrawStartingHand();
            player.SetResource(ResourceType.Food, 5);

            // Set selected card as first in hand
            CardPiece cardPiece = player.hand.cards[0];
            player.SetSelectedCard(cardPiece);
            int numCards = player.hand.cards.Count;

            // Get current food count and play card
            int foodCount = player.GetResourceCount(ResourceType.Food);
            player.gameMap.AddPiece(GamePiece.CreatePiece<GamePiece>(cardPiece, player), new Vector3Int(0, 0, 0));

            // Confirm card was removed from hand and resources decremented
            Assert.AreEqual(numCards - 1, player.hand.cards.Count);
            Assert.AreEqual(foodCount - 1, player.GetResourceCount(ResourceType.Food));
        }

        // Test incrementing game data cards drawn
        [Test]
        public void DrawsCard() {
            Player player = PlayerUTests.CreateTestPlayer();
            player.DrawTopCard();

            // Confirm card was drawn
        }
    }
}
