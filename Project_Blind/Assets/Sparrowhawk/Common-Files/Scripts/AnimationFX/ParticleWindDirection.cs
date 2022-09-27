using UnityEngine;
using System.Collections;

public class ParticleWindDirection : MonoBehaviour {

	public bool right = false;
	private bool lastDirectionCheck = false;
	private int direction = 1;

	private static ParticleWindDirection m_instance;

	public static ParticleWindDirection instance
	{
		get { return m_instance; }
	}

	void Awake()
	{
		m_instance = this;
	}

	void Update()
	{
		if(right != lastDirectionCheck)
			CheckDirection();
	}

	private void CheckDirection()
	{
		lastDirectionCheck = right;
		if(right)
			SetDirection(1);
		else
			SetDirection(-1);
	}
	
	public void SetDirection(int newDirection)
	{
		direction = newDirection;
		DirectionChanged();
	}

	public int GetDirection()
	{
		return direction;
	}

	private void DirectionChanged()
	{
		UseParticleWindDirection[] particles = GameObject.FindObjectsOfType<UseParticleWindDirection>();
		foreach(UseParticleWindDirection particle in particles)
			particle.SetDirection();
	}
}
