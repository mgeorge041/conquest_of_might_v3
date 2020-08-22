using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GamePieceTests
    {
        // Create game piece
        private Unit CreateUnit() {
            Unit newUnit = GamePiece.LoadTestUnit();
            return newUnit;
        }

        // Test creates game piece
        [Test]
        public void CreatesTestGamePiece()
        {
            GamePiece newPiece = CreateUnit();
            Assert.IsNotNull(newPiece);
            Assert.AreEqual(newPiece.GetCard().cardName, Card.LoadTestUnitCard().cardName);
        }

        // Test creates new unit
        [Test]
        public void CreatesTestUnit() {
            Unit newUnit = CreateUnit();
            Assert.IsNotNull(newUnit);
            Assert.AreEqual(newUnit.GetRemainingSpeed(), Card.LoadTestUnitCard().speed);
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
