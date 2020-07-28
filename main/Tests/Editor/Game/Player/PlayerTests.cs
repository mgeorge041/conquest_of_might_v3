using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTests
    {
        // Create a player
        public static Player CreateTestPlayer() {
            GameMap gameMap = new GameMap();
            Vector3Int startTileCoords = new Vector3Int(-4, 4, 0);
            Player player = new Player(0, gameMap, startTileCoords);
            player.Initialize();
            player.ClearResources();
            return player;
        }

        // Test create a player controller
        [Test]
        public void PlayerCreated()
        {
            Player player = CreateTestPlayer();
            Assert.IsNotNull(player);
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

        // Test set selected card
        [Test]
        public void GetsSelectedCard() {
            Player player = CreateTestPlayer();

            // Confirm gets card
            CardPieceDisplay newCard = Resources.Load<CardPieceDisplay>("Prefabs/Card Unit Display");
            player.SetSelectedCard(newCard);
            Assert.IsNotNull(player.GetSelectedCard());
            Assert.IsTrue(player.HasSelectedCard());
        }

        // Test create initialized player
        [Test]
        public void CreatesInitializedPlayer() {
            Player player = CreateTestPlayer();
            player.InitializeGameSetup();

            Assert.AreEqual(2, player.GetPieces().Count);
            Assert.AreEqual("Castle", player.GetPieces()[0].GetCard().cardName);
            Assert.IsTrue(player.GetHand().GetCards().Count > 0);
        }

        // Test create piece
        [Test]
        public void CreatesPieceAtTile() {
            Player player = CreateTestPlayer();

            // Play piece at start location
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);
            CardUnit testCard = Card.LoadTestUnitCard();
            player.PlayCardAtTile(testCard, tileCoords);
            Assert.IsNotNull(player.gameMap.GetHexAtTileCoords(tileCoords).GetPiece());
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

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
