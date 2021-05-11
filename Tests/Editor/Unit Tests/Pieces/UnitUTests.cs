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
            Unit newUnit = TestFunctions.CreateClassObject<Unit>("Assets/Resources/Prefabs/Unit.prefab");
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
    }
}