using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace Tests.UTests.MapTests
{
    public class ActionMapUTests
    {
        private ActionMap actionMap;

        // Create test action map
        public static ActionMap CreateTestActionMap()
        {
            ActionMap newActionMap = TestFunctions.CreateClassObject<ActionMap>("Assets/Resources/Prefabs/Action Map.prefab");
            return newActionMap;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            actionMap = CreateTestActionMap();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            actionMap = null;
        }

        // Test creates action map
        [Test]
        public void CreatesActionMap()
        {
            Assert.IsNotNull(actionMap);
        }

        // Test movement tile is set
        [Test]
        public void MovementTileIsNotNull()
        {
            Assert.IsNotNull(actionMap.movementTile);
            Assert.AreEqual(Resources.Load<Tile>(ENV.MOVE_TILE_RESOURCE_PATH), actionMap.movementTile);
        }

        // Test attack tile is set
        [Test]
        public void AttackTileIsNotNull()
        {
            Assert.IsNotNull(actionMap.attackTile);
            Assert.AreEqual(Resources.Load<Tile>(ENV.ATTACK_TILE_RESOURCE_PATH), actionMap.attackTile);
        }

        // Test playable tile is set
        [Test]
        public void PlayableTileIsNotNull()
        {
            Assert.IsNotNull(actionMap.playableTile);
            Assert.AreEqual(Resources.Load<Tile>(ENV.PLAY_TILE_RESOURCE_PATH), actionMap.playableTile);
        }
    }
}