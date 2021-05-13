using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.ITests.PieceTests;

namespace Tests.ITests.MapTests
{
    public class GameMapPieceITests
    {
        private GameMap gameMap;
        private Unit unit1;
        private Unit unit2;
        private Vector3Int hexCoords;
        private Vector3Int targetHexCoords;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            unit1 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            hexCoords = new Vector3Int(0, 0, 0);
            targetHexCoords = new Vector3Int(1, -1, 0);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(gameMap.gameObject);
            C.Destroy(unit1.gameObject);
            if (unit2 != null)
                C.Destroy(unit2.gameObject);
        }

        // Test adding piece
        [Test]
        public void AddsPiece()
        {
            bool addedPiece = gameMap.AddPiece(unit1, hexCoords);
            GameHex gameHex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.AreEqual(unit1, gameHex.piece);
            Assert.IsTrue(addedPiece);
        }

        // Test adding 2nd piece
        [Test]
        public void AddsOnlyOnePieceToTile()
        {
            // Add piece
            gameMap.AddPiece(unit1, hexCoords);
            GameHex gameHex = gameMap.GetHexAtHexCoords(hexCoords);

            // Confirm doesn't add second piece
            gameMap.AddPiece(unit2, hexCoords);
            Assert.AreNotEqual(unit2, gameHex.piece);
            Assert.AreEqual(unit1, gameHex.piece);
        }

        // Test getting piece at hex coords
        [Test]
        public void GetsPieceAtHexCoords()
        {
            gameMap.AddPiece(unit1, hexCoords);
            GamePiece gotPiece = gameMap.GetHexPiece(hexCoords);
            Assert.AreEqual(unit1, gotPiece);
        }
    }
}
