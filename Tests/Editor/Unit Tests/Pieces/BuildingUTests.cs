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
            Building newBuilding = TestFunctions.CreateClassObject<Building>("Assets/Resources/Prefabs/Building.prefab");
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
    }
}