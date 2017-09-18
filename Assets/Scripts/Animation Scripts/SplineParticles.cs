using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineParticles : MonoBehaviour {

	ParticleSystem partSys;

//	void LateUpdate()
//	{
//		InitializeIfNeeded ();
//		int numParticlesAlive = partSys.GetParticles (m_Particles);
//		for (int i = 0; i < numParticlesAlive; ++i) 
//		{
//			float lerp = m_Particles [i].lifetime / m_Particles [i].startLifetime;
//			Vector3 newPosition = GetLinePosition(lerp);
//			m_Particles[i].position = newPosition;
//		}
//		m_system.SetParticles (m_Particles, numParticlesAlive);
//	}
//
//	Vector3 GetLinePosition(float lerp)
//	{
//		return Vector3.Lerp( spline.GetPoint (3), spline.GetPoint (0) ,lerp);
//	}
}
