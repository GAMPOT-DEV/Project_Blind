using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class AdjustFrameRate : MonoBehaviour {

	private Animator animator;
	public float speed;

	void Start()
	{
		animator = GetComponent<Animator>();
		OnEnable();
	}

	void OnEnable()
	{
		if(animator != null)
			animator.speed = speed;
	}
}
