using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerNameplate : MonoBehaviour {

    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private RectTransform healthBarFill;

	[SerializeField]
	private RectTransform healthBarFill2;

    [SerializeField]
    private Network_PlayerManager player;
	
	// Update is called once per frame
	void Update () {

        usernameText.text = player.username;
		healthBarFill.sizeDelta = new Vector2 ((player.GetHealthPct() * 600),200);
		healthBarFill2.sizeDelta = new Vector2 ((player.GetHealthPct() * 600),200);

    }
}
