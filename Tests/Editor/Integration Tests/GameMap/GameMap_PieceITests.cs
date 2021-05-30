using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.UTests.PlayerTests;
using Tests.ITests.PieceTests;

namespace Tests.ITests.MapTests
{
    public class GameMapPieceITests
    {
        private GameMap gameMap;
        private Unit unit1;
        private Unit unit2;
        private Player player;
        private Vector3Int hexCoords;
        private Vector3Int targetHexCoords;
        private GameHex gameHex;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            player = PlayerUTests.CreateTestPlayer();
            unit1 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit1.player = player;
            hexCoords = new Vector3Int(0, 0, 0);
            targetHexCoords = new Vector3Int(1, -1, 0);

            // Get center hex and set selected piece
            gameMap.AddPiece(unit1, hexCoords);
            gameHex = gameMap.GetWorldPositionHex(Vector3Int.zero);
            player.SetSelectedPiece(unit1);
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
            Assert.AreEqual(unit1, gameHex.piece);
        }

        // Test adding piece and setting transform
        [Test]
        public void AddsPieceToCenterTileAndSetsTransform()
        {
            Assert.AreEqual(unit1.transform.position, gameMap.tileGrid.CellToWorld(gameHex.tileCoords));
        }

        // Test adding piece to non-center tile and setting transform
        [Test]
        public void AddsPieceToNonCenterTileAndSetsTransform()
        {
            gameMap.AddPiece(unit2, targetHexCoords);
            Assert.AreEqual(unit2.transform.position, gameMap.tileGrid.CellToWorld(Hex.HexToTileCoords(targetHexCoords)));
        }

        // Test adding 2nd piece
        [Test]
        public void AddsOnlyOnePieceToTile()
        {
            gameMap.AddPiece(unit2, hexCoords);
            Assert.AreNotEqual(unit2, gameHex.piece);
            Assert.AreEqual(unit1, gameHex.piece);
        }

        // Test getting piece at hex coords
        [Test]
        public void GetsPieceAtHexCoords()
        {
            GamePiece gotPiece = gameMap.GetHexPieceFromHexCoords(hexCoords);
            Assert.AreEqual(unit1, gotPiece);
        }

        // Test moves piece within speed range
        [Test]
        public void MovesPieceWithinSpeedRangeToEmptyHex()
        {
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(1, 0.55f));
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameHex.piece);
            Assert.AreEqual(unit1, gameMap.GetHexAtHexCoords(targetHexCoords).piece);
        }

        // Test moves piece within speed range and updates transform
        [Test]
        public void MovesPieceWithinSpeedRangeAndUpdatesTransform()
        {
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(1, 0.55f));
            Vector3 startPosition = unit1.transform.position;
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Vector3 endPosition = unit1.transform.position;

            // Confirm position is set correctly
            Assert.AreNotEqual(startPosition, endPosition);
            Assert.AreEqual(gameMap.HexToWorldCoords(targetHexCoords), endPosition);
        }

        // Test does not move piece outside of speed range
        [Test]
        public void DoesNotMovePieceOutsideSpeedRangeToEmptyHex()
        {
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(3, 1.65f));
            unit1.DecreaseSpeed(3);
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameMap.GetHexAtHexCoords(targetHexCoords).piece);
            Assert.AreEqual(unit1, gameHex.piece);
        }

        // Test does not move to a hex with a piece
        [Test]
        public void DoesNotMovePieceToHexWithPiece()
        {
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(1, 0.55f));
            gameMap.AddPiece(unit2, targetHexCoords);
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.AreEqual(unit1, gameHex.piece);
            Assert.AreEqual(unit2, gameMap.GetHexAtHexCoords(targetHexCoords).piece);
        }

        // Test attacks piece within range
        [Test]
        public void AttacksPieceWithinRange()
        {
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(1, 0.55f));
            gameMap.AddPiece(unit2, targetHexCoords);
            unit1.might = 3;
            gameMap.AttackPiece(player.GetSelectedPiece(), unit2);
            Assert.AreEqual(2, unit2.currentHealth);
        }

        // Test does not attack piece outside of range
        [Test]
        public void DoesNotAttackPieceOutsideRange()
        {
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.WorldToHexCoords(new Vector3(3, 1.65f));
            gameMap.AddPiece(unit2, targetHexCoords);
            unit1.might = 3;
            unit1.range = 1;
            gameMap.AttackPiece(player.GetSelectedPiece(), unit2);
            Assert.AreEqual(5, unit2.currentHealth);
        }
    }
}
