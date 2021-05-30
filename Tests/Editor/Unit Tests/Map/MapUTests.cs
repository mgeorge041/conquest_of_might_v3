using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Tests.UTests.MapTests
{
    public class MapUTests
    {
        // Coordinates for neighbor hexes
        private Vector3Int[] neighborCoords =
        {
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, -1),
            new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 1),
            new Vector3Int(-1, 0, 1)
        };

        // Test variables
        private Map map;
        private Vector3Int hexCoords;
        private Vector3Int badHexCoords;
        private Vector3Int badTileCoords;
        private List<Hex> hexesInRange;
        private List<Vector3Int> hexCoordsInRange;

        // Create map for tests
        public static Map CreateTestMap()
        {
            Map map = TestFunctions.CreateClassObject<Map>("Assets/Prefabs/Map/Map.prefab");
            map.Initialize();
            return map;
        }

        // Setup 
        [SetUp]
        public void Setup()
        {
            map = CreateTestMap();
            hexCoords = new Vector3Int(0, 0, 0);
            badHexCoords = new Vector3Int(-7, -7, 0);
            badTileCoords = new Vector3Int(-7, -7, 0);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(map.gameObject);
        }

        // Test map created
        [Test]
        public void MapCreated()
        {
            Assert.IsNotNull(map);
        }

        // Test game map is correct size
        [Test]
        public void GameMapIsCorrectSize()
        {
            Assert.AreEqual(5, map.mapRadius);
            Assert.AreEqual(91, map.hexCoordsDict.Count);
        }

        // Test map updates size
        [Test]
        public void GameMapUpdatesSize()
        {
            map.UpdateMapToRadius(7);
            Assert.AreEqual(7, map.mapRadius);
            Assert.AreEqual(169, map.hexCoordsDict.Count);
        }

        // Test get hexes within 1 range
        [Test]
        public void Gets6HexesIn1Range() {
            // Confirm gets 6 tiles in 1 range
            hexesInRange = map.GetHexesInRange(map.hexCoordsDict, hexCoords, 1);
            Assert.AreEqual(hexesInRange.Count, 6);
        }

        // Test get hex coords within 1 range
        [Test]
        public void GetsCorrectHexCoordsIn1Range()
        {
            hexCoordsInRange = map.GetHexCoordsInRange(hexCoords, 1);
            for (int i = 0; i < neighborCoords.Length; i++)
            {
                Assert.Contains(neighborCoords[i], hexCoordsInRange);
            }
        }

        // Test get hexes within 2 range
        [Test]
        public void Gets18HexesIn2Range()
        {
            hexesInRange = map.GetHexesInRange(map.hexCoordsDict, hexCoords, 2);
            Assert.AreEqual(hexesInRange.Count, 18);
        }

        // Test get hexes within 1 range from non-center tile
        [Test]
        public void Gets6HexesIn1RangeFromNonCenterHex()
        {
            hexCoords = new Vector3Int(-1, 1, 0);
            hexesInRange = map.GetHexesInRange(map.hexCoordsDict, hexCoords, 1);
        }

        // Test get hex coords within 1 range from non-center tile
        [Test]
        public void GetsCorrectHexCoordsIn1RangeFromNonCenterHex()
        {
            hexCoordsInRange = map.GetHexCoordsInRange(hexCoords, 1);
            Assert.AreEqual(hexesInRange.Count, 6);
            for (int i = 0; i < neighborCoords.Length; i++)
            {
                Assert.Contains(neighborCoords[i] + hexCoords, hexCoordsInRange);
            }
        }

        // Test get hex at hex coords in map
        [Test]
        public void GetsHexAtHexCoordsInMap()
        {
            // Confirm gets hex that is not in map
            Hex hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);
        }

        // Test get hex at hex coords not in map
        [Test]
        public void DoesNotGetHexAtHexCoordsNotInMap()
        {
            // Confirm does not get hex that is not in map
            Hex hex = map.GetHexAtHexCoords(badHexCoords);
            Assert.IsNull(hex);
        }

        // Test get hex at tile coords in map
        [Test]
        public void GetsHexAtTileCoords()
        {
            // Confirm gets hex that is in map
            Hex hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);
        }

        // Test get hex at tile coords not in map
        [Test]
        public void DoesNotGetHexAtTileCoordsNotInMap()
        {
            // Confirm does not get hex that is not in map
            Hex hex = map.GetHexAtHexCoords(badTileCoords);
            Assert.IsNull(hex);
        }

        // Test convert cell to world coords and back
        [Test]
        public void ConvertsCellToWorldCoordsAndBack()
        {
            Vector3 worldCoords = map.HexToWorldCoords(hexCoords);
            Vector3Int newHexCoords = map.WorldToHexCoords(worldCoords);
            Assert.AreEqual(hexCoords, newHexCoords);
        }

        // Test convert non-center cell to world coords and back
        [Test]
        public void ConvertsNonCenterCellToWorldCoordsAndBack()
        {
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            Vector3 worldCoords = map.HexToWorldCoords(targetHexCoords);
            Vector3Int newHexCoords = map.WorldToHexCoords(worldCoords);
            Assert.AreEqual(targetHexCoords, newHexCoords);
        }
    }
}
