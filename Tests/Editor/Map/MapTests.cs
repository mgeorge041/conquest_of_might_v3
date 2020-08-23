using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace Tests
{
    public class MapTests
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

        // Create map for tests
        private Map CreateMap() {
            Map map = new Map();
            map.CreateMap();
            return map;
        }

        // Test map created
        [Test]
        public void MapCreated()
        {
            Map map = CreateMap();
            Assert.IsNotNull(map);
        }

        // Test getting distance between hexes
        [Test]
        public void CorrectDistanceBetweenHexes() {
            Map map = CreateMap();
            Vector3Int hexA = new Vector3Int(1, -1, 0);
            Vector3Int hexB = new Vector3Int(2, -2, 0);
            int distance = map.GetDistanceHexCoords(hexA, hexB);
            Assert.AreEqual(distance, 1);

            hexA = new Vector3Int(3, -2, -1);
            hexB = new Vector3Int(1, -1, 0);
            distance = map.GetDistanceHexCoords(hexA, hexB);
            Assert.AreEqual(distance, 2);
        }

        // Test getting distance to center hex
        [Test]
        public void CorrectDistanceCenterHex() {
            Map map = CreateMap();
            Vector3Int hexA = new Vector3Int(1, -1, 0);
            int distance = map.GetDistanceToCenterHex(hexA);
            Assert.AreEqual(distance, 1);

            hexA = new Vector3Int(3, -2, -1);
            distance = map.GetDistanceToCenterHex(hexA);
            Assert.AreEqual(distance, 3);
        }

        // Test converting hex to tile coords
        [Test]
        public void CorrectHexToTileCoords() {
            Map map = CreateMap();
            Vector3Int hexCoords = new Vector3Int(1, -1, 0);
            Vector3Int tileCoords = map.HexToTileCoords(hexCoords);
            Assert.AreEqual(tileCoords, new Vector3Int(0, 1, 0));
        }

        // Test converting tile to hex coords
        [Test]
        public void CorrectTileToHexCoords() {
            Map map = CreateMap();
            Vector3Int tileCoords = new Vector3Int(1, -1, 0);
            Vector3Int hexCoords = map.TileToHexCoords(tileCoords);
            Assert.AreEqual(hexCoords, new Vector3Int(-1, -1, 2));
        }

        // Test map is drawn correctly
        [Test]
        public void CorrectMapSize() {
            Map map = CreateMap();
            Assert.AreEqual(map.GetMapRadius(), GameSetupData.mapRadius);
            Assert.AreEqual(map.GetHexCoordsDict().Count, 217);
        }

        // Test map updates size
        [Test]
        public void MapUpdatesSize() {
            Map map = CreateMap();
            map.UpdateMapToRadius(7);
            Assert.AreEqual(7, map.GetMapRadius());
            Assert.AreEqual(169, map.GetHexCoordsDict().Count);
        }

        // Test get hexes within range
        [Test]
        public void GetsHexesInRange() {
            Map map = CreateMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);

            // Confirm gets 6 tiles in 1 range
            List<Hex> hexesInRange = map.GetHexesInRange(hexCoords, 1);
            List<Vector3Int> hexCoordsInRange = map.GetHexCoordsInRange(hexCoords, 1);
            Assert.AreEqual(hexesInRange.Count, 6);
            for (int i = 0; i < neighborCoords.Length; i++) {
                Assert.Contains(neighborCoords[i], hexCoordsInRange);
            }

            // Confirm gets 18 tiles in 2 range
            hexesInRange = map.GetHexesInRange(hexCoords, 2);
            Assert.AreEqual(hexesInRange.Count, 18);

            // Confirm gets 6 tiles in 1 range (not center tile)
            hexCoords = new Vector3Int(-1, 1, 0);
            hexesInRange = map.GetHexesInRange(hexCoords, 1);
            hexCoordsInRange = map.GetHexCoordsInRange(hexCoords, 1);
            Assert.AreEqual(hexesInRange.Count, 6);
            for (int i = 0; i < neighborCoords.Length; i++) {
                Assert.Contains(neighborCoords[i] + hexCoords, hexCoordsInRange);
            }
        }

        // Test get hex at hex coords
        [Test]
        public void GetsHexAtHexCoords() {
            Map map = CreateMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);

            // Confirm gets hex that is in map
            Hex hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, 2, 3);
            hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(9, -9, 0);
            hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);
        }

        // Test get hex at tile coords
        [Test]
        public void GetsHexAtTileCoords() {
            Map map = CreateMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);

            // Confirm gets hex that is in map
            Hex hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, 2, 3);
            hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, -2, -2);
            hex = map.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MapTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
