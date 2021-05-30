using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.CardTests;
using Tests.UTests.PieceTests;

namespace Tests.ITests.PieceTests
{
    public class UnitCardUnitITests
    {
        private Unit unitWithoutCard;
        private Unit unit1;
        private Unit unit2;
        private CardUnit cardUnit;

        // Create test unit with card
        public static Unit CreateTestUnitWithCard()
        {
            CardUnit cardUnit = CardUnitUTests.CreateTestCardUnit();
            Unit unit = UnitUTests.CreateTestUnit();
            unit.SetCard(cardUnit);
            return unit;
        }

        // Setup 
        [SetUp]
        public void Setup()
        {
            unitWithoutCard = UnitUTests.CreateTestUnit();
            unit1 = CreateTestUnitWithCard();
            unit2 = CreateTestUnitWithCard();
            cardUnit = CardUnitUTests.CreateTestCardUnit();
        }

        // End
        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(unitWithoutCard.gameObject);
            Object.DestroyImmediate(unit1.gameObject);
            Object.DestroyImmediate(unit2.gameObject);
            cardUnit = null;
        }

        // Test sets card info
        [Test]
        public void SetsUnitStats()
        {
            unitWithoutCard.SetCard(cardUnit);
            Assert.AreEqual(cardUnit.cardName, unitWithoutCard.pieceName);
            Assert.AreEqual(cardUnit.health, unitWithoutCard.health);
        }

        // Test does not set if card not a CardUnit
        [Test]
        public void DoesNotSetForNonCardUnit()
        {
            unitWithoutCard.SetCard(CardBuildingUTests.CreateTestCardBuilding());
            Assert.IsNull(unitWithoutCard.GetCard());
        }

        // Test take damage
        [Test]
        public void UnitTakesCorrectDamage()
        {
            int damageDealt = 2;
            int damageTaken = unit1.TakeDamage(damageDealt);
            Assert.AreEqual(3, unit1.currentHealth);
            Assert.AreEqual(damageDealt, damageTaken);
        }

        // Test take damage greater than health
        [Test]
        public void UnitTakesMaxDamageEqualToHealth()
        {
            int damageDealt = 10;
            int damageTaken = unit1.TakeDamage(damageDealt);
            Assert.AreEqual(0, unit1.currentHealth);
            Assert.AreEqual(5, damageTaken);
        }

        // Test attack units
        [Test]
        public void UnitAttacks()
        {
            unit1.might = 3;
            unit1.AttackPiece(unit2);
            Assert.AreEqual(2, unit2.currentHealth);
            Assert.IsFalse(unit1.canAttack);
            Assert.IsFalse(unit1.canMove);
            Assert.IsFalse(unit1.hasActions);
        }

        // Test kill pieces
        [Test]
        public void UnitAttacksAndKillsUnit()
        {
            unit1.AttackPiece(unit2);
            Assert.AreEqual(0, unit2.currentHealth);
        }

        // Test losing speed
        [Test]
        public void UnitLosesSpeed()
        {
            unit1.DecreaseSpeed(1);
            Assert.AreEqual(4, unit1.remainingSpeed);
            Assert.IsTrue(unit1.canMove);
        }

        // Test losing all speed
        [Test]
        public void UnitLosesAllSpeedAndGoesToZero()
        {
            unit1.DecreaseSpeed(5);
            Assert.AreEqual(0, unit1.remainingSpeed);
            Assert.IsFalse(unit1.canMove);
        }

        // Test resetting piece
        [Test]
        public void UnitResets()
        {
            // Use all unit actions and reset
            unit1.EndTurn();
            unit1.ResetPiece();

            Assert.AreEqual(5, unit1.remainingSpeed);
            Assert.IsTrue(unit1.canMove);
            Assert.IsTrue(unit1.canAttack);
        }

        // Test ending turn
        [Test]
        public void UnitEndsTurn()
        {
            unit1.EndTurn();
            Assert.AreEqual(0, unit1.remainingSpeed);
            Assert.IsFalse(unit1.canMove);
            Assert.IsFalse(unit1.canAttack);
        }

        // Test disabling unit
        [Test]
        public void UnitDisablesWhenHasNoActions()
        {
            Color disabledColor = new Color(0.5f, 0.5f, 0.5f);
            unit1.EndTurn();
            Assert.AreEqual(disabledColor, unit1.art.color);
        }

        // Test setting lifebar width when takes damage
        [Test]
        public void SetsLifebarWidthToMatchCurrentHealth()
        {
            unit1.TakeDamage(1);
            unit1.SetLifebarCurrentHealth();
            Assert.AreEqual(0.8f, unit1.lifebar.fillAmount);
        }

        // Test changing lifebar with new health
        [Test]
        public void SetsLifebarWidthToMatchNewHealth()
        {
            Sprite[] overlays = Resources.LoadAll<Sprite>(ENV.LIFEBAR_ART_RESOURCE_PATH);
            unit1.health = 6;
            Assert.AreEqual(unit1.lifebarOverlay.sprite, overlays[5]);
        }

        // Test changing lifebar with new health and width is correct
        [Test]
        public void LifebarDeathbarOverlayWidthMatchNewHealth()
        {
            unit1.health = 6;
            Assert.AreEqual(unit1.lifebarOverlay.rectTransform.rect.width, unit1.lifebar.rectTransform.rect.width);
            Assert.AreEqual(unit1.lifebarOverlay.rectTransform.rect.width, unit1.deathbar.rectTransform.rect.width);
        }

        // Test changing lifebar with new health and width maxes at 64 pixels
        [Test]
        public void LifebarWidthMaxesAt64Pixels()
        {
            unit1.health = 7;
            Assert.AreEqual(0.64f, unit1.lifebarOverlay.rectTransform.rect.width);
        }
    }
}