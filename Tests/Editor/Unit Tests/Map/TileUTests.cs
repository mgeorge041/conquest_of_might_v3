using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.MapTests
{
    public class TileUTests
    {
        public static TileData CreateTestTileData()
        {
            TileData tileData = Resources.Load<TileData>("Tiles/Tile Data/Default");
            return tileData;
        }

        // Test create tile data
        [Test]
        public void CreatesTileData()
        {
            TileData tileData = CreateTestTileData();
            Assert.IsNotNull(tileData);
            Assert.AreEqual("Default", tileData.tileName);
            Assert.AreEqual(1, tileData.moveCost);
        }
    }
}