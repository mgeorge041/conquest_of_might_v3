using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Tests.UTests.MapTests
{
    public class GameMapUTests
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
        private GameMap gameMap;
        private Vector3Int hexCoords;
        private List<Vector3Int> hexCoordsInRange;

        // Create game map
        public static GameMap CreateTestGameMap()
        {
            GameMap gameMap = TestFunctions.CreateClassObject<GameMap>("Assets/Prefabs/Map/Game Map.prefab");
            gameMap.Initialize();
            return gameMap;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = CreateTestGameMap();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(gameMap.gameObject);
        }

        // Test create game map
        [Test]
        public void CreatesGameMap()
        {
            Assert.IsNotNull(gameMap);
            Assert.AreEqual(Vector3.zero, gameMap.transform.position);
        }

        // Test get hex coords within 1 range
        [Test]
        public void GetsCorrectHexCoordsIn1Range()
        {
            hexCoordsInRange = gameMap.GetHexCoordsInRange(hexCoords, 1);
            for (int i = 0; i < neighborCoords.Length; i++)
            {
                Assert.Contains(neighborCoords[i], hexCoordsInRange);
            }
        }

        // Test get hex
        [Test]
        public void GetsHex()
        {
            GameHex gameHex = gameMap.GetHexAtHexCoords(Vector3Int.zero);
            Assert.IsNotNull(gameHex);
        }
    }
}
