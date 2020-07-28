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
        private MovementMap CreateMovementMap() {
            MovementMap movementMap = new MovementMap();
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
            MovementMap movementMap = CreateMovementMap();
            Assert.IsNotNull(movementMap);
        }

        // Test set attack and move tiles
        [Test]
        public void MovementMapTilesSet() {
            MovementMap movementMap = CreateMovementMap();
            Assert.IsNotNull(movementMap.attackTile);
            Assert.IsNotNull(movementMap.movementTile);
        }


        // Test create movement map tiles
        [Test]
        public void MovementMapTilesCreated() {
            MovementMap movementMap = CreateMovementMap();
            FogOfWarMap fogMap = new FogOfWarMap();
            GameMap gameMap = new GameMap();
            Unit newPiece = GamePiece.LoadTestUnit();
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);
            gameMap.AddPiece(newPiece, tileCoords);

            // Confirm shows correct number of movement tiles
            movementMap.CreateMovementMap(newPiece, gameMap, fogMap);
            Assert.AreEqual(6, movementMap.GetNumberMovementMapTiles());
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
            MovementMap movementMap = CreateMovementMap();
            FogOfWarMap fogMap = new FogOfWarMap();
            GameMap gameMap = new GameMap();
            Unit newPiece = GamePiece.LoadTestUnit();
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);
            gameMap.AddPiece(newPiece, tileCoords);

            // Confirm shows correct number of movement tiles
            movementMap.CreateMovementMap(newPiece, gameMap, fogMap);
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
