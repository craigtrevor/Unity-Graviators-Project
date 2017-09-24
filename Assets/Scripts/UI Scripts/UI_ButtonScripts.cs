using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UI_ButtonScripts : MonoBehaviour 
{
    static string lastScene;
    static string currentScene;

    //Once a button is pressed the scene will change to another scene depending on the assigned scene in the string: NameofLevel
    public void ChangeScreen(string NameofLevel)
    {
        StartCoroutine(DelaySceneLoad(NameofLevel));
    }

    public void ShutdownMatch(string NameofLevel)
    {
        StartCoroutine(DelaySceneLoad(NameofLevel));
        NetworkManager.Shutdown();

        Debug.Log("Please wait as the scene changes");
    }


    public void LoadCharacterSelectionLAN(string NameofLevel)
    {
        Network_SceneManager.instance.serverScene = "JoinLAN_Scene";
        StartCoroutine(DelaySceneLoad(NameofLevel));
    }

    public void LoadCharacterSelectionInternet(string NameofLevel)
    {
        Network_SceneManager.instance.serverScene = "Lobby_Scene";
        StartCoroutine(DelaySceneLoad(NameofLevel));
    }

    public void LoadCharacterSelectionPractice(string NameofLevel)
    {
        Network_SceneManager.instance.serverScene = "JoinPracticeRange_Scene";
        StartCoroutine(DelaySceneLoad(NameofLevel));
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(Network_SceneManager.instance.serverScene);
        StartCoroutine(DelaySceneLoad(null));
    }

    //Once a button is pressed the application will close
    public void QuitGame() 
	{
        Application.Quit();
		Debug.Log ("Please, don't leave me");
	}

    IEnumerator DelaySceneLoad(string NameofLevel)
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Please wait as the scene changes");
        SceneManager.LoadScene(NameofLevel);
    }
}
