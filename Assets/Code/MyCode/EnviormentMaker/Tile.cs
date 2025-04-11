using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _highlight_red;
    [SerializeField] private GameObject _highlight_blue;
    [SerializeField] private GameObject _highlight_yellow;

    private Vector2Int _position;

    private void Start()
    {
        EnvironmentDataHolder.Instance.placesObjects = new List<Object2D>();
    }
    public void Init(Vector2Int position, bool isOffset)
    {
        _position = position;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        if (TilePlacementManager.Instance.selectedColor == "rood")
        {
            _highlight_red.SetActive(true);
            _highlight_blue.SetActive(false);
            _highlight_yellow.SetActive(false);
            _highlight.SetActive(false);
        }
        else if (TilePlacementManager.Instance.selectedColor == "blauw")
        {
            _highlight_red.SetActive(false);
            _highlight_blue.SetActive(true);
            _highlight_yellow.SetActive(false);
            _highlight.SetActive(false);
        }
        else if (TilePlacementManager.Instance.selectedColor == "geel")
        {
            _highlight_red.SetActive(false);
            _highlight_blue.SetActive(false);
            _highlight_yellow.SetActive(true);
            _highlight.SetActive(false);
        }
        else
        {
            _highlight_red.SetActive(false);
            _highlight_blue.SetActive(false);
            _highlight_yellow.SetActive(false);
            _highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        _highlight_red.SetActive(false);
        _highlight_blue.SetActive(false);
        _highlight_yellow.SetActive(false);
    }
    void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        if (TilePlacementManager.Instance.selectedColor != null )
        {
            if (Input.GetMouseButton(0))
            {
                TilePlacementManager.Instance.PlaceTileAt(_position);
                EnvironmentDataHolder.Instance.placesObjects.Add(new Object2D
                {
                    environmentId = EnvironmentDataHolder.Instance.environmentId,
                    prefabId = TilePlacementManager.Instance.selectedColor,
                    positionX = _position.x,
                    positionY = _position.y,
                    //standaard values
                    //scaleX = 1,
                    //scaleY = 1,
                    //rotationZ = 0,
                    //sortingLayer = 1,
                });
            }
            //else if (Input.GetMouseButton(1))
            //{
            //    Debug.Log("Right mouse button clicked"); //godsamme dit werkt niet... T is gelukkig geen must have ;)
            //    TilePlacementManager.Instance.RemoveTileAt(_position);
            //}
        }
        
    }
}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class Tile : MonoBehaviour
//{
//    [SerializeField] private Color _baseColor, _offsetColor;
//    [SerializeField] private SpriteRenderer _renderer;
//    [SerializeField] private GameObject _highlight;

//    private Vector2Int _position;

//    private void Start()
//    {
//        EnvironmentDataHolder.Instance.placesObjects = new List<Object2D>();
//    }
//    public void Init(Vector2Int position, bool isOffset)
//    {
//        _position = position;
//        _renderer.color = isOffset ? _offsetColor : _baseColor;
//    }

//    void OnMouseEnter()
//    {
//        _highlight.SetActive(true);
//    }

//    void OnMouseExit()
//    {
//        _highlight.SetActive(false);
//    }
//    void OnMouseDown()
//    {
//        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
//        if (TilePlacementManager.Instance.selectedColor != null)
//        { 
//            if (Input.GetMouseButton(0))
//            {
//                //TilePlacementManager.Instance.PlaceTileAt(_position);
//                TilePlacementManager.Instance.PlaceTileAt(_position/*, _renderer.color == _offsetColor*/);
//                EnvironmentDataHolder.Instance.placesObjects.Add(new Object2D
//                {
//                    environmentId = EnvironmentDataHolder.Instance.environmentId,
//                    prefabId = TilePlacementManager.Instance.selectedColor,
//                    positionX = _position.x,
//                    positionY = _position.y,
//                    //standaard values
//                    //scaleX = 1,
//                    //scaleY = 1,
//                    //rotationZ = 0,
//                    //sortingLayer = 1,
//                });
//            }
//            else/* if (Input.GetMouseButton(1))*/
//            {
//                Debug.Log("Right mouse button clicked");
//                TilePlacementManager.Instance.RemoveTileAt(_position);
//            }
//        }

//    }

//    //void Update()
//    //{
//    //    if (Input.GetMouseButtonDown(0)) Debug.Log("Pressed left click.");
//    //    if (Input.GetMouseButtonDown(1)) Debug.Log("Pressed right click.");
//    //    if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
//    //}


//}