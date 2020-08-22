using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameSimulationTests
    {
        // Test simulated game
        [Test]
        public void GameRuns() {
            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            gameManager.StartGame();

            // Confirm is first player's turn
            Assert.IsTrue(gameManager.GetPlayer(0).isTurn);
        }
    }
}
