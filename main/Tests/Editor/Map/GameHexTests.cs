using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameHexTests
    {
        // Creates new game hex
        private GameHex CreateGameHex() {
            TileData tileData = Resources.Load<TileData>("Tiles/Grass");
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);
            GameHex gameHex = new GameHex(tileData, hexCoords);
            return gameHex;
        }

        // Test create game hex
        [Test]
        public void GameHexCreated()
        {
            GameHex gameHex = CreateGameHex();
            Assert.IsNotNull(gameHex);
        }

        // Test neighbor coordinates
        [Test]
        public void CorrectNeighborHexCoords() {
            GameHex gameHex = CreateGameHex();
            Vector3Int neighborCoords = gameHex.GetNeighborByIndex(1);
            Assert.AreEqual(neighborCoords, new Vector3Int(0, 1, -1));

            neighborCoords = gameHex.GetNeighborByIndex(5);
            Assert.AreEqual(neighborCoords, new Vector3Int(-1, 0, 1));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GameHexTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
