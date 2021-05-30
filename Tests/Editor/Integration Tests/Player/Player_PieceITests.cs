using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.ITests.PieceTests;
using Tests.UTests.PlayerTests;

namespace Tests.ITests.PlayerTests
{
    public class PlayerPieceITests
    {
        private Unit unit1;
        private Player player1;

        // Setup
        [SetUp]
        public void Setup()
        {
            unit1 = UnitCardUnitITests.CreateTestUnitWithCard();
            player1 = PlayerUTests.CreateTestPlayer(1);
            unit1.player = player1;
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(unit1.gameObject);
        }
        // Test set selected piece as unit
        [Test]
        public void SetSelectedPieceAsUnit()
        {
            player1.SetSelectedPiece(unit1);
            Assert.AreEqual(unit1, player1.selectedPiece);
        }

        // Test set selected piece as building
        [Test]
        public void SetSelectedPieceAsBuilding()
        {
            Building building1 = BuildingCardBuildingITests.CreateTestBuildingWithCard();
            player1.SetSelectedPiece(building1);
            Assert.AreEqual(building1, player1.selectedPiece);
        }

        // Test set selected piece as null
        [Test]
        public void SetSelectedPieceAsNull()
        {
            player1.SetSelectedPiece(null);
            Assert.IsNull(player1.selectedPiece);
        }

        // Test set selected piece as null when setting same piece twice
        [Test]
        public void SetSelectedPieceNullWhenSetTwiceAsSamePiece()
        {
            player1.SetSelectedPiece(unit1);
            player1.SetSelectedPiece(unit1);
            Assert.IsNull(player1.selectedPiece);
        }
    }
}