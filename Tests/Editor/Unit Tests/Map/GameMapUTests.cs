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
        private GameMap gameMap;

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
        }
    }
}
