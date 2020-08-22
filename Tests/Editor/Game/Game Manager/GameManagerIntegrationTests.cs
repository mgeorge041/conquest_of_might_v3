using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameManagerIntegrationTests
    {
        // Test player turns
        [Test]
        public void GoesToNextPlayerTurn() {
            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            gameManager.StartGame();
            gameManager.GetPlayer(0).EndTurn();

            // Confirm has moved on to next player
            Assert.AreEqual(1, gameManager.GetCurrentTurnPlayerId());
            Assert.AreEqual(gameManager.GetPlayer(1), gameManager.GetCurrentTurnPlayer());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GameManagerIntegrationTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
