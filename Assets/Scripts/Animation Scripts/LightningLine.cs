using  System.Collections;
using  System.Collections.Generic;
using  UnityEngine;
using  UnityEditor;

public class  Lightingline:  MonoBehaviour  {

	public  Vector3  target;
	public  LineRenderer LR;
	public float  arcLength =  2.0f;
	public float  arcVariation =  2.0f;
	public float  inaccuracy =  1.0f;

	void  Update()  {
		Vector3  lastPoint =  Vector3.zero;
		int  i =  1;
		LR.SetPosition(0, Vector3.zero); //make the origin of the LR the same as the transform
		while  (Vector3.Distance(target,  lastPoint)  >.5)  { //was the Last arc not touching the target?
			LR.positionCount  = i +  1; //then we need a new vertex in our Line renderer
			Vector3  fwd = target - lastPoint; //gives the direction to our target from the end of the Last arc
			fwd.Normalize(); //makes the direction to scaLe
			fwd = Randomize(fwd,  inaccuracy);//we don't want a straight Line to the target though
			fwd *= Random.Range(arcLength * arcVariation, arcLength); //nature is never too uniform
			fwd += lastPoint; //point 4- distance * direction = new point, this is where our new arc ends
			LR.SetPosition(i,  fwd); //this teLLs the Line renderer where to draw to
			i++;
			lastPoint =  fwd; //so we know where we are starting from for the next arc
		}
	}

	void  OnValidate()  {
		LR.SetPosition(1, target);
	}

	Vector3  Randomize  (Vector3 v3,  float  inaccuracy2)  {
		v3  +=  new  Vector3(Random.Range(-1.0f,  1.0f),  Random.Range(-1.0f,  1.0f),  Random.Range(-1.0f, 1.0f))  *  inaccuracy2;
		v3.Normalize();
		return  v3;
	}
}