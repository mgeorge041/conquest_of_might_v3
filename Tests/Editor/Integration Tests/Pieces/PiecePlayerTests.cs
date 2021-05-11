using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.PieceTests;

namespace Tests.IntegrationTests.PieceTests
{
    public class PiecePlayerTests
    {
        private Unit unit1;
        private Unit unit2;

        // Create test unit with player
        public static Unit CreateTestUnitWithPlayer()
        {
            Unit unit = UnitUTests.CreateTestUnit();
            Player player = PlayerUTests.CreateTestPlayer();
            unit.player = player;
            return unit;
        }

        // Create test unit for player
        public static Unit CreateTestUnitForPlayer(int playerId)
        {
            Unit unit = UnitUTests.CreateTestUnit();
            Player player = PlayerUTests.CreateTestPlayer(playerId);
            unit.player = player;
            return unit;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            unit1 = CreateTestUnitWithPlayer();
            unit2 = CreateTestUnitWithPlayer();
        }

        // Test creates unit
        [Test]
        public void CreatesUnit()
        {
            Unit unit = CreateTestUnitWithPlayer();
            Assert.IsNotNull(unit);
            Assert.IsNotNull(unit.player);
            Assert.AreEqual(1, unit.GetPlayerId());
        }

        // Test creates unit for player
        [Test]
        public void CreatesUnitForPlayer()
        {
            Unit unit = CreateTestUnitForPlayer(2);
            Assert.IsNotNull(unit);
            Assert.IsNotNull(unit.player);
            Assert.AreEqual(2, unit.GetPlayerId());
        }

    }
}