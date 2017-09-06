using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI_ButtonScripts : MonoBehaviour 
{
    static string lastScene;
    static string currentScene;

    //Once a button is pressed the scene will change to another scene depending on the assigned scene in the string: NameofLevel
    public void ChangeScreen(string NameofLevel)
    {
        SceneManager.LoadScene(NameofLevel);

        Debug.Log("Please wait as the scene changes");
    }

    public void LoadCharacterSelectionLAN(string NameofLevel)
    {
        Network_SceneManager.instance.serverScene = "JoinLAN_Scene";
        SceneManager.LoadScene(NameofLevel);

        Debug.Log("Please wait as the scene changes");
    }

    public void LoadCharacterSelectionInternet(string NameofLevel)
    {
        Network_SceneManager.instance.serverScene = "Lobby_Scene";
        SceneManager.LoadScene(NameofLevel);

        Debug.Log("Please wait as the scene changes");
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(Network_SceneManager.instance.serverScene);

        Debug.Log("Please wait as the scene changes");
    }

    //Once a button is pressed the application will close
    public void QuitGame() 
	{
        Application.Quit();
		Debug.Log ("Please, don't leave me");
	}
}
