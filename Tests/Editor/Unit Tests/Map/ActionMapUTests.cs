using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.MapTests
{
    public class ActionMapUTests
    {
        private ActionMap actionMap;

        // Create test action map
        public static ActionMap CreateTestActionMap()
        {
            ActionMap newActionMap = new ActionMap();
            return newActionMap;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            actionMap = CreateTestActionMap();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            actionMap = null;
        }

        // Test creates action map
        [Test]
        public void CreatesActionMap()
        {
            Assert.IsNotNull(actionMap);
        }
    }
}