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
            gameHex = gameMap.GetWorldPositionHex(Vector3Int.zero);
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
            gameHex = gameMap.GetWorldPositionHex(new Vector3(1, 0.55f));
            player.SetSelectedPiece(gameHex.piece);
            Assert.IsNull(player.selectedPiece);
        }

        // Test deselecting player selected piece on hex click with selected piece
        [Test]
        public void SetsSelectedPieceToNullOnHexWithCurrentlySelectedPieceClick()
        {
            gameHex = gameMap.GetWorldPositionHex(Vector3Int.zero);
            player.SetSelectedPiece(gameHex.piece);
            Assert.IsNull(player.selectedPiece);
        }
    }
}