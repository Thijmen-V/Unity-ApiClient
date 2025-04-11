using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField xInputField, yInputField;
    public TMP_Text xValueText, yValueText;
    [SerializeField] private Button xPlus, xMin, yPlus, yMin;

    private int xSize = 20, ySize = 10;  // Standaardwaarden
    private const int xMinValue = 20, xMaxValue = 200;
    private const int yMinValue = 10, yMaxValue = 100;

    void Start()
    {
        xInputField.text = xSize.ToString();
        yInputField.text = ySize.ToString();
        UpdateValues();

        xInputField.onEndEdit.AddListener((value) => { UpdateXSize(value); });
        yInputField.onEndEdit.AddListener((value) => { UpdateYSize(value); });

        xPlus.onClick.AddListener(() => ChangeXSize(5));
        xMin.onClick.AddListener(() => ChangeXSize(-5));
        yPlus.onClick.AddListener(() => ChangeYSize(5));
        yMin.onClick.AddListener(() => ChangeYSize(-5));
    }

    void UpdateXSize(string value)
    {
        if (int.TryParse(value, out int newXSize))
        {
            xSize = Mathf.Clamp(newXSize, xMinValue, xMaxValue);
            xInputField.text = xSize.ToString();  // Voorkomt ongeldige invoer
            UpdateValues();
        }
    }

    void UpdateYSize(string value)
    {
        if (int.TryParse(value, out int newYSize))
        {
            ySize = Mathf.Clamp(newYSize, yMinValue, yMaxValue);
            yInputField.text = ySize.ToString();  // Voorkomt ongeldige invoer
            UpdateValues();
        }
    }

    void ChangeXSize(int amount)
    {
        xSize = Mathf.Clamp(xSize + amount, xMinValue, xMaxValue);
        xInputField.text = xSize.ToString();
        UpdateValues();
    }

    void ChangeYSize(int amount)
    {
        ySize = Mathf.Clamp(ySize + amount, yMinValue, yMaxValue);
        yInputField.text = ySize.ToString();
        UpdateValues();
    }

    void UpdateValues()
    {
        xValueText.text = $"Lengte: {xSize}";
        yValueText.text = $"Hoogte: {ySize}";

        xPlus.interactable = xSize < xMaxValue;
        xMin.interactable = xSize > xMinValue;
        yPlus.interactable = ySize < yMaxValue;
        yMin.interactable = ySize > yMinValue;
    }
}
