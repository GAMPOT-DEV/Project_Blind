using UnityEngine;
using System.Collections;

public class UseParticleWindDirection : MonoBehaviour {

	private bool isShuriken = false;
	private ParticleSystem particles;

	void Start()
	{
		particles = gameObject.GetComponent<ParticleSystem>();
		if(particles != null)
			isShuriken = true;
	}

	public void SetDirection()
	{
		if(!isShuriken)
			transform.localScale = new Vector3((transform.localScale.x * -1), transform.localScale.y, transform.localScale.z);
	}


}
