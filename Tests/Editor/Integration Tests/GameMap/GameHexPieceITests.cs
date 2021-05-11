using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.ITests.PieceTests;

namespace Tests.ITests.MapTests
{
    public class GameHexPieceITests
    {
        private GameHex gameHex;
        private Unit unit;
        private Building building;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameHex = GameHexUTests.CreateTestGameHex();
            unit = UnitCardUnitITests.CreateTestUnitWithCard();
            building = BuildingCardBuildingITests.CreateTestBuildingWithCard();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            gameHex = null;
            C.Destroy(unit.gameObject);
            C.Destroy(building.gameObject);
        }

        // Test adding unit
        [Test]
        public void AddsUnit()
        {
            gameHex.piece = unit;
            Assert.IsTrue(gameHex.HasPiece());
        }

        // Test whether added piece can move
        [Test]
        public void AddsUnitThatCanMove()
        {
            gameHex.piece = unit;
            Assert.IsTrue(gameHex.PieceCanMove());
        }

        // Test adding building
        [Test]
        public void AddsBuilding()
        {
            gameHex.piece = building;
            Assert.IsTrue(gameHex.HasPiece());
        }
    }
}