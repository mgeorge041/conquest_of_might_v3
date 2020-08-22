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
        private GameManager CreateGameManager() {
            GameMap gameMap = new GameMap();
            GameManager gameManager = new GameManager(gameMap);
            return gameManager;
        }

        // Test create game manager
        [Test]
        public void CreatesGameManager() {
            GameManager gameManager = CreateGameManager();
            Assert.IsNotNull(gameManager);
            Assert.AreEqual(2, gameManager.GetPlayers().Count);
            
            // Confirm current player is first player
            Assert.AreEqual(0, gameManager.GetCurrentTurnPlayerId());
            Assert.AreEqual(gameManager.GetPlayer(0), gameManager.GetCurrentTurnPlayer());
        }

        // Test player turns
        [Test]
        public void GoesToNextPlayerTurn() {
            GameManager gameManager = CreateGameManager();
            gameManager.StartGame();
            gameManager.GetPlayer(0).EndTurn();

            // Confirm has moved on to next player
            Assert.AreEqual(1, gameManager.GetCurrentTurnPlayerId());
            Assert.AreEqual(gameManager.GetPlayer(1), gameManager.GetCurrentTurnPlayer());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GameManagerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
