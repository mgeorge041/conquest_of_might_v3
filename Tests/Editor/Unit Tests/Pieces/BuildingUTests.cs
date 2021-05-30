using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.PieceTests
{
    public class BuildingUTests
    {
        private Building building1;

        // Create test building
        public static Building CreateTestBuilding()
        {
            Building newBuilding = TestFunctions.CreateClassObject<Building>(ENV.BUILDING_PREFAB_FULL_PATH);
            return newBuilding;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            building1 = CreateTestBuilding();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(building1.gameObject);
        }

        // Test create unit
        [Test]
        public void CreatesBuilding()
        {
            Assert.IsNotNull(building1);
        }

        // Test building is created with actions
        [Test]
        public void BuildingStartsWithActions()
        {
            Assert.IsTrue(building1.canAttack);
            Assert.IsTrue(building1.hasActions);
        }

        // Test end turn
        [Test]
        public void EndsTurn()
        {
            building1.EndTurn();
            Assert.IsFalse(building1.canAttack);
            Assert.IsFalse(building1.hasActions);
        }

        // Test reset piece
        [Test]
        public void ResetsPiece()
        {
            building1.ResetPiece();
            Assert.IsTrue(building1.canAttack);
            Assert.IsTrue(building1.hasActions);
        }
    }
}