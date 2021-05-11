using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

namespace Tests.UTests.PlayerTests
{
    public class PlayerGameDataUTests
    {
        private PlayerGameData playerGameData;

        // Create a player game data object
        public static PlayerGameData CreateTestPlayerGameData() {
            PlayerGameData gameData = new PlayerGameData();
            return gameData;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            playerGameData = CreateTestPlayerGameData();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            playerGameData = null;
        }

        // Test creates player game data
        [Test]
        public void CreatesPlayerGameData()
        {
            Assert.IsNotNull(playerGameData);
        }

        // Test adding defeated pieces
        [Test]
        public void AddsDefeatedPieces()
        {
            playerGameData.AddPiecesDefeated(5);
            Assert.AreEqual(5, playerGameData.numPiecesDefeated);
        }

        // Test adding damage given
        [Test]
        public void AddsDamageGiven()
        {
            playerGameData.AddDamageGiven(5);
            Assert.AreEqual(5, playerGameData.damageGiven);
        }

        // Test adding damage taken
        [Test]
        public void AddsDamageTaken()
        {
            playerGameData.AddDamageTaken(5);
            Assert.AreEqual(5, playerGameData.damageTaken);
        }

        // Test adding resource
        [Test]
        public void AddsCollectedResource()
        {
            playerGameData.AddCollectedResource(ResourceType.Food);
            Assert.AreEqual(1, playerGameData.collectedResources[ResourceType.Food]);
        }
    }
}
