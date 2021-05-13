using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.ITests.PieceTests;
using Tests.UTests.PlayerTests;

namespace Tests.ITests.MapTests
{
    public class GameMapActionMapITests
    {
        private GameMap gameMap;
        private ActionMap actionMap;
        private Unit unit1;
        private Unit unit2;
        private Player player1;
        private Player player2;
        private Vector3Int hexCoords;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            actionMap = ActionMapUTests.CreateTestActionMap();
            unit1 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            player1 = PlayerUTests.CreateTestPlayer(1);
            player2 = PlayerUTests.CreateTestPlayer(2);
            player1.gameMap = gameMap;
            player2.gameMap = gameMap;
            unit1.player = player1;
            unit2.player = player2;
            hexCoords = Vector3Int.zero;
            gameMap.AddPiece(unit1, hexCoords);

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

        // Test set selected piece as unit
        [Test]
        public void SetSelectedPieceAsUnit()
        {
            Building building1 = BuildingCardBuildingITests.CreateTestBuildingWithCard();
            player1.SetSelectedPiece(unit1);
            Assert.AreEqual(unit1, player1.selectedPiece);
        }

        // Test set selected piece as building
        [Test]
        public void SetSelectedPieceAsBuilding()
        {
            Building building1 = BuildingCardBuildingITests.CreateTestBuildingWithCard();
            gameMap.AddPiece(building1, new Vector3Int(2, -2, 0));
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

        // Test move piece
        [Test]
        public void MovesPiece1HexAway()
        {
            // Add piece
            GameHex startingHex = gameMap.GetHexAtHexCoords(hexCoords);
            gameMap.AddPiece(unit1, hexCoords);

            // Move piece
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            GameHex targetHex = gameMap.GetHexAtHexCoords(targetHexCoords);
            gameMap.MovePiece(unit1, targetHexCoords);

            // Confirm piece is moved
            Assert.IsNull(startingHex.piece);
            Assert.AreEqual(unit1, targetHex.piece);
            Assert.AreEqual(4, unit1.remainingSpeed);
        }

        // Test move piece 2 hexes away
        [Test]
        public void MovesPiece2HexesAway()
        {
            // Add piece
            GameHex startingHex = gameMap.GetHexAtHexCoords(hexCoords);
            gameMap.AddPiece(unit1, hexCoords);

            // Move piece
            Vector3Int targetHexCoords = new Vector3Int(2, -2, 0);
            GameHex targetHex = gameMap.GetHexAtHexCoords(targetHexCoords);
            gameMap.MovePiece(unit1, targetHexCoords);

            // Confirm piece is moved
            Assert.IsNull(startingHex.piece);
            Assert.AreEqual(unit1, targetHex.piece);
            Assert.AreEqual(3, unit1.remainingSpeed);
        }

        // Test kills piece
        [Test]
        public void AttacksAndKillsPiece()
        {
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.AddPiece(unit1, hexCoords);
            gameMap.AddPiece(unit2, targetHexCoords);

            // Confirm piece is attacked and dies
            GameHex targetHex = gameMap.GetHexAtHexCoords(targetHexCoords);
            gameMap.AttackPiece(unit1, unit2);
            Assert.AreEqual(0, unit2.currentHealth);
            Assert.IsNull(targetHex.piece);
            Assert.IsTrue(unit2 == null);
        }

        // Test creates movement tiles in 1 range
        [Test]
        public void CreatesMovementTilesFor1Speed()
        {
            unit1.DecreaseSpeed(unit1.remainingSpeed - 1);

            actionMap.CreateActionMap(unit1, gameMap);
            Assert.AreEqual(6, actionMap.movementTiles.Count);
        }

        // Test creates movement tiles in 1 range with a piece on 1 hex
        [Test]
        public void Creates5MovementTilesFor1SpeedWithHexThatHasUnit()
        {
            gameMap.AddPiece(unit2, new Vector3Int(1, -1, 0));
            unit1.DecreaseSpeed(unit1.remainingSpeed - 1);
            actionMap.CreateActionMap(unit1, gameMap);
            Assert.AreEqual(5, actionMap.movementTiles.Count);
        }

        // Test creates 1 attack tile for unit in range
        [Test]
        public void Creates1AttackTileForHexThatHasEnemyUnit()
        {
            gameMap.AddPiece(unit2, new Vector3Int(1, -1, 0));
            actionMap.CreateActionMap(unit1, gameMap);
            Assert.AreEqual(1, actionMap.attackTiles.Count);
        }

        // Test creates 1 attack tile and 6 movement tile for unit in 1 range
        [Test]
        public void Creates1AttackTileAnd6MovementTilesForUnitWith1SpeedAndEnemyUnitIn2Range()
        {
            gameMap.AddPiece(unit2, new Vector3Int(2, -2, 0));
            unit1.DecreaseSpeed(unit1.remainingSpeed - 1);
            unit1.range = 1;
            actionMap.CreateActionMap(unit1, gameMap);
            Assert.AreEqual(1, actionMap.attackTiles.Count);
            Assert.AreEqual(6, actionMap.movementTiles.Count);
        }
    }
}