using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.PlayerTests;
using Tests.UTests.CardTests;

namespace Tests.ITests.PlayerTests
{
    public class PlayerGameDataCardITests
    {
        private PlayerGameData playerGameData;
        private CardUnit cardUnit;
        private CardResource cardResource;
        private CardBuilding cardBuilding;

        // Setup
        [SetUp]
        public void Setup()
        {
            playerGameData = PlayerGameDataUTests.CreateTestPlayerGameData();
            cardUnit = CardUnitUTests.CreateTestCardUnit();
            cardResource = CardResourceUTests.CreateTestFoodCardResource();
            cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
        }

        // Test adding drawn unit card
        [Test]
        public void AddsDrawnUnitCard()
        {
            playerGameData.AddDrawnCard(cardUnit);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numDrawnCards);
            Assert.AreEqual(1, playerGameData.drawnCardRaces[Race.Magic]);
            Assert.AreEqual(1, playerGameData.drawnCardTypes[CardType.Unit]);
        }

        // Test adding played unit card
        [Test]
        public void AddsPlayedUnitCard()
        {
            playerGameData.AddPlayedCard(cardUnit);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numPlayedCards);
            Assert.AreEqual(1, playerGameData.playedCardRaces[Race.Magic]);
            Assert.AreEqual(1, playerGameData.playedCardTypes[CardType.Unit]);
        }

        // Test adding drawn building card
        [Test]
        public void AddsDrawnBuildingCard()
        {
            playerGameData.AddDrawnCard(cardBuilding);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numDrawnCards);
            Assert.AreEqual(1, playerGameData.drawnCardRaces[Race.None]);
            Assert.AreEqual(1, playerGameData.drawnCardTypes[CardType.Building]);
        }

        // Test adding played building card
        [Test]
        public void AddsPlayedBuildingCard()
        {
            playerGameData.AddPlayedCard(cardBuilding);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numPlayedCards);
            Assert.AreEqual(1, playerGameData.playedCardRaces[Race.None]);
            Assert.AreEqual(1, playerGameData.playedCardTypes[CardType.Building]);
        }

        // Test adding drawn resource card
        [Test]
        public void AddsDrawnResourceCard()
        {
            playerGameData.AddDrawnCard(cardResource);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numDrawnCards);
            Assert.AreEqual(1, playerGameData.drawnCardRaces[Race.None]);
            Assert.AreEqual(1, playerGameData.drawnCardTypes[CardType.Resource]);
            Assert.AreEqual(1, playerGameData.drawnResources[ResourceType.Food]);
        }

        // Test adding played resource card
        [Test]
        public void AddsPlayedResourceCard()
        {
            playerGameData.AddPlayedCard(cardResource);

            // Confirm card has been added
            Assert.AreEqual(1, playerGameData.numPlayedCards);
            Assert.AreEqual(1, playerGameData.playedCardRaces[Race.None]);
            Assert.AreEqual(1, playerGameData.playedCardTypes[CardType.Resource]);
        }
    }
}