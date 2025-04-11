using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EnvironmentPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text sizeText;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button openButton;

    private string environmentId;

    public Action<string> OnDeleteClicked;
    public Action<string, float, float> OnOpenClicked; // id, width, height

    public void SetData(string name, float length, float height, string id)
    {
        nameText.text = name;
        sizeText.text = $"{length} x {height}";
        environmentId = id;


        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() => OnDeleteClicked?.Invoke(name));

        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() =>
            OnOpenClicked?.Invoke(environmentId, length, height)
        );
    }
}