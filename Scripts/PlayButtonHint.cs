using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonHint : MonoBehaviour 
{
	public float timeToShowHint;
	public float timer;

	public bool countHintTime = false;
	
	void Update ()
	{
		if(countHintTime)
		{
			HintTimerCounter();
		}
	}

	public void HintTimerCounter()
    {
        timer += Time.deltaTime;


        if (timer >= timeToShowHint)
        {
            Animator anim = this.gameObject.GetComponent<Animator>();
        	if (anim != null)
        	{
           		anim.SetTrigger("shine");
        	}

            ResetHintTimer();
        }

    }

	public void ResetHintTimer()
    {
        timer = 0;
    }

	public void FirstHint()
	{
		Animator anim = this.gameObject.GetComponent<Animator>();
       
	    if (anim != null)
        {
           	anim.SetTrigger("shine");
        }

		countHintTime = true;
	}

	public void ResetHint()
	{
		timer = 0;
		countHintTime = false;
	}

    public void EnableCount()
    {
        countHintTime = true;
    }
}