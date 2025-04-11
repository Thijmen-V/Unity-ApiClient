//using UnityEngine;

//public class CameraController : MonoBehaviour
//{
//    [SerializeField] private float zoomSpeed = 5f;
//    [SerializeField] private float minZoom = 3f;
//    private float maxZoom;

//    private Camera _camera;
//    private Vector3 dragOrigin;

//    void Start()
//    {
//        _camera = Camera.main;
//        // Wacht tot EnvironmentDataHolder beschikbaar is
//        AdjustMaxZoom();
//    }

//    void Update()
//    {
//        HandleZoom();
//        HandleDrag();
//    }

//    private void HandleZoom()
//    {
//        float scroll = Input.GetAxis("Mouse ScrollWheel");
//        if (scroll != 0)
//        {
//            _camera.orthographicSize -= scroll * zoomSpeed;
//            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
//        }
//    }

//    private void HandleDrag()
//    {
//        if (Input.GetMouseButtonDown(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
//        {
//            dragOrigin = Input.mousePosition;
//        }

//        if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
//        {
//            Vector3 difference = _camera.ScreenToWorldPoint(dragOrigin) - _camera.ScreenToWorldPoint(Input.mousePosition);
//            _camera.transform.position += difference;
//            dragOrigin = Input.mousePosition;
//        }
//    }

//    public void AdjustMaxZoom()
//    {
//        int worldWidth = EnvironmentDataHolder.Instance.width;
//        int worldHeight = EnvironmentDataHolder.Instance.height;

//        float aspectRatio = (float)Screen.width / Screen.height;
//        float widthBasedZoom = worldWidth / (2f * aspectRatio) + 1;
//        float heightBasedZoom = worldHeight / 2f + 1;

//        maxZoom = Mathf.Max(widthBasedZoom, heightBasedZoom);
//    }
//}

using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    private float maxZoom;

    private Camera _camera;
    private Vector3 dragOrigin;

    private IEnumerator Start()
    {
        _camera = Camera.main;

        // Wacht totdat de EnvironmentDataHolder is geladen
        yield return new WaitUntil(() => EnvironmentDataHolder.Instance != null);

        AdjustMaxZoom();
    }

    void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _camera.orthographicSize -= scroll * zoomSpeed;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
        {
            Vector3 difference = _camera.ScreenToWorldPoint(dragOrigin) - _camera.ScreenToWorldPoint(Input.mousePosition);
            _camera.transform.position += difference;
            dragOrigin = Input.mousePosition;
        }
    }

    public void AdjustMaxZoom()
    {
        int worldWidth = EnvironmentDataHolder.Instance.width;
        int worldHeight = EnvironmentDataHolder.Instance.height;

        float aspectRatio = (float)Screen.width / Screen.height;
        float widthBasedZoom = worldWidth / (2f * aspectRatio) + 1;
        float heightBasedZoom = worldHeight / 2f + 1;

        maxZoom = Mathf.Max(widthBasedZoom, heightBasedZoom);
    }
}
