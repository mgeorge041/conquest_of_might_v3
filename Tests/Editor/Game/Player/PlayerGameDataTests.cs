using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

namespace Tests
{
    public class PlayerGameDataTests
    {
        // Create a player game data object
        public static PlayerGameData CreateTestPlayerGameData() {
            PlayerGameData gameData = new PlayerGameData();
            return gameData;
        }

        // Test adding drawn card
        [Test]
        public void AddsDrawnUnitCard() {
            PlayerGameData gameData = CreateTestPlayerGameData();
            Card testCard = CardTests.LoadTestUnitCard();
            gameData.AddDrawnCard(testCard);

            // Confirm card has been added
            Assert.AreEqual(1, gameData.numDrawnCards);
            Assert.AreEqual(1, gameData.drawnCardRaces[Race.Magic]);
            Assert.AreEqual(1, gameData.drawnCardTypes[CardType.Unit]);
        }

        // Test adding played card
        [Test]
        public void AddsPlayedUnitCard() {
            PlayerGameData gameData = CreateTestPlayerGameData();
            Card testCard = CardTests.LoadTestUnitCard();
            gameData.AddPlayedCard(testCard);

            // Confirm card has been added
            Assert.AreEqual(1, gameData.numPlayedCards);
            Assert.AreEqual(1, gameData.playedCardRaces[Race.Magic]);
            Assert.AreEqual(1, gameData.playedCardTypes[CardType.Unit]);
        }

        // Test adding drawn card
        [Test]
        public void AddsDrawnResourceCard() {
            PlayerGameData gameData = CreateTestPlayerGameData();
            Card testCard = CardTests.LoadTestResourceCard();
            gameData.AddDrawnCard(testCard);

            // Confirm card has been added
            Assert.AreEqual(1, gameData.numDrawnCards);
            Assert.AreEqual(1, gameData.drawnCardRaces[Race.None]);
            Assert.AreEqual(1, gameData.drawnCardTypes[CardType.Resource]);
            Assert.AreEqual(1, gameData.drawnResources[ResourceType.Food]);
        }

        // Test adding played card
        [Test]
        public void AddsPlayedResourceCard() {
            PlayerGameData gameData = CreateTestPlayerGameData();
            Card testCard = CardTests.LoadTestResourceCard();
            gameData.AddPlayedCard(testCard);

            // Confirm card has been added
            Assert.AreEqual(1, gameData.numPlayedCards);
            Assert.AreEqual(1, gameData.playedCardRaces[Race.None]);
            Assert.AreEqual(1, gameData.playedCardTypes[CardType.Resource]);
        }
    }
}
