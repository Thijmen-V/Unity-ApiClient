using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleApp : MonoBehaviour
{
    [Header("Login/Regist Panel")]
    //public User user;
    [SerializeField] private GameObject loginAndRegister_Panel;
    public TMP_InputField emailInputField, passwordInputField;
    public TMP_Text loginStatusTxt, registerStatusTxt, algemeneStatusTxt;

    [Header("Home Panel")]
    //public User user;
    [SerializeField] private GameObject Home_Panel;
    [SerializeField] private GameObject[] EnviormentPanels;
    [SerializeField] private GameObject[] EnviormentPanels_Close;
    public TMP_Text ReadEnvironmentsStatus_Txt;

    [Header("Nieuw Environment Panel")]
    [SerializeField] private GameObject NewEnvironment_Panel;
    public TMP_InputField max_X, max_Y, environName;
    public TMP_Text NewEnvironmentStatus_Txt;

    public Environment2D environment2D;
    public Object2D object2D;

    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public Environment2DApiClient enviroment2DApiClient;
    public Object2DApiClient object2DApiClient;

    Dictionary<(float, float), Object2D> filtrerenDict = new();
    List<Object2D> gefilterdeObj = new();

    void Start()
    {
        algemeneStatusTxt.text = "";
        loginStatusTxt.text = "";
        registerStatusTxt.text = "";
        NewEnvironmentStatus_Txt.text = "";
        ReadEnvironmentsStatus_Txt.text = "";

        if (EnvironmentDataHolder.Instance != null && EnvironmentDataHolder.Instance.returningFromBuilder)
        {
            Home_Panel.SetActive(true);
            loginAndRegister_Panel.SetActive(false);

            if(EnvironmentDataHolder.Instance.retunsByS)
            {
                if (EnvironmentDataHolder.Instance.placesObjects != null && EnvironmentDataHolder.Instance.retunsByS)
                {
                    filtrerenDict.Clear(); // Clear de lijst
                    gefilterdeObj.Clear();
                    foreach (var ongefilterdeObjecten in EnvironmentDataHolder.Instance.placesObjects)
                    {
                        //Debug.Log($"=Object ID: {ongefilterdeObjecten.prefabId} x: {ongefilterdeObjecten.positionX}, y: {ongefilterdeObjecten.positionY}, Environment ID: {ongefilterdeObjecten.environmentId}");

                        if (string.IsNullOrEmpty(ongefilterdeObjecten.prefabId))
                            continue;

                        var positieKey = (ongefilterdeObjecten.positionX, ongefilterdeObjecten.positionY);
                        filtrerenDict[positieKey] = ongefilterdeObjecten; // overschrijft automatisch bij duplicate posities
                    }
                    gefilterdeObj = filtrerenDict.Values.ToList(); // Converteer de dictionary waarden naar een lijst
                    foreach (var alleObj in gefilterdeObj)
                    {
                        // Zoek of er al een object bestaat op dezelfde positie
                        var bestaandObj = EnvironmentDataHolder.Instance.loadedObjects.FirstOrDefault(loaded =>
                            loaded.positionX == alleObj.positionX &&
                            loaded.positionY == alleObj.positionY);

                        //Object2D object2D = new Object2D(); // nieuwe instantie per loop

                        if (bestaandObj == null)
                        {
                            Debug.Log($"+Object wordt gemaakt: {alleObj.prefabId} x: {alleObj.positionX}, y: {alleObj.positionY}");
                            object2D.environmentId = alleObj.environmentId;
                            object2D.prefabId = alleObj.prefabId;
                            object2D.positionX = alleObj.positionX;
                            object2D.positionY = alleObj.positionY;
                            object2D.scaleX = 1;
                            object2D.scaleY = 1;
                            object2D.rotationZ = 0;
                            object2D.sortingLayer = 1;
                            CreateObject2D();
                        }
                        else
                        {
                            Debug.Log($"Object wordt geüpdatet: {alleObj.prefabId} x: {alleObj.positionX}, y: {alleObj.positionY}");
                            object2D.id = bestaandObj.id; // neem bestaand ID over
                            object2D.environmentId = alleObj.environmentId;
                            object2D.prefabId = alleObj.prefabId;
                            object2D.positionX = alleObj.positionX;
                            object2D.positionY = alleObj.positionY;
                            object2D.scaleX = 1;
                            object2D.scaleY = 1;
                            object2D.rotationZ = 0;
                            object2D.sortingLayer = 1;
                            UpdateObject2D();
                        }
                    }


                    Debug.Log("Objecten geladen en opgeslagen in de database.");
                    EnvironmentDataHolder.Instance.placesObjects.Clear(); // Clear de lijst
                    EnvironmentDataHolder.Instance.loadedObjects.Clear(); // Clear de lijst
                }
                else
                {
                    Debug.Log("Geen objecten geladen.");
                }
            }
            EnvironmentDataHolder.Instance.returningFromBuilder = false; // reset
            EnvironmentDataHolder.Instance.retunsByS = false; // reset

            ReadEnvironment2Ds(); // Herlaad de lijst
        }
        else
        {
            Home_Panel.SetActive(false);
            loginAndRegister_Panel.SetActive(true);
        }

        NewEnvironment_Panel.SetActive(false);
    }
    public void OpenAddEnvironmentPanel()
    {
        environName.text = "";
        //max_X.text = "";
        //max_Y.text = "";
        NewEnvironment_Panel.SetActive(true);
        loginAndRegister_Panel.SetActive(false);
        Home_Panel.SetActive(false);
    }

    public void GoToHomePage()
    {
        ReadEnvironment2Ds();
        NewEnvironment_Panel.SetActive(false);
        loginAndRegister_Panel.SetActive(false);
        Home_Panel.SetActive(true);
    }

    public void SetEnvironmentsActive(int active_TM)
    {
        for (int i = 0; i < EnviormentPanels.Length; i++)
        {
            bool isActive = i < active_TM;
            EnviormentPanels[i].SetActive(isActive);
            EnviormentPanels_Close[i].SetActive(!isActive);
        }
    }

    public void OpenEnvironment(string environmentId)
    {
        object2D.environmentId = environmentId;
        ReadObject2Ds();
    }

    public async void OpenEnvironment(string environmentId, float length, float height)
    {
        EnvironmentDataHolder.Instance.environmentId = environmentId;
        EnvironmentDataHolder.Instance.width = Mathf.RoundToInt(length);
        EnvironmentDataHolder.Instance.height = Mathf.RoundToInt(height);

        // Objecten ophalen en opslaan
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(environmentId);
        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                EnvironmentDataHolder.Instance.loadedObjects = dataResponse.Data;
                SceneManager.LoadScene("BuilderScene");
                break;

            case WebRequestError errorResponse:
                Debug.LogError("Fout bij laden van objecten: " + errorResponse.ErrorMessage);
                break;
        }
    }



    List<string> environmentsNames = new List<string>();


    #region Login
    User user = new User();

    [ContextMenu("User/Register")]
    public async void Register()
    {
        user.email = emailInputField.text;
        user.password = passwordInputField.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                // TODO: Handle succes scenario;
                registerStatusTxt.text = "";
                algemeneStatusTxt.text = "Succesvol geregistreerd!";
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                if (user.email == "")
                {
                    registerStatusTxt.text = "Vul een email in!";
                }
                else if (user.password == "")
                {
                    registerStatusTxt.text = "Vul een wachtwoord in!";
                }
                else
                {
                    registerStatusTxt.text = "Helaas, registreren is niet gelukt. Check of: \n Je account al bestaat, probeer in te loggen! \n Je wachtwoord minimaal 10 karaters lang is en ten minste 1 lowercase, uppercase, cijfer en niet-alphanummeriek karakter bevat!";
                }
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login()
    {
        user.email = emailInputField.text;
        user.password = passwordInputField.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                loginStatusTxt.text = "";
                algemeneStatusTxt.text = "Succesvol ingelogd!";
                ReadEnvironment2Ds();
                Home_Panel.SetActive(true);
                loginAndRegister_Panel.SetActive(false);
                // TODO: Todo handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                if (user.email == "")
                {
                    loginStatusTxt.text = "Vul een email in!";
                }
                else if (user.password == "")
                {
                    loginStatusTxt.text = "Vul een wachtwoord in!";
                }
                else if (errorMessage.Contains("Unauthorized"))
                {
                    loginStatusTxt.text = "Je wachtwoord of email is verkeerd!";
                }
                else
                {
                    loginStatusTxt.text = errorMessage;
                }
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion

    #region Environment

    [ContextMenu("Environment2D/Read all")]
    public async void ReadEnvironment2Ds()
    {
        environmentsNames.Clear();
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.ReadEnvironment2Ds();

        switch (webRequestResponse)
        {
            case WebRequestData<List<Environment2D>> dataResponse:
                List<Environment2D> environment2Ds = dataResponse.Data;
                Debug.Log("List of environment2Ds: ");
                ReadEnvironmentsStatus_Txt.text = "";

                for (int i = 0; i < environment2Ds.Count && i < EnviormentPanels.Length; i++)
                {
                    environmentsNames.Add(environment2Ds[i].name); //namen toevoegen om later te kijken of de namen al bestaan
                    var environment = environment2Ds[i];
                    var panel = EnviormentPanels[i];
                    var ui = panel.GetComponent<EnvironmentPanelUI>();
                    if (ui != null)
                    {
                        ui.SetData(environment.name, environment.maxLength, environment.maxHeight, environment.id);
                        ui.OnDeleteClicked = DeleteEnvironment2D;
                        ui.OnOpenClicked = OpenEnvironment;

                    }
                }
                SetEnvironmentsActive(environment2Ds.Count);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read environment2Ds error: " + errorMessage);
                ReadEnvironmentsStatus_Txt.text = errorMessage;
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Environment2D/Create")]
    public async void CreateEnvironment2D()
    {
        environment2D.name = environName.text;
        environment2D.maxLength = int.Parse(max_X.text);
        environment2D.maxHeight = int.Parse(max_Y.text);


        if (environmentsNames.Any(n => n.Equals(environment2D.name, StringComparison.OrdinalIgnoreCase)))
        {
            Debug.Log("Dit environment bestaat al!");
            NewEnvironmentStatus_Txt.text = "Dit environment bestaat al!";
            return;
        }
        if (environment2D.maxLength < 20 || environment2D.maxLength > 200)
        {
            Debug.Log("De lengte moet tussen de 20 en 200 zijn!");
            NewEnvironmentStatus_Txt.text = "De lengte moet tussen de 20 en 200 zijn!";
            return;
        }
        else if (environment2D.maxHeight < 10 || environment2D.maxHeight > 100)
        {
            Debug.Log("De hoogte moet tussen de 10 en 100 zijn!");
            NewEnvironmentStatus_Txt.text = "De hoogte moet tussen de 10 en 100 zijn!";
            return;
        }

        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.CreateEnvironment(environment2D);
        switch (webRequestResponse)
        {
            case WebRequestData<Environment2D> dataResponse:
                environment2D.id = dataResponse.Data.id;
                Debug.Log("Create environment2D succes!");
                GoToHomePage();
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create environment2D error: " + errorMessage);
                NewEnvironmentStatus_Txt.text = errorMessage + " \n Mogelijke oplossingen: \n Is de naam tussen 1 en 25 karakters? \n Heb je al 5 environments? ";
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Environment2D/Delete")]

    public async void DeleteEnvironment2D(string name)
    {
        environment2D.name = name;
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.DeleteEnvironment(name);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log($"Delete environment '{name}' success!");
                ReadEnvironment2Ds(); // Herlaad de lijst
                ReadEnvironmentsStatus_Txt.text = name + " is verwijderd!";
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log($"Delete environment '{name}' error: {errorMessage}");
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion Environment

    #region Object2D

    [ContextMenu("Object2D/Read all")]
    public async void ReadObject2Ds()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(object2D.environmentId);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                Debug.Log("List of object2Ds: " + object2Ds);
                object2Ds.ForEach(object2D => Debug.Log(object2D.id));
                // TODO: Succes scenario. Show the enviroments in the UI
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read object2Ds error: " + errorMessage);
                // TODO: Error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Object2D/Create")]
    public async void CreateObject2D()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.CreateObject2D(object2D);

        switch (webRequestResponse)
        {
            case WebRequestData<Object2D> dataResponse:
                object2D.id = dataResponse.Data.id;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create Object2D error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Object2D/Update")]
    public async void UpdateObject2D()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.UpdateObject2D(object2D);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                string responseData = dataResponse.Data;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Update object2D error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
    #endregion

}
