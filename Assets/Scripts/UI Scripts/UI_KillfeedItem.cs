using UnityEngine;
using UnityEngine.UI;

public class UI_KillfeedItem : MonoBehaviour {

    [SerializeField]
    Text text;

    public void Setup (string player, string source)
    {
        text.text = "<b>" + source + "</b>" + " killed " + "<i>" + player + "</i>";
    }
}
