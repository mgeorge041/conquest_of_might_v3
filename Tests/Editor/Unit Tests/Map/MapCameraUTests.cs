using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UTests.MapTests
{
    public class MapCameraUTests
    {
        private MapCamera mapCamera;
        private Vector2 cameraXBounds;
        private Vector2 cameraYBounds;
        private Vector2 cameraZoomBounds;
        private float initialOrthographicSize;
        private float initialXPos;
        private float initialYPos;

        // Create test map camera
        public static MapCamera CreateTestMapCamera()
        {
            MapCamera newMapCamera = TestFunctions.CreateClassObject<MapCamera>("Assets/Resources/Prefabs/Map Camera.prefab");
            return newMapCamera;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            mapCamera = CreateTestMapCamera();
            cameraXBounds = new Vector2(-10, 10);
            cameraYBounds = new Vector2(-10, 10);
            cameraZoomBounds = new Vector2(3, 10);
            mapCamera.SetCameraBounds(cameraXBounds.x, cameraXBounds.y, cameraYBounds.x, cameraYBounds.y);
            initialOrthographicSize = mapCamera.camera.orthographicSize;
            initialXPos = mapCamera.transform.position.x;
            initialYPos = mapCamera.transform.position.y;
        }

        // End
        [TearDown]
        public void Teardown()
        {
            C.Destroy(mapCamera.gameObject);
        }

        // Test map camera created
        [Test]
        public void CreatesMapCamera()
        {
            Assert.IsNotNull(mapCamera);
        }

        // Test map camera moves to position
        [Test]
        public void CameraMovesToPosition()
        {
            Vector3 newPosition = new Vector3(5, 5, -10);
            mapCamera.MoveCameraToPosition(newPosition);
            Assert.AreEqual(newPosition, mapCamera.transform.position);
        }

        // Test map camera does not move past bounds
        [Test]
        public void CameraMovesDoesNotMoveToPositionOutOfBounds()
        {
            Vector3 newPosition = new Vector3(15, 15);
            mapCamera.MoveCameraToPosition(newPosition);
            Assert.AreEqual(cameraXBounds.y, mapCamera.transform.position.x);
            Assert.AreEqual(cameraYBounds.y, mapCamera.transform.position.y);
        }

        // Test camera scroll wheel zoom in
        [Test]
        public void CameraZoomsIn()
        {
            mapCamera.Zoom(2);
            Assert.AreEqual(initialOrthographicSize - 1, mapCamera.camera.orthographicSize);

        }

        // Test camera zoom in does not pass bounds
        [Test]
        public void CameraDoesNotZoomInPastBounds()
        {
            mapCamera.Zoom(20);
            Assert.AreEqual(cameraZoomBounds.x, mapCamera.camera.orthographicSize);
        }

        // Test camera scroll wheel zoom out
        [Test]
        public void CameraZoomsOut()
        {
            mapCamera.Zoom(-2);
            Assert.AreEqual(initialOrthographicSize + 1, mapCamera.camera.orthographicSize);

        }

        // Test camera zoom in does not pass bounds
        [Test]
        public void CameraDoesNotZoomOutPastBounds()
        {
            mapCamera.Zoom(-20);
            Assert.AreEqual(cameraZoomBounds.y, mapCamera.camera.orthographicSize);
        }

        // Test camera moves horizontally
        [Test]
        public void CameraMovesHorizontally()
        {
            mapCamera.MoveCameraByDistance(3, 0);
            Assert.AreEqual(initialXPos + 3, mapCamera.transform.position.x);
        }

        // Test camera does not move past left bound
        [Test]
        public void CameraDoesNotMovePastLeftBound()
        {
            mapCamera.MoveCameraByDistance(30, 0);
            Assert.AreEqual(cameraXBounds.y, mapCamera.transform.position.x);
        }

        // Test camera does not move past right bound
        [Test]
        public void CameraDoesNotMovePastRightBound()
        {
            mapCamera.MoveCameraByDistance(-30, 0);
            Assert.AreEqual(cameraXBounds.x, mapCamera.transform.position.x);
        }
        
        // Test camera moves vertically
        [Test]
        public void CameraMovesVertically()
        {
            mapCamera.MoveCameraByDistance(0, 3);
            Assert.AreEqual(initialYPos + 3, mapCamera.transform.position.y);
        }

        // Test camera does not move past upper bound
        [Test]
        public void CameraDoesNotMovePastUpperBound()
        {
            mapCamera.MoveCameraByDistance(0, 30);
            Assert.AreEqual(cameraYBounds.y, mapCamera.transform.position.y);
        }

        // Test camera does not move past lower bound
        [Test]
        public void CameraDoesNotMovePastLowerBound()
        {
            mapCamera.MoveCameraByDistance(0, -30);
            Assert.AreEqual(cameraYBounds.x, mapCamera.transform.position.y);
        }


    }
}