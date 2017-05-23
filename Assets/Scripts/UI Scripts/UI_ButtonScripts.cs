using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI_ButtonScripts : MonoBehaviour 
{

	//Once a button is pressed the scene will change to another scene depending on the assigned scene in the string: NameofLevel
	public void ChangeScreen(string NameofLevel)
	{
		SceneManager.LoadScene(NameofLevel);

		Debug.Log ("Please wait as the scene changes");
	}
	
	//Once a button is pressed the application will close
	public void QuitGame() 
	{
        Application.Quit();
		Debug.Log ("Please, don't leave me");
	}
}
