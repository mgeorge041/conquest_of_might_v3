using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;
using Tests.UTests.PieceTests;

namespace Tests.ITests.PieceTests
{
    public class BuildingCardBuildingITests
    {
        private Building buildingWithoutCard;
        private Building building1;
        private Building building2;
        private CardBuilding cardBuilding;

        // Create test unit with card
        public static Building CreateTestBuildingWithCard()
        {
            CardBuilding cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
            Building building = BuildingUTests.CreateTestBuilding();
            building.SetCard(cardBuilding);
            return building;
        }

        // Setup 
        [SetUp]
        public void Setup()
        {
            buildingWithoutCard = BuildingUTests.CreateTestBuilding();
            building1 = CreateTestBuildingWithCard();
            building2 = CreateTestBuildingWithCard();
            cardBuilding = CardBuildingUTests.CreateTestCardBuilding();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(buildingWithoutCard.gameObject);
            Object.DestroyImmediate(building1.gameObject);
            Object.DestroyImmediate(building2.gameObject);
            cardBuilding = null;
        }

        // Test sets card info
        [Test]
        public void SetsBuildingStats()
        {
            buildingWithoutCard.SetCard(cardBuilding);
            Assert.AreEqual(cardBuilding.cardName, buildingWithoutCard.pieceName);
            Assert.AreEqual(cardBuilding.health, buildingWithoutCard.health);
        }

        // Test take damage
        [Test]
        public void BuildingTakesCorrectDamage()
        {
            int damageDealt = 2;
            int damageTaken = building1.TakeDamage(damageDealt);
            Assert.AreEqual(8, building1.currentHealth);
            Assert.AreEqual(damageDealt, damageTaken);
        }

        // Test take damage greater than health
        [Test]
        public void BuildingTakesMaxDamageEqualToHealth()
        {
            int damageDealt = 20;
            int damageTaken = building1.TakeDamage(damageDealt);
            Assert.AreEqual(0, building1.currentHealth);
            Assert.AreEqual(10, damageTaken);
        }

        // Test attack units
        [Test]
        public void BuildingAttacks()
        {
            building1.might = 3;
            building1.AttackPiece(building2);
            Assert.AreEqual(7, building2.currentHealth);
            Assert.IsFalse(building1.canAttack);
            Assert.IsFalse(building1.canMove);
            Assert.IsFalse(building1.hasActions);
        }

        // Test kill pieces
        [Test]
        public void BuildingAttacksAndKillsBuilding()
        {
            building1.AttackPiece(building2);
            Assert.AreEqual(0, building2.currentHealth);
        }

        // Test resetting piece
        [Test]
        public void BuildingResets()
        {
            // Use all unit actions and reset
            building1.EndTurn();
            building1.ResetPiece();

            Assert.IsTrue(building1.canMove);
            Assert.IsTrue(building1.canAttack);
        }

        // Test ending turn
        [Test]
        public void BuildingEndsTurn()
        {
            building1.EndTurn();
            Assert.IsFalse(building1.canMove);
            Assert.IsFalse(building1.canAttack);
        }

        // Test disabling unit
        [Test]
        public void BuildingDisablesWhenHasNoActions()
        {
            Color disabledColor = new Color(0.5f, 0.5f, 0.5f);
            building1.EndTurn();
            Assert.AreEqual(disabledColor, building1.art.color);
        }

        // Test setting lifebar width when takes damage
        [Test]
        public void SetsLifebarWidthToMatchCurrentHealth()
        {
            building1.TakeDamage(1);
            building1.SetLifebarCurrentHealth();
            Assert.AreEqual(0.9f, building1.lifebar.fillAmount);
        }

        // Test changing lifebar with new health
        [Test]
        public void SetsLifebarWidthToMatchNewHealth()
        {
            Sprite[] overlays = Resources.LoadAll<Sprite>("Art/Cards/Healthbar Overlay");
            building1.health = 6;
            Assert.AreEqual(building1.lifebarOverlay.sprite, overlays[5]);
        }
    }
}