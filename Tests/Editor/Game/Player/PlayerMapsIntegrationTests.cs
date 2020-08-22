using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerMapsIntegrationTests
    {
        // Create a player with a game map
        public static Player CreateTestPlayerWithGameMap() {
            GameMap gameMap = new GameMap();
            Vector3Int startTileCoords = new Vector3Int(-4, 4, 0);
            Player player = new Player(0, startTileCoords, gameMap);
            return player;
        }

        // Test setting selected card
        [Test]
        public void SetsSelectedCard() {
            Player player = CreateTestPlayerWithGameMap();
            CardPiece testCard = CardTests.LoadTestUnitCard();
            player.SetSelectedCard(testCard);

            // Confirm selected card is set
            Assert.AreEqual(testCard, player.GetSelectedCard());
            Assert.IsTrue(player.HasSelectedCard());
            Assert.IsFalse(player.HasSelectedPiece());
        }

        // Test setting selected piece
        [Test]
        public void SetsSelectedPiece() {
            Player player = CreateTestPlayerWithGameMap();
            GamePiece testPiece = GamePieceTests.CreateTestUnitForPlayer(player);
            player.gameMap.AddPiece(testPiece, new Vector3Int(0, 0, 0));
            player.SetSelectedPiece(testPiece);

            // Confirm selected piece is set
            Assert.AreEqual(testPiece, player.GetSelectedPiece());
            Assert.IsTrue(player.HasSelectedPiece());
            Assert.IsFalse(player.HasSelectedCard());
        }
    }
}
