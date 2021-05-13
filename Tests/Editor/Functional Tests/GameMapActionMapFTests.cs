using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;
using Tests.ITests.PieceTests;
using Tests.UTests.PlayerTests;

namespace Tests.FTests.GameMapTests
{
    public class GameMapActionMapFTests
    {
        private GameMap gameMap;
        private ActionMap actionMap;
        private Player player1;
        private Player player2;
        private Unit unit1;
        private Unit unit2;
        private Vector3Int hexCoords;
        private GameHex gameHex;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            player1 = PlayerUTests.CreateTestPlayer(1);
            player2 = PlayerUTests.CreateTestPlayer(2);
            player1.gameMap = gameMap;
            player2.gameMap = gameMap;
            actionMap = player1.actionMap;
            unit1 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit2 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit1.player = player1;
            unit2.player = player2;
            hexCoords = new Vector3Int(0, 0, 0);
            gameMap.AddPiece(unit1, hexCoords);

            // Get center hex
            gameHex = gameMap.GetWorldPositionHex<GameHex>(Vector3Int.zero);
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(gameMap.gameObject);
            C.Destroy(unit1.gameObject);
            C.Destroy(unit2.gameObject);
            player1 = null;
            player2 = null;
        }

        // Test create action map on click piece
        [Test]
        public void CreatesActionMapOnPieceClick()
        {
            unit1.remainingSpeed = 2;
            player1.SetSelectedPiece(gameHex.piece);
            Assert.AreEqual(18, actionMap.movementTiles.Count);
        }

        // Test move piece to moveable tile
        [Test]
        public void MovesPieceToMoveableTile()
        {
            unit1.remainingSpeed = 1;
            player1.SetSelectedPiece(gameHex.piece);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.MovePiece(player1.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameMap.GetHexAtHexCoords(hexCoords).piece);
            Assert.AreEqual(unit1, gameMap.GetHexAtHexCoords(targetHexCoords).piece);
        }

        // Test does not move piece to not moveable tile
        [Test]
        public void DoesNotMovePieceToNonMoveableTile()
        {
            unit1.remainingSpeed = 1;
            player1.SetSelectedPiece(gameHex.piece);
            Vector3Int targetHexCoords = new Vector3Int(2, -2, 0);
            gameMap.MovePiece(player1.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(gameMap.GetHexAtHexCoords(targetHexCoords).piece);
            Assert.AreEqual(player1.GetSelectedUnit(), gameMap.GetHexAtHexCoords(hexCoords).piece);
        }

        // Test move piece to moveable tile and deset selected player piece
        [Test]
        public void MovesPieceToMoveableTileAndDeselectsPiece()
        {
            unit1.remainingSpeed = 1;
            player1.SetSelectedPiece(gameHex.piece);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.MovePiece(player1.GetSelectedUnit(), targetHexCoords);
            Assert.IsNull(player1.selectedPiece);
            Assert.AreEqual(0, actionMap.movementTiles.Count);
            Assert.AreEqual(0, actionMap.attackTiles.Count);
        }

        // Test attack piece 
        [Test]
        public void AttacksPiece()
        {
            player1.SetSelectedPiece(gameHex.piece);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.AddPiece(unit2, targetHexCoords);
            unit1.might = 3;
            gameMap.AttackPiece(player1.selectedPiece, unit2);
            Assert.AreEqual(2, unit2.currentHealth);
        }

        // Test does not attack piece of same player
        [Test]
        public void DoesNotAttackPieceOfSamePlayer()
        {
            player1.SetSelectedPiece(gameHex.piece);
            Unit unit3 = UnitCardUnitITests.CreateTestUnitWithCard();
            unit3.player = player1;
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.AddPiece(unit3, targetHexCoords);
            gameMap.AttackPiece(unit1, unit3);
            Assert.AreEqual(5, unit3.currentHealth);
        }

        // Test attack piece and deset selected player piece
        [Test]
        public void AttacksPieceAndDeselectsPiece()
        {
            player1.SetSelectedPiece(gameHex.piece);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            gameMap.AddPiece(unit2, targetHexCoords);
            unit1.might = 3;
            gameMap.AttackPiece(player1.selectedPiece, unit2);
            Assert.IsNull(player1.selectedPiece);
            Assert.AreEqual(0, actionMap.movementTiles.Count);
            Assert.AreEqual(0, actionMap.attackTiles.Count);
        }
    }
}