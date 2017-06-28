using UnityEngine;
using UnityEngine.UI;

public class UI_UserAccountLobby : MonoBehaviour {

    public Text usernameText;

    void Start()
    {
        if (UI_UserAccountManager.IsLoggedIn)
            usernameText.text = UI_UserAccountManager.LoggedIn_Username;
    }

    public void LogOut()
    {
        if (UI_UserAccountManager.IsLoggedIn)
            UI_UserAccountManager.instance.LogOut();
    }
}
