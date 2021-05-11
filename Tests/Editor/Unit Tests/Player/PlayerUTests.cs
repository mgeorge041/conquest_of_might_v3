using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace Tests.UTests.PlayerTests
{
    public class PlayerUTests
    {
        private Player player;
        private Vector3Int startTileCoords;

        // Create a player
        public static Player CreateTestPlayer() {
            Vector3Int startTileCoords = new Vector3Int(-4, 4, 0);
            Player player = new Player(0, startTileCoords);
            return player;
        }

        // Create a player with a specific ID
        public static Player CreateTestPlayer(int playerId)
        {
            Vector3Int startTileCoords = new Vector3Int(-4, 4, 0);
            Player player = new Player(playerId, startTileCoords);
            return player;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            player = CreateTestPlayer();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            player = null;
        }

        // Test create a player 
        [Test]
        public void PlayerCreated()
        {
            Assert.IsNotNull(player);
        }

        // Test player initialized
        [Test]
        public void PlayerInitialized() {

            // Confirm game data created
            Assert.IsNotNull(player.gameData);

            // Confirm hand and deck initialized
            Assert.IsNotNull(player.hand);
            Assert.IsNotNull(player.deck);

            // Confirm resources initialized
            Assert.IsNotNull(player.GetResources());
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Food));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Wood));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Mana));

            // Confirm maps are made
            Assert.IsNotNull(player.actionMap);
            Assert.IsNotNull(player.fogMap);
        }

        // Test get resource count
        [Test]
        public void ReturnsResourceCount() {
            Player player = CreateTestPlayer();

            // Confirm get resource count
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.Food));
            Assert.AreEqual(0, player.GetResourceCount(ResourceType.None));
        }

        // Test increase resource count
        [Test]
        public void ResourcesIncrement() {
            Player player = CreateTestPlayer();

            // Confirm adds food
            player.IncrementResource(ResourceType.Food, 1);
            Assert.AreEqual(1, player.GetResourceCount(ResourceType.Food));
        }

        // Test decrease resource count
        [Test]
        public void ResourcesBottomAtZero() {
            Player player = CreateTestPlayer();
                
            // Confirm counts don't drop below 0
            player.IncrementResource(ResourceType.Food, -1);
            Assert.GreaterOrEqual(0, player.GetResourceCount(ResourceType.Food));
        }

        // Test start first turn
        [Test]
        public void StartsFirstTurn() {
            Player player = CreateTestPlayer();
            int deckSize = player.GetDeckSize();
            player.StartFirstTurn();

            // Confirm 15 cards are drawn
            Assert.AreEqual(deckSize - 15, player.GetDeckSize());
        }

        // Test start turn 
        [Test]
        public void StartsTurn() {
            Player player = CreateTestPlayer();
            int deckSize = player.GetDeckSize();
            player.StartTurn();

            // Confirm one card is drawn
            Assert.AreEqual(deckSize - 1, player.GetDeckSize());
        }

        // Test draws a card
        [Test]
        public void DrawsCard() {
            Player player = CreateTestPlayer();
            Card drawnCard = player.DrawTopCard();
            Assert.IsNotNull(drawnCard);
        }
    }
}
