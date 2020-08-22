using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GamePieceTests
    {
        // Load test unit
        public static Unit LoadTestUnit() {
            CardUnit cardUnit = (CardTests.LoadTestUnitCard());
            Unit newUnit = new Unit(cardUnit);
            newUnit.SetCard(CardTests.LoadTestUnitCard());
            newUnit.SetInfo();
            newUnit.ResetPiece();
            return newUnit;
        }

        // Create test unit
        public static Unit CreateTestUnit() {
            CardUnit cardUnit = CardTests.LoadTestUnitCard();
            Unit unit = new Unit(cardUnit);
            return unit;
        }

        // Create test unit with player
        public static Unit CreateTestUnitWithPlayer() {
            Unit unit = CreateTestUnit();
            Player player = PlayerTests.CreateTestPlayer();
            unit.SetPlayer(player);
            return unit;
        }

        // Create test unit for player
        public static Unit CreateTestUnitForPlayer(Player player) {
            Unit unit = CreateTestUnit();
            unit.SetPlayer(player);
            return unit;
        }

        // Test creates game piece
        [Test]
        public void CreatesTestGamePiece()
        {
            GamePiece newPiece = CreateTestUnit();
            Assert.IsNotNull(newPiece);
            Assert.AreEqual(newPiece.GetCard().cardName, CardTests.LoadTestUnitCard().cardName);
        }

        // Test creates new unit
        [Test]
        public void CreatesTestUnit() {
            Unit newUnit = CreateTestUnit();
            Assert.IsNotNull(newUnit);
            Assert.AreEqual(newUnit.GetRemainingSpeed(), CardTests.LoadTestUnitCard().speed);
        }

        // 

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
