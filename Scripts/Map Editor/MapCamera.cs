using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class MapCamera : MonoBehaviour
{
    readonly int defaultZoom = 5;
    readonly int maxZoom = 10;
    readonly int minZoom = 3;
    public float moveSpeed = 1f;
    float scrollSpeed = 0.5f;
    float maxXPos;
    float minXPos;
    float maxYPos;
    float minYPos;
    public Tilemap tilemap;
    public Camera camera;
    private Vector3 oldScrollClickPosition;
    private bool scrollDrag = false;
    public GameObject uiPanel;
    public int uiPixelWidth;

    // Centers camera on position
    public void MoveCameraToPosition(Vector3 newPosition) {
        Debug.Log("camera position: " + transform.position);
        Debug.Log("moving to: " + newPosition);
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, minXPos, maxXPos),
                Mathf.Clamp(newPosition.y, minYPos, maxYPos), -10);
    }

    //Checks for user input for camera movement across board
    public void CameraMove()
    {
        //Camera movement variables
        float moveZ = Input.mouseScrollDelta.y;

        //Zooms camera in and out
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (moveZ != 0)
            {
                float newZoom = camera.orthographicSize - moveZ * scrollSpeed;
                camera.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
                SetCameraBounds();
            }
        }

        // Moves camera via scroll wheel clicks
        if (Input.GetMouseButtonDown(2)) {
            scrollDrag = true;
            oldScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        // Dragging with mouse scroll wheel click
        if (scrollDrag) {

            // Get new mouse location and distance
            Vector3 newScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dist = -(newScrollClickPosition - oldScrollClickPosition);
            
            // Set camera position and update old position
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + dist.x, minXPos, maxXPos),
                Mathf.Clamp(transform.position.y + dist.y, minYPos, maxYPos), -10);
            oldScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        // Release mouse scroll wheel click
        if (Input.GetMouseButtonUp(2)) {
            scrollDrag = false;
        }
            
        //Moves camera via arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if (moveX != 0 || moveY != 0)
        {
            float xDist = moveX * moveSpeed * Time.fixedDeltaTime;
            float yDist = moveY * moveSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + xDist, minXPos, maxXPos), 
                Mathf.Clamp(transform.position.y + yDist, minYPos, maxYPos), -10);
        }
    }

    // Get size of map grid
    public void SetCameraBounds()
    {
        // Current map dimensions
        float tilemapMax = tilemap.cellBounds.max[0];
        float tileHeight = tilemap.layoutGrid.cellSize.x;
        float tileWidth = tilemap.layoutGrid.cellSize.y;
        float currentZoom = camera.orthographicSize;
        
        // Calculate map dimensions
        float tilemapHeight = tilemapMax * tileHeight;
        float tilemapWidth = (tilemapMax + (tilemapMax - 1) / 2) * tileWidth;
        float tilemapHalfWidth = tilemapWidth / 2;
        float cameraWidth = 2 * currentZoom * camera.aspect;
        float halfCameraWidth = currentZoom * camera.aspect;

        // Get coverage of screen by UI
        float screenWidth = Screen.width;
        float uiRatio = uiPixelWidth / screenWidth;
        float uncoveredRatio = 1 - uiRatio;
        float uncoveredScreen = uncoveredRatio * cameraWidth;
        float mapMid = (0.5f - uncoveredRatio / 2) * cameraWidth;

        // Set min and max positions
        if (tilemapHeight * 2 < Screen.height / 100) {
            maxYPos = 0;
            minYPos = 0;
        }
        else {
            maxYPos = Math.Max(tilemapHeight - currentZoom, 0);
            minYPos = -maxYPos;
        }

        if (uncoveredScreen > tilemapWidth) {
            maxXPos = mapMid;
            minXPos = mapMid;
        }
        else {
            // This is for UI that covers part of the screen
            maxXPos = Math.Max(tilemapHalfWidth - halfCameraWidth + uiRatio * cameraWidth, 0);
            minXPos = -(tilemapHalfWidth - halfCameraWidth);
            //maxXPos = Math.Max(tilemapHalfWidth - halfCameraWidth, 0);
            //minXPos = Math.Min(-(tilemapHalfWidth - halfCameraWidth), 0);
            if (minXPos > maxXPos) {
                minXPos = maxXPos;
            }
        }

        // Set camera position
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXPos, maxXPos),
                Mathf.Clamp(transform.position.y, minYPos, maxYPos), -10);
    }

    // Set tilemap
    public void SetTilemap(Tilemap tilemap) {
        this.tilemap = tilemap;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get UI panel width
        //uiPixelWidth = (int)uiPanel.GetComponent<RectTransform>().rect.width;

        // Set camera zoom
        camera.orthographicSize = defaultZoom;

        // Get tilemap bounds
        SetCameraBounds();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        if (Input.GetKey(KeyCode.Z))
            camera.orthographicSize = defaultZoom;
    }
}
