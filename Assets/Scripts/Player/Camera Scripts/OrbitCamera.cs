using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour 
{
	public GameObject target = null;
	public bool orbitY;
	public static bool orbitScene;
	
	// Update is called once per frame

	void Awake () 
	{
		orbitY = true;
		orbitScene = true;
	}

	void Update () 
	{
        if (orbitScene) 
		{
			if (target != null) 
			{
				transform.LookAt(target.transform);
				
				if (orbitY)
				{
					transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * 8);
				}
			}
			
		}
	}
}

