using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCamMovement : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private float zoomAmount, minCameraSize, maxCameraSize;
    private Vector3 dragOrigin;

    [SerializeField]
    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake() {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x/2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x/2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y/2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y/2f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();

        // On scroll zoom in and out
        if(Input.GetAxis("Mouse ScrollWheel") > 0) {
            ZoomIn();
        } else if(Input.GetAxis("Mouse ScrollWheel") < 0) {
            ZoomOut();
        }
    }

    private void PanCamera() {
        if(Input.GetMouseButtonDown(0)) {
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0)) {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            
            camera.transform.position = ClampCamera(camera.transform.position + difference);
        }

        // WASD Panning
        if(Input.GetKey(KeyCode.W)) {
            camera.transform.position = ClampCamera(camera.transform.position + Vector3.up * 0.1f);
        } else if(Input.GetKey(KeyCode.S)) {
            camera.transform.position = ClampCamera(camera.transform.position + Vector3.down * 0.1f);
        } else if(Input.GetKey(KeyCode.A)) {
            camera.transform.position = ClampCamera(camera.transform.position + Vector3.left * 0.1f);
        } else if(Input.GetKey(KeyCode.D)) {
            camera.transform.position = ClampCamera(camera.transform.position + Vector3.right * 0.1f);
        }
    }

    public void ZoomIn() {
        float newSize = camera.orthographicSize - zoomAmount;

        camera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

        camera.transform.position = ClampCamera(camera.transform.position);
    }	

    public void ZoomOut() {
        float newSize = camera.orthographicSize + zoomAmount;

        camera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

        camera.transform.position = ClampCamera(camera.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition) {
        float cameraHeight = camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        float minX = mapMinX + cameraWidth;
        float maxX = mapMaxX - cameraWidth;
        float minY = mapMinY + cameraHeight;
        float maxY = mapMaxY - cameraHeight;

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(clampedX, clampedY, targetPosition.z);
    }
}
