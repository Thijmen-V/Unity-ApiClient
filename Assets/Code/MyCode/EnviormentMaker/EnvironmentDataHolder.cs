using UnityEngine;
using System.Collections.Generic;

public class EnvironmentDataHolder : MonoBehaviour
{
    public static EnvironmentDataHolder Instance;

    public string environmentId;
    public int width;
    public int height;
    public List<Object2D> loadedObjects;
    public List<Object2D> placesObjects;

    public bool returningFromBuilder = false;
    public bool retunsByS = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
