using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.ITests.PieceTests;
using Tests.UTests.PlayerTests;
using Tests.ITests.MapTests;

namespace Tests.FTests.GameMapTests
{
    public class GameMapFTests
    {
        private GameMap gameMap;
        private Player player;
        private Unit unit;
        private Vector3Int hexCoords;
        private GameHex gameHex;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            player = PlayerUTests.CreateTestPlayer();
            player.gameMap = gameMap;
            unit = UnitCardUnitITests.CreateTestUnitWithCard();
            unit.player = player;
            hexCoords = new Vector3Int(0, 0, 0);
            gameMap.AddPiece(unit, hexCoords);

            // Get center hex and set selected piece
            gameHex = gameMap.GetWorldPositionHex<GameHex>(Vector3Int.zero);
            player.SetSelectedPiece(gameHex.piece);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(gameMap.gameObject);
            player = null;
            C.Destroy(unit.gameObject);
        }

        // Test setting player selected piece on hex click with piece
        [Test]
        public void SetsSelectedPieceToPieceOnHexWithPieceClick()
        {
            Assert.AreEqual(unit, player.selectedPiece);
        }

        // Test setting player selected piece on hex click without piece
        [Test]
        public void SetsSelectedPieceToNullOnHexWithoutPieceClick()
        {
            gameHex = gameMap.GetWorldPositionHex<GameHex>(new Vector3(1, 0.55f));
            player.SetSelectedPiece(gameHex.piece);
            Assert.IsNull(player.selectedPiece);
        }

        // Test deselecting player selected piece on hex click with selected piece
        [Test]
        public void SetsSelectedPieceToNullOnHexWithCurrentlySelectedPieceClick()
        {
            gameHex = gameMap.GetWorldPositionHex<GameHex>(Vector3Int.zero);
            player.SetSelectedPiece(gameHex.piece);
            Assert.IsNull(player.selectedPiece);
        }

        // Test moves piece within speed range
        [Test]
        public void MovesPieceWithinSpeedRangeToEmptyHex()
        {
            Vector3Int targetHexCoords = gameMap.GetWorldPositionHexCoords(new Vector3(1, 0.55f));
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameHex.piece);
            Assert.AreEqual(unit, gameMap.GetHexAtHexCoords(targetHexCoords).piece);
        }

        // Test does not move piece outside of speed range
        [Test]
        public void DoesNotMovePieceOutsideSpeedRangeToEmptyHex()
        {
            Vector3Int targetHexCoords = gameMap.GetWorldPositionHexCoords(new Vector3(3, 1.65f));
            unit.DecreaseSpeed(3);
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameMap.GetHexAtHexCoords(targetHexCoords).piece);
            Assert.AreEqual(unit, gameHex.piece);
        }

        // Test does not move to a hex with a piece
        [Test]
        public void DoesNotMovePieceToHexWithPiece()
        {
            Unit unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.GetWorldPositionHexCoords(new Vector3(1, 0.55f));
            gameMap.AddPiece(unit2, targetHexCoords);
            gameMap.MovePiece(player.GetSelectedUnit(), targetHexCoords);
            Assert.AreEqual(unit, gameHex.piece);
            Assert.AreEqual(unit2, gameMap.GetHexAtHexCoords(targetHexCoords).piece);
        }

        // Test attacks piece within range
        [Test]
        public void AttacksPieceWithinRange()
        {
            Unit unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.GetWorldPositionHexCoords(new Vector3(1, 0.55f));
            gameMap.AddPiece(unit2, targetHexCoords);
            unit.might = 3;
            gameMap.AttackPiece(player.GetSelectedPiece(), unit2);
            Assert.AreEqual(2, unit2.currentHealth);
        }

        // Test does not attack piece outside of range
        [Test]
        public void DoesNotAttackPieceOutsideRange()
        {
            Unit unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            Player player2 = PlayerUTests.CreateTestPlayer(2);
            unit2.player = player2;
            Vector3Int targetHexCoords = gameMap.GetWorldPositionHexCoords(new Vector3(3, 1.65f));
            gameMap.AddPiece(unit2, targetHexCoords);
            unit.might = 3;
            unit.range = 1;
            gameMap.AttackPiece(player.GetSelectedPiece(), unit2);
            Assert.AreEqual(5, unit2.currentHealth);
        }
    }
}