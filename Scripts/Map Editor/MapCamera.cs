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
    private Tilemap _tilemap;
    public Tilemap tilemap
    {
        get { return _tilemap; }
        set { 
            _tilemap = value;
            UpdateCameraBounds();
        }
    }
    public Camera camera;
    private bool isScrollDrag = false;
    private Vector3 oldScrollClickPosition;
    public GameObject uiPanel;
    public int uiPixelWidth;
    public int uiPixelHeight;

    // Centers camera on position
    public void MoveCameraToPosition(Vector3 newPosition) {
        //transform.position = new Vector3(Mathf.Clamp(newPosition.x, minXPos, maxXPos),
        //        Mathf.Clamp(newPosition.y, minYPos, maxYPos), -10);
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, minXPos, maxXPos),
                Mathf.Clamp(newPosition.y, minYPos, maxYPos), -10);
    }

    // Centers camera on position
    public void MoveCameraToPosition(float xPos, float yPos)
    {
        transform.position = new Vector3(Mathf.Clamp(xPos, minXPos, maxXPos),
                Mathf.Clamp(yPos, minYPos, maxYPos), -10);
    }

    // Move camera based on distance
    public void MoveCameraByDistance(float xDist, float yDist)
    {
        MoveCameraToPosition(transform.position.x + xDist, transform.position.y + yDist);
    }

    // Zoom camera in or out
    public void Zoom(float moveZ)
    {
        float newZoom = camera.orthographicSize - moveZ * scrollSpeed;
        camera.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    // Checks for user input for camera movement across board
    public void CameraMove()
    {
        // Check for zoom
        float moveZ = Input.mouseScrollDelta.y;
        if (!EventSystem.current.IsPointerOverGameObject() && moveZ != 0)
        {
            Zoom(moveZ);
            UpdateCameraBounds();
        }

        // Check for camera move via arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float xDist = moveX * moveSpeed * Time.fixedDeltaTime;
        float yDist = moveY * moveSpeed * Time.fixedDeltaTime;
        MoveCameraByDistance(xDist, yDist);

        // Check for panning camera
        if (Input.GetMouseButtonDown(2))
        {
            // Get initial position
            oldScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            isScrollDrag = true;
            Debug.Log("scroll drag");
        }
        if (isScrollDrag) { 
            

            // Get new mouse location and distance
            Vector3 newScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dist = -(newScrollClickPosition - oldScrollClickPosition);
            MoveCameraToPosition(new Vector3(transform.position.x + dist.x, transform.position.y + dist.y));

            // Update position
            oldScrollClickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(2))
        {
            isScrollDrag = false;
        }
    }

    // Set camera bounds
    public void SetCameraBounds(Vector4 bounds)
    {
        SetCameraXBounds(bounds.x, bounds.y);
        SetCameraYBounds(bounds.z, bounds.w);
    }

    // Set camera bounds
    public void SetCameraBounds(float minX, float maxX, float minY, float maxY)
    {
        SetCameraXBounds(minX, maxX);
        SetCameraYBounds(minY, maxY);
    }

    // Set camera X bounds
    public void SetCameraXBounds(float minX, float maxX)
    {
        minXPos = minX;
        maxXPos = maxX;
    }

    // Set camera Y bounds
    public void SetCameraYBounds(float minY, float maxY)
    {
        minYPos = minY;
        maxYPos = maxY;
    }

    // Calculate camera bounds from tilemap
    public Vector4 CalculateCameraBounds()
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
        float uiHorizontalRatio = uiPixelWidth / screenWidth;
        float uncoveredRatio = 1 - uiHorizontalRatio;
        float uncoveredScreen = uncoveredRatio * cameraWidth;
        float mapMid = (0.5f - uncoveredRatio / 2) * cameraWidth;
        float uiVerticalRatio = uiPixelHeight / 100;

        float minX;
        float minY;
        float maxX;
        float maxY;

        // Set min and max positions
        if (tilemapHeight * 2 < Screen.height / 100)
        {
            minY = 0;
            maxY = 0;
        }
        else
        {
            maxY  = Math.Max(tilemapHeight - currentZoom, 0);
            minY = -maxY - uiVerticalRatio;
        }

        if (uncoveredScreen > tilemapWidth)
        {
            minX = mapMid;
            maxX = mapMid;
        }
        else
        {
            // This is for UI that covers part of the screen
            maxX = Math.Max(tilemapHalfWidth - halfCameraWidth + uiHorizontalRatio * cameraWidth, 0);
            minX = -(tilemapHalfWidth - halfCameraWidth);
            //maxXPos = Math.Max(tilemapHalfWidth - halfCameraWidth, 0);
            //minXPos = Math.Min(-(tilemapHalfWidth - halfCameraWidth), 0);
            if (minX > maxX)
            {
                minX = maxX;
            }
        }

        return new Vector4(minX, maxX, minY, maxY);
    }

    // Get size of map grid
    public void UpdateCameraBounds()
    {
        Vector4 newCameraBounds = CalculateCameraBounds();
        SetCameraBounds(newCameraBounds);
        MoveCameraToPosition(transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get UI panel width
        //uiPixelWidth = (int)uiPanel.GetComponent<RectTransform>().rect.width;

        // Set camera zoom
        camera.orthographicSize = defaultZoom;

        // Get tilemap bounds
        UpdateCameraBounds();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        if (Input.GetKey(KeyCode.Z))
            camera.orthographicSize = defaultZoom;
    }
}
