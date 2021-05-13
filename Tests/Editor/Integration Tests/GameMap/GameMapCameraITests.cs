using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tests.UTests.MapTests;

namespace Tests.ITests.MapTests
{
    public class GameMapCameraITests
    {
        private GameMap gameMap;
        private MapCamera mapCamera;
        private int dumb;

        // Setup
        [SetUp]
        public void Setup()
        {
            gameMap = GameMapUTests.CreateTestGameMap();
            mapCamera = MapCameraUTests.CreateTestMapCamera();
            mapCamera.tilemap = gameMap.tilemap;
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(gameMap.gameObject);
            C.Destroy(mapCamera.gameObject);
        }

        // Test setting camera bounds
        [Test]
        public void SetsCameraBoundsToTilemap()
        {
            Debug.Log("camera bounds: " + mapCamera.CalculateCameraBounds());
            gameMap.UpdateMapToRadius(1);
            Debug.Log("new camera bounds: " + mapCamera.CalculateCameraBounds());
            Debug.Log("ortho size before: " + mapCamera.camera.orthographicSize);
            mapCamera.Zoom(140);
            Debug.Log("ortho size after: " + mapCamera.camera.orthographicSize);
            Debug.Log("newer camera bounds: " + mapCamera.CalculateCameraBounds());
        }

    }
}