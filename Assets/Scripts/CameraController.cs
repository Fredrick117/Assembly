using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize - scroll * zoomSpeed * cam.orthographicSize,
            minZoom, maxZoom);
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(2))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += diff;
        }
    }
}
