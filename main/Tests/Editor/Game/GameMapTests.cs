using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

namespace Tests
{
    public class GameMapTests
    {
        // Create game map
        private GameMap CreateGameMap() {
            GameMap gameMap = new GameMap();
            return gameMap;
        }

        // Create game map with players
        private GameMap CreateGameMapWithPlayers() {
            GameMap gameMap = CreateGameMap();

            List<Player> players = new List<Player>();
            for (int i = 0; i < 2; i++) {
                players.Add(new Player(i, gameMap, new Vector3Int(0, 0, 0)));
            }

            gameMap.SetPlayers(players);
            return gameMap;
        }

        // Add piece
        private void AddPiece(GameMap gameMap) {
            Vector3Int tileCoords = new Vector3Int(0, 0, 0);

            // Confirm adds piece
            Unit newPiece = GamePiece.LoadTestUnit(1);
            gameMap.AddPiece(newPiece, tileCoords);
            GameHex gameHex = gameMap.GetHexAtTileCoords(tileCoords);
            Assert.IsNotNull(gameHex.GetPiece());
            Assert.AreEqual(gameHex.GetPiece().GetCard().cardName, "Wizard");
            Assert.AreEqual(gameHex.GetPiece().GetPlayerId(), 1);

            // Confirm doesn't add second piece
            Unit secondPiece = GamePiece.LoadTestUnit(2);
            gameMap.AddPiece(secondPiece, tileCoords);
            Assert.AreNotEqual(gameHex.GetPiece().GetPlayerId(), 2);
            Assert.AreEqual(gameHex.GetPiece().GetPlayerId(), 1);
        }

        // Test create game map
        [Test]
        public void CreatesGameMap() {
            GameMap gameMap = CreateGameMap();
            Assert.IsNotNull(gameMap);
            Assert.AreEqual(GameSetupData.mapRadius, gameMap.GetMapRadius());
        }

        // Test getting piece at hex coords
        [Test]
        public void GetsPieceAtHexCoords() {
            GameMap gameMap = CreateGameMap();
            AddPiece(gameMap);

            // Confirm get piece
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);
            GamePiece piece = gameMap.GetHexPiece(hexCoords);
            Assert.IsNotNull(piece);
        }

        // Test get hex at hex coords
        [Test]
        public void GetsHexAtHexCoords() {
            GameMap gameMap = CreateGameMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);

            // Confirm gets hex that is in map
            Hex hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);
            hexCoords = new Vector3Int(7, -7, 0);
            hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, 2, 3);
            hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(9, -9, 0);
            hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);
        }

        // Test get hex at tile coords
        [Test]
        public void GetsHexAtTileCoords() {
            GameMap gameMap = CreateGameMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);

            // Confirm gets hex that is in map
            Hex hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNotNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, 2, 3);
            hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);

            // Confirm does not get hex that is not in map
            hexCoords = new Vector3Int(1, -2, -2);
            hex = gameMap.GetHexAtHexCoords(hexCoords);
            Assert.IsNull(hex);
        }

        // Test move piece
        [Test]
        public void MovesPiece() {
            GameMap gameMap = CreateGameMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);
            GameHex startingHex = gameMap.GetHexAtHexCoords(hexCoords);
            Unit unit = GamePiece.LoadTestUnit();
            gameMap.AddPiece(unit, hexCoords);

            // Confirm piece is moved
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);
            Vector3Int targetTileCoords = Map.ConvertHexToTileCoords(targetHexCoords);
            GameHex targetHex = gameMap.GetHexAtTileCoords(targetTileCoords);
            gameMap.MovePiece(unit, targetTileCoords);
            Assert.IsNull(startingHex.GetPiece());
            Assert.IsNotNull(targetHex.GetPiece());
        }

        // Test attack piece
        [Test]
        public void AttacksPiece() {
            GameMap gameMap = CreateGameMap();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);

            GameHex startingHex = gameMap.GetHexAtHexCoords(hexCoords);
            Unit unit = GamePiece.LoadTestUnit();
            gameMap.AddPiece(unit, hexCoords);
            gameMap.AddPiece(unit, Map.ConvertHexToTileCoords(targetHexCoords));

            // Confirm piece is attacked
            GameHex targetHex = gameMap.GetHexAtHexCoords(targetHexCoords);
            GamePiece targetPiece = targetHex.GetPiece();
            gameMap.AttackPiece(unit, targetPiece);
            Assert.AreEqual(targetHex.GetPiece().GetCurrentHealth(), 2);
        }

        // Test kills piece
        [Test]
        public void AttacksAndKillsPiece() {
            GameMap gameMap = CreateGameMapWithPlayers();
            Vector3Int hexCoords = new Vector3Int(0, 0, 0);
            Vector3Int targetHexCoords = new Vector3Int(1, -1, 0);

            GameHex startingHex = gameMap.GetHexAtHexCoords(hexCoords);
            Unit unit = GamePiece.LoadTestUnit(0);
            gameMap.AddPiece(unit, hexCoords);
            unit = GamePiece.LoadTestUnit(1);
            gameMap.AddPiece(unit, Map.ConvertHexToTileCoords(targetHexCoords));

            // Confirm piece is attacked
            GameHex targetHex = gameMap.GetHexAtHexCoords(targetHexCoords);
            GamePiece targetPiece = targetHex.GetPiece();
            gameMap.AttackPiece(unit, targetPiece);
            Assert.AreEqual(targetHex.GetPiece().GetCurrentHealth(), 2);

            // Confirm piece dies
            gameMap.AttackPiece(unit, targetPiece);
            Assert.AreEqual(0, targetPiece.GetCurrentHealth());
            Assert.IsNull(targetHex.GetPiece());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator GameMapTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
