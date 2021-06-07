using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;
using Tests;

namespace Tests.UTests.PlayerTests
{
    public class HandUTests
    {
        private Hand hand;

        // Create hand 
        public static Hand CreateTestHand() {
            Hand hand = TestFunctions.CreateClassObject<Hand>("Assets/Resources/Prefabs/Hand.prefab");
            return hand;
        }

        // Create hand
        public static Hand CreateTestHand(Player player)
        {
            Hand hand = TestFunctions.CreateClassObject<Hand>("Assets/Resources/Prefabs/Hand.prefab");
            return hand;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            hand = CreateTestHand();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            hand = null;
        }

        // Test creates hand
        [Test]
        public void CreatesHand()
        {
            Assert.IsNotNull(hand);
        }

        // Test hand collapses
        [Test]
        public void HandCollapses() {

            // Set initial transform position
            Transform handObject = new GameObject().transform;
            float height = 270;
            Vector3 newPosition = new Vector3(0, height);
            handObject.position = newPosition;
            Assert.AreEqual(handObject.position, newPosition);

            // Confirm collapses to correct location
            Hand.CollapseHand(handObject, height);
            Assert.AreEqual(handObject.position.y, -260);
        }

        // Test hand expands
        public void HandExpands() {

            // Set initial transform position
            Transform handObject = new GameObject().transform;
            float height = 270;
            Vector3 newPosition = new Vector3(0, height);
            handObject.position = newPosition;
            Assert.AreEqual(handObject.position, newPosition);

            // Confirm collapses then expands back to correct location
            Hand.CollapseHand(handObject, height);
            Hand.ExpandHand(handObject, height);
            Assert.AreEqual(handObject.position.y, height);
        }
    }
}
