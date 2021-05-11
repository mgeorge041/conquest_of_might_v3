using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Tests.UTests.CardTests;

namespace Tests.UTests.PlayerTests
{
    public class PlayerIntegrationTests
    {
        // Test player initialization
        [Test]
        public void PlayerDrawsStartingHand() {
            Player player = PlayerUTests.CreateTestPlayer();
            int deckSize = player.GetDeckSize();
            player.DrawStartingHand();

            // Confirm game data updated
            PlayerGameData gameData = player.gameData;
            Assert.AreEqual(15, gameData.numDrawnCards);

            // Confirm hand and deck updated
            Assert.Greater(player.hand.cards.Count, 0);
            Assert.AreEqual(deckSize - 15, player.GetDeckSize());
        }

        // Test player updates on card drawn
        [Test]
        public void CorrectlyDrawsCard() {
            Player player = PlayerUTests.CreateTestPlayer();
            int deckSize = player.GetDeckSize();
            Card drawnCard = player.DrawTopCard();

            // Confirm game data updated
            PlayerGameData gameData = player.gameData;
            Assert.AreEqual(1, gameData.numDrawnCards);
            Assert.AreEqual(1, gameData.drawnCardTypes[drawnCard.cardType]);
            Assert.AreEqual(1, gameData.drawnCardRaces[drawnCard.race]);

            // Confirm hand and deck have correct counts
            if (drawnCard.cardType != CardType.Resource) {
                Assert.AreEqual(1, player.hand.cards.Count);
            }
            Assert.AreEqual(deckSize - 1, player.GetDeckSize());
        }
        /*
        // Test playing piece
        [Test]
        public void PlaysPiece() {
            Player player = PlayerUTests.CreateTestPlayerWithGameMap();
            CardPiece testCard = CardUnitUTests.LoadTestCardUnit();
            GameMap gameMap = player.gameMap;
            GamePiece newPiece = GamePiece.CreatePiece(testCard, player);
            gameMap.AddPiece(newPiece, new Vector3Int(0, 0, 0));

            // Confirm piece is added to player list
            Assert.AreEqual(1, player.GetPieces().Count);

            // Confirm player game data updated
            PlayerGameData gameData = player.GetPlayerGameData();
            Assert.AreEqual(1, gameData.numPlayedCards);
            Assert.AreEqual(1, gameData.playedCardTypes[CardType.Unit]);
            Assert.AreEqual(1, gameData.playedCardRaces[Race.Magic]);

            // Confirm piece is added to map
            Assert.IsNotNull(newPiece.gameHex);
        }

        // Test set selected card
        [Test]
        public void SetsSelectedCard() {
            Player player = PlayerUTests.CreateTestPlayerWithGameMap();
            CardPiece newCard = CardUnitUTests.LoadTestCardUnit();
            player.SetSelectedCard(newCard);

            // Confirm sets card
            Assert.IsNotNull(player.GetSelectedCard());
            Assert.IsTrue(player.HasSelectedCard());

            // Confirm piece is unselected
            Assert.IsNull(player.GetSelectedPiece());
            Assert.IsFalse(player.HasSelectedPiece());
        }

        // Test set selected piece
        [Test]
        public void SetsSelectedPiece() {
            Player player = PlayerUTests.CreateTestPlayerWithGameMap();
            CardPiece testCard = CardUnitUTests.LoadTestCardUnit();
            GameMap gameMap = player.gameMap;
            GamePiece newPiece = GamePiece.CreatePiece(testCard, player);
            gameMap.AddPiece(newPiece, new Vector3Int(0, 0, 0));

            player.SetSelectedPiece(newPiece);

            // Confirm piece is unselected
            Assert.IsNotNull(player.GetSelectedPiece());
            Assert.IsTrue(player.HasSelectedPiece());

            // Confirm card is unselected
            Assert.IsNull(player.GetSelectedCard());
            Assert.IsFalse(player.HasSelectedCard());
        }
        */
    }
}
