using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TilePlacementManager : MonoBehaviour
{
    public static TilePlacementManager Instance;

    [SerializeField] private GameObject blauweTilePrefab;
    [SerializeField] private GameObject rodeTilePrefab;
    [SerializeField] private GameObject geleTilePrefab;
    [SerializeField] private Transform objectParent;

    private Dictionary<Vector2Int, GameObject> placedObjects = new Dictionary<Vector2Int, GameObject>();

    public string selectedColor = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleColor("rood");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleColor("blauw");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleColor("geel");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            EnvironmentDataHolder.Instance.returningFromBuilder = true;
            EnvironmentDataHolder.Instance.retunsByS = true;
            SceneManager.LoadScene("SampleScene");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnvironmentDataHolder.Instance.returningFromBuilder = true;
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void ToggleColor(string color)
    {
        if (selectedColor == color)
            selectedColor = null; // deselect
        else
            selectedColor = color;

        Debug.Log("Selected color: " + selectedColor);
    }

    public void PlaceTileAt(Vector2Int position)
    {
        if (string.IsNullOrEmpty(selectedColor)) return;

        GameObject prefabToUse = GetPrefabByColor(selectedColor);
        if (prefabToUse == null) return;

        // Als er al iets staat --> overschrijven
        RemoveTileAt(position);

        Vector3 worldPos = new Vector3(position.x, position.y, 0);
        GameObject tile = Instantiate(prefabToUse, worldPos, Quaternion.identity, objectParent);
        placedObjects[position] = tile;
    }

    public void RemoveTileAt(Vector2Int position)
    {
        if (placedObjects.TryGetValue(position, out GameObject existing))
        {
            Destroy(existing);
            placedObjects.Remove(position);
        }
    }

    private GameObject GetPrefabByColor(string color)
    {
        switch (color.ToLower())
        {
            case "rood": return rodeTilePrefab;
            case "blauw": return blauweTilePrefab;
            case "geel": return geleTilePrefab;
            default: return null;
        }
    }
}


