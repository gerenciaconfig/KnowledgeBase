using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimer : MonoBehaviour 
{	
	public string audioName;
	public bool startCount;

	public float timer;
	public float maxTime;

	public AudioManager am;


	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(startCount)
		{
			timer += Time.deltaTime;
		}

		if(timer >= maxTime)
		{
			startCount = false;
			TimerComplete();
		}
	}

	void StartTimer()
	{
		timer = 0;
		startCount = true;
	}
	void TimerComplete()
	{
		am.PlaySound(audioName);
		ResetTimer();
	}


	void ResetTimer()
	{
		timer = 0;
		startCount = true;
	}
}