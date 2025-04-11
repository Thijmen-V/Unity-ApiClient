using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject blauweTilePrefab;
    [SerializeField] private GameObject rodeTilePrefab;
    [SerializeField] private GameObject geleTilePrefab;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        _width = EnvironmentDataHolder.Instance.width;
        _height = EnvironmentDataHolder.Instance.height;

        GenerateGrid();
        PlaceObject2Ds();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(new Vector2Int(x, y), isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    private void PlaceObject2Ds()
    {
        var objects = EnvironmentDataHolder.Instance.loadedObjects;

        foreach (var obj in objects)
        {
            Vector2 pos = new Vector2(obj.positionX, obj.positionY);
            GameObject prefabToUse = null;

            switch (obj.prefabId.ToLower())
            {
                case "blauw":
                    prefabToUse = blauweTilePrefab;
                    break;
                case "rood":
                    prefabToUse = rodeTilePrefab;
                    break;
                case "geel":
                    prefabToUse = geleTilePrefab;
                    break;
                default:
                    Debug.LogWarning($"Onbekende prefabId: {obj.prefabId}");
                    continue;
            }

            Instantiate(prefabToUse, pos, Quaternion.identity);
        }
    }
}

//using System.Collections.Generic;
//using UnityEngine;

//public class GridManager : MonoBehaviour
//{
//    [SerializeField] private int _width, _height;
//    [SerializeField] private Tile _tilePrefab;
//    [SerializeField] private Transform _cam;

//    [SerializeField] private GameObject blauweTilePrefab;
//    [SerializeField] private GameObject rodeTilePrefab;
//    [SerializeField] private GameObject geleTilePrefab;

//    private Dictionary<Vector2, Tile> _tiles;

////    private readonly Dictionary<string, Color> prefabColorMap = new Dictionary<string, Color>()
////{
////    // Rood
////    { "rood", new Color(1f, 0.25f, 0.25f) },       // Lichtrood
////    { "rood_offset", new Color(0.75f, 0f, 0f) },   // Donkerrood

////    // Blauw
////    { "blauw", new Color(0.4f, 0.6f, 1f) },        // Lichtblauw
////    { "blauw_offset", new Color(0.1f, 0.2f, 0.6f) }, // Donkerblauw

////    // Geel
////    { "geel", new Color(1f, 1f, 0.4f) },           // Lichtgeel
////    { "geel_offset", new Color(0.75f, 0.75f, 0f) } // Donkergeel
////};


//    void Start()
//    {
//        _width = EnvironmentDataHolder.Instance.width;
//        _height = EnvironmentDataHolder.Instance.height;

//        GenerateGrid();
//        PlaceObject2Ds();
//    }

//    void GenerateGrid()
//    {
//        _tiles = new Dictionary<Vector2, Tile>();
//        for (int x = 0; x < _width; x++)
//        {
//            for (int y = 0; y < _height; y++)
//            {
//                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
//                spawnedTile.name = $"Tile {x} {y}";

//                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
//                spawnedTile.Init(new Vector2Int(x, y), isOffset);

//                _tiles[new Vector2(x, y)] = spawnedTile;
//            }
//        }

//        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
//    }

//    public Tile GetTileAtPosition(Vector2 pos)
//    {
//        if (_tiles.TryGetValue(pos, out var tile))
//        {
//            return tile;
//        }
//        return null;
//    }

//    private void PlaceObject2Ds()
//    {
//        var objects = EnvironmentDataHolder.Instance.loadedObjects;

//        foreach (var obj in objects)
//        {
//            Vector2Int intPos = new Vector2Int((int)obj.positionX, (int)obj.positionY);
//            GameObject prefabToUse = null;

//            switch (obj.prefabId.ToLower())
//            {
//                case "blauw":
//                    prefabToUse = blauweTilePrefab;
//                    break;
//                case "rood":
//                    prefabToUse = rodeTilePrefab;
//                    break;
//                case "geel":
//                    prefabToUse = geleTilePrefab;
//                    break;
//                default:
//                    Debug.Log($"Onbekende prefabId: {obj.prefabId}");
//                    continue;
//            }

//            GameObject tileObj = Instantiate(prefabToUse, (Vector2)intPos, Quaternion.identity);

//            TilePlacementManager.Instance.RegisterPlacedTile(intPos, tileObj);
//        }
//    }


//    //private void PlaceObject2Ds()
//    //{
//    //    var objects = EnvironmentDataHolder.Instance.loadedObjects;

//    //    foreach (var obj in objects)
//    //    {
//    //        Vector2 pos = new Vector2(obj.positionX, obj.positionY);
//    //        GameObject prefabToUse = null;

//    //        switch (obj.prefabId.ToLower())
//    //        {
//    //            case "blauw":
//    //                prefabToUse = blauweTilePrefab;
//    //                break;
//    //            case "rood":
//    //                prefabToUse = rodeTilePrefab;
//    //                break;
//    //            case "geel":
//    //                prefabToUse = geleTilePrefab;
//    //                break;
//    //            default:
//    //                Debug.Log($"Onbekende prefabId: {obj.prefabId}");
//    //                continue;
//    //        }

//    //        // Bepaal of deze positie een offset tile is
//    //        bool isOffset = IsOffset((int)pos.x, (int)pos.y);
//    //        string colorKey = obj.prefabId.ToLower() + (isOffset ? "_offset" : "");

//    //        GameObject tileObj = Instantiate(prefabToUse, pos, Quaternion.identity);

//    //        // Gebruik vaste kleur uit colormap
//    //        if (tileObj.TryGetComponent<SpriteRenderer>(out var renderer))
//    //        {
//    //            if (prefabColorMap.TryGetValue(colorKey, out var finalColor))
//    //            {
//    //                renderer.color = finalColor;
//    //            }
//    //            else
//    //            {
//    //                Debug.LogWarning($"Geen kleur gevonden voor sleutel: {colorKey}");
//    //            }
//    //        }
//    //    }
//    //}

//    private bool IsOffset(int x, int y)
//    {
//        return (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
//    }

//    private Color ApplyOffsetColor(Color baseColor, bool isOffset)
//    {
//        if (!isOffset) return baseColor;

//        return baseColor * 0.85f; // of 0.75f als je donkerder wilt
//    }
//}
