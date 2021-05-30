using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.PieceTests
{
    public class UnitUTests
    {
        private Unit unit1;

        // Create test unit
        public static Unit CreateTestUnit()
        {
            Unit newUnit = TestFunctions.CreateClassObject<Unit>(ENV.UNIT_PREFAB_FULL_PATH);
            return newUnit;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            unit1 = CreateTestUnit();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(unit1.gameObject);
        }

        // Test create unit
        [Test]
        public void CreatesUnit()
        {
            Assert.IsNotNull(unit1);
        }

        // Test unit is created with actions
        [Test]
        public void UnitStartsWithActions()
        {
            Assert.IsTrue(unit1.canAttack);
            Assert.IsTrue(unit1.canMove);
            Assert.IsTrue(unit1.hasActions);
        }

        // Test end turn
        [Test]
        public void EndsTurn()
        {
            unit1.EndTurn();
            Assert.IsFalse(unit1.canAttack);
            Assert.IsFalse(unit1.canMove);
            Assert.IsFalse(unit1.hasActions);
        }

        // Test reset piece
        [Test]
        public void ResetsPiece()
        {
            unit1.ResetPiece();
            Assert.IsTrue(unit1.canAttack);
            Assert.IsTrue(unit1.canMove);
            Assert.IsTrue(unit1.hasActions);
        }
    }
}