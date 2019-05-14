using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomStars : MonoBehaviour 
{
	
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();	
	}

	public void OnEnable()
	{
		anim.ForceStateNormalizedTime(UnityEngine.Random.Range(0.0f, 1.0f));
	} 
}