using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameManagerTests
    {
        // Create game manager
        public static GameManager CreateTestGameManager() {
            GameMap gameMap = new GameMap(5);
            GameManager gameManager = new GameManager(2, gameMap);
            return gameManager;
        }

        // Test create game manager
        [Test]
        public void CreatesGameManager() {
            GameManager gameManager = CreateTestGameManager();
            Assert.IsNotNull(gameManager);
            Assert.AreEqual(2, gameManager.GetPlayers().Count);

            // Confirm players created
            Assert.IsNotNull(gameManager.GetPlayer(0));
            
            // Confirm current player is first player
            Assert.AreEqual(0, gameManager.GetCurrentTurnPlayerId());
            Assert.AreEqual(gameManager.GetPlayer(0), gameManager.GetCurrentTurnPlayer());

            // Confirm each player has castle
            for (int i = 0; i < 2; i++) {
                Player player = gameManager.GetPlayer(i);
                Assert.AreEqual(1, player.GetPieces().Count);
                Assert.AreEqual("Castle", player.GetPieces()[0].GetCard().cardName);
                Assert.AreEqual(player.GetStartTileCoords(), player.GetPieces()[0].GetGameHex().GetTileCoords());
            }
            
        }
    }
}
