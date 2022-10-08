using UnityEngine;
using System.Collections;

public class OffsetAnimation : MonoBehaviour {

	public Animator animator;
	public string animationName;
	public float offsetAmount;
	public bool random = false;

	void OnEnable()
	{
		if(!random)
			animator.Play(animationName, 0, offsetAmount);
		else
			animator.Play(animationName, 0, Random.Range(0, 1f));
	}

}
