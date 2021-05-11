using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.MapTests
{
    public class HexUTests
    {
        private Hex hex;
        private int distance;
        private Vector3Int hexACoords;
        private Vector3Int hexBCoords;

        // Create new test hex
        public static Hex CreateTestHex()
        {
            Hex newHex = new Hex(TileUTests.CreateTestTileData(), Vector3Int.zero);
            return newHex;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            hex = CreateTestHex();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            hex = null;
        }

        // Test creates hex
        [Test]
        public void CreatesHex()
        {
            Assert.IsNotNull(hex);
        }

        // Test sets initial hex coords
        [Test]
        public void SetsEmptyHexCoordsToZero()
        {
            Assert.AreEqual(Vector3Int.zero, hex.hexCoords);
            Assert.AreEqual(Vector3Int.zero, hex.tileCoords);
        }

        // Test creates hex static function
        [Test]
        public void CreatesHexStaticFunction()
        {
            // Create normal hex
            Hex hex = Hex.CreateHex<Hex>(TileUTests.CreateTestTileData(), Vector3Int.zero);
            Assert.IsNotNull(hex);
            Assert.AreEqual(Vector3Int.zero, hex.hexCoords);
            Assert.AreEqual(Vector3Int.zero, hex.tileCoords);

            // Create game hex
            GameHex gameHex = (GameHex)Hex.CreateHex<GameHex>(TileUTests.CreateTestTileData(), Vector3Int.zero);
            Assert.IsNotNull(gameHex);
            Assert.AreEqual(Vector3Int.zero, gameHex.hexCoords);
            Assert.AreEqual(Vector3Int.zero, gameHex.tileCoords);
        }

        // Test getting tile data move cost
        [Test]
        public void GetsTileMoveCost()
        {
            Assert.AreEqual(1, hex.GetMoveCost());
        }

        // Test getting distance between hexes
        [Test]
        public void CorrectDistanceBetweenAdjacentHexesAs1()
        {
            hexACoords = new Vector3Int(1, -1, 0);
            hexBCoords = new Vector3Int(2, -2, 0);
            distance = Hex.GetDistanceHexCoords(hexACoords, hexBCoords);
            Assert.AreEqual(1, distance);
        }

        // Test getting distance between non-adjacent hexes
        [Test]
        public void CorrectDistanceBetweenNonAdjacentHexesAs2()
        {
            hexACoords = new Vector3Int(3, -2, -1);
            hexBCoords = new Vector3Int(1, -1, 0);
            distance = Hex.GetDistanceHexCoords(hexACoords, hexBCoords);
            Assert.AreEqual(2, distance);
        }

        // Test getting distance to center hex
        [Test]
        public void CorrectDistanceCenterHex()
        {
            hexACoords = new Vector3Int(1, -1, 0);
            distance = Hex.GetDistanceToCenterHex(hexACoords);
            Assert.AreEqual(distance, 1);

            hexACoords = new Vector3Int(3, -2, -1);
            distance = Hex.GetDistanceToCenterHex(hexACoords);
            Assert.AreEqual(distance, 3);
        }

        // Test converting hex to tile coords
        [Test]
        public void CorrectHexToTileCoords()
        {
            hexACoords = new Vector3Int(1, -1, 0);
            Vector3Int tileCoords = Hex.HexToTileCoords(hexACoords);
            Assert.AreEqual(tileCoords, new Vector3Int(0, 1, 0));
        }

        // Test converting tile to hex coords
        [Test]
        public void CorrectTileToHexCoords()
        {
            Vector3Int tileCoords = new Vector3Int(1, -1, 0);
            hexACoords = Hex.TileToHexCoords(tileCoords);
            Assert.AreEqual(hexACoords, new Vector3Int(-1, -1, 2));
        }
    }
}