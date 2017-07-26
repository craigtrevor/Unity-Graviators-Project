using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_UserAccountLobby : MonoBehaviour {

    public Text usernameText;
    public Text characterText;

    [SerializeField]
    Network_Manager networkManager;


    void Start()
    {
        if (UI_UserAccountManager.IsLoggedIn)
            usernameText.text = UI_UserAccountManager.LoggedIn_Username;

        networkManager = GameObject.Find("CustomNetworkManager").GetComponent<Network_Manager>();

        characterText.text = networkManager.characterName;

    }

    public void SwitchCharacter()
    {
        SceneManager.LoadScene("Character_Select");          
    }

    public void LogOut()
    {
        if (UI_UserAccountManager.IsLoggedIn)
            UI_UserAccountManager.instance.LogOut();
    }
}
