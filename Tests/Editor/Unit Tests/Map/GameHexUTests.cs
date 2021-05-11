using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PieceTests;

namespace Tests.UTests.MapTests
{
    public class GameHexUTests
    {
        // Coordinates for neighbor hexes
        private Vector3Int[] neighborCoords =
        {
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, -1),
            new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 1),
            new Vector3Int(-1, 0, 1)
        };
        private GameHex gameHex;

        // Creates new game hex
        public static GameHex CreateTestGameHex() 
        {
            GameHex newGameHex = new GameHex(TileUTests.CreateTestTileData(), Vector3Int.zero);
            return newGameHex;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            gameHex = CreateTestGameHex();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            gameHex = null;
        }

        // Test create game hex
        [Test]
        public void GameHexCreated()
        {
            Assert.IsNotNull(gameHex);
        }

        // Test neighbor coordinates
        [Test]
        public void CorrectNeighborHexCoords() 
        {    
            for (int i = 0; i < gameHex.neighborHexCoords.Length; i++)
            {
                Assert.AreEqual(neighborCoords[i], gameHex.GetNeighborByIndex(i));
            }
        }
    }
}
