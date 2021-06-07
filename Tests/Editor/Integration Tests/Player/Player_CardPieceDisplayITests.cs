using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.ITests.CardTests;

namespace Tests.ITests.PlayerTests
{
    public class Player_CardPieceDisplayITests
    {
        private Player player;
        private CardUnitDisplay cardUnitDisplay;
        private CardBuildingDisplay cardBuildingDisplay;
        private CardResourceDisplay cardResourceDisplay;

        // Setup
        [SetUp]
        public void Setup()
        {
            player = PlayerUTests.CreateTestPlayer();
            cardUnitDisplay = CardUnit_CardUnitDisplayITests.CreateTestCardUnitDisplay();
            cardBuildingDisplay = CardBuilding_CardBuildingDisplayITests.CreateTestCardBuildingDisplay();
            cardResourceDisplay = CardResource_CardResourceDisplayITests.CreateTestCardResourceDisplay();
        }

        // End 
        [TearDown]
        public void Teardown()
        {

        }


    }
}