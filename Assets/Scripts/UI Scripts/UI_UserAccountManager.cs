using UnityEngine;
using System.Collections;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UI_UserAccountManager : MonoBehaviour {

    public static UI_UserAccountManager instance;
    UI_LoginMenu uiLoginMenu;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

    }

    void Start()
    {
        if (Network_SceneChecker.instance.sceneName == loggedOutSceneName)
        {
            uiLoginMenu = gameObject.GetComponent<UI_LoginMenu>();
        }
    }

    public static string LoggedIn_Username { get; protected set; } //stores username once logged in
    private static string LoggedIn_Password = ""; //stores the password once logged in

    public static bool IsLoggedIn { get; protected set; }

    public string loggedInSceneName = "Lobby_Scene";
    public string loggedOutSceneName = "Network_Login";

    public delegate void OnDataRecievedCallback(string data);

    public void LogOut()
    {
        LoggedIn_Username = "";
        LoggedIn_Password = "";

        IsLoggedIn = false;

        Debug.Log("User logged out!");

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn(string username, string password)
    {
        LoggedIn_Username = username;
        LoggedIn_Password = password;

        IsLoggedIn = true;

        Debug.Log("Logged in as " + username);

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void SendData(string data)
    {
        //called when the 'Send Data' button on the data part is pressed

        if (IsLoggedIn)
        {
            StartCoroutine(sendSendDataRequest(LoggedIn_Username, LoggedIn_Password, data));
        }
    }

    IEnumerator sendSendDataRequest(string username, string password, string data)
    {
        IEnumerator e = DCF.SetUserData(username, password, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success" && Network_SceneChecker.instance.sceneName == loggedOutSceneName)
        {
            uiLoginMenu.SendDataRequestSuccess();
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            username = "";
            password = "";

            if (Network_SceneChecker.instance.sceneName == loggedOutSceneName)
            {
                uiLoginMenu.SendDataRequestError();
            }
        }
    }

    public void GetData(OnDataRecievedCallback onDataReceived)
    {
        // called when the 'Get Data' button on the data part is pressed

       if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendGetDataRequest(LoggedIn_Username, LoggedIn_Password, onDataReceived));
        }
    }

    IEnumerator sendGetDataRequest(string username, string password, OnDataRecievedCallback onDataReceived)
    {
        string data = "Error";

        IEnumerator e = DCF.GetUserData(username, password); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error" && Network_SceneChecker.instance.sceneName == loggedOutSceneName)
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            username = "";
            password = "";
            uiLoginMenu.GetDataRequestError();
            data = response;
        }
        else
        {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            //uiLoginMenu.GetDataRequestSuccess();
            data = response;
            // LoggedIn_DataOutputField.text = response;
        }

        if (onDataReceived != null)
            onDataReceived.Invoke(data);
    }
}
