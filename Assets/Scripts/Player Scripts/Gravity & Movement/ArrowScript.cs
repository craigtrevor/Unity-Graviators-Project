using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    //Change colour of arrow
    public void ChangeColour(Material material) {
        //Component[] renderers = GetComponentsInChildren<Renderer>(); //Get components from children

        GetComponent<Renderer>().material = material;
        //foreach (Renderer renderer in renderers) { //Change colour in all children
        //    renderer.material = material;
        //}
    }
    
}
