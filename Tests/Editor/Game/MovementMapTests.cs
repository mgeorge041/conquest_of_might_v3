using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MovementMapTests
    {
        // Create movement map
        private ActionMap CreateMovementMap() {
            ActionMap movementMap = new ActionMap();
            return movementMap;
        }

        private Vector3Int[] neighborCoords = {
            new Vector3Int(-1, 1, 0),
            new Vector3Int(0, 1, -1),
            new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 1),
            new Vector3Int(-1, 0, 1)
        };

        // Test create movement map
        [Test]
        public void MovementMapCreated()
        {
            ActionMap movementMap = CreateMovementMap();
            Assert.IsNotNull(movementMap);
        }

        // Test set attack and move tiles
        [Test]
        public void MovementMapTilesSet() {
            ActionMap movementMap = CreateMovementMap();
            Assert.IsNotNull(movementMap.attackTile);
            Assert.IsNotNull(movementMap.movementTile);
        }


        // Test create movement map tiles
        [Test]
        public void MovementMapTilesCreated() {
            ActionMap movementMap = CreateMovementMap();
            FogMap fogMap = new FogMap();
            GameMap gameMap = new GameMap();

            // Fake set piece on game map
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);
            Unit newPiece = GamePieceTests.CreateTestUnit();
            newPiece.SetGameHex(gameMap.GetHexAtTileCoords(tileCoords));

            // Confirm shows correct number of movement tiles
            movementMap.CreateActionMap(newPiece, gameMap, fogMap);
            Assert.AreEqual(18, movementMap.GetNumberMovementMapTiles());
            Assert.AreEqual("Movement Tile", movementMap.GetPaintedTileAtTileCoords(new Vector3Int(1, 0, 0)).name);
            Assert.IsNull(movementMap.GetPaintedTileAtTileCoords(new Vector3Int(10, 10, 0)));

            // Confirm each of the 6 adjacent tiles are painted
            for (int i = 0; i < neighborCoords.Length; i++) {
                Assert.IsTrue(movementMap.MoveableToTile(Map.ConvertHexToTileCoords(neighborCoords[i])));
            }
        }

        // Test whether tile is moveable
        [Test]
        public void MovementMapTileIsMoveable() {
            ActionMap movementMap = CreateMovementMap();
            FogMap fogMap = new FogMap();
            GameMap gameMap = new GameMap();

            // Fake set piece on game map
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);
            Unit newPiece = GamePieceTests.CreateTestUnit();
            newPiece.SetGameHex(gameMap.GetHexAtTileCoords(tileCoords));

            // Confirm shows correct number of movement tiles
            movementMap.CreateActionMap(newPiece, gameMap, fogMap);
            Assert.IsTrue(movementMap.MoveableToTile(new Vector3Int(1, 0, 0)));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MovementMapTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
