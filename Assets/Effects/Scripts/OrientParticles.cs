using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrientParticles : MonoBehaviour
{

	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	public float angleOffset = 0.0f;
	public float x;
	public float y;
	public float z;
	public float Ox;
	public float Oy;
	public float Oz;

	private void LateUpdate()
	{
		InitializeIfNeeded();

		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		// Change only the particles that are alive
		for (int i = 0; i < numParticlesAlive; i++)
		{
			// use atan2 to calc angle based on position, then convert to degrees.
			float angle = Mathf.Atan2(m_Particles[i].position.x, m_Particles[i].position.y) * Mathf.Rad2Deg;
			// add the offset (in case the artwork is rotated, etc.)
			//m_Particles[i].rotation = angle + angleOffset;
			x = Mathf.Atan2(m_Particles[i].position.z, m_Particles[i].position.y) * Mathf.Rad2Deg - Ox;
			y = Mathf.Atan2 (m_Particles [i].position.z, m_Particles [i].position.x) * Mathf.Rad2Deg - Oy;
			z = Mathf.Atan2 (m_Particles [i].position.y, m_Particles [i].position.x) * Mathf.Rad2Deg - Oz;
			x = Ox;
			y = Oy;
			//z = 0;
			/*if (rotX < 0)
				rotX += 360;
			if (rotY < 0)
				rotY += 360;
			if (rotZ < 0)
				rotZ += 360;*/
			m_Particles[i].rotation3D = new Vector3(x, y, z);
		}

		// Apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);
	}

	void InitializeIfNeeded()
	{
		if (m_System == null)
			m_System = GetComponent<ParticleSystem>();

		if (m_Particles == null || m_Particles.Length < m_System.maxParticles)
			m_Particles = new ParticleSystem.Particle[m_System.maxParticles]; 
	}

}