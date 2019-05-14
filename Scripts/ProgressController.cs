using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressController : MonoBehaviour
{
	public UnityEvent progress;
	public UnityEvent progressFail;
	public UnityEvent success;
	public UnityEvent fail;
    public UnityEvent onRightAnswer;
    public UnityEvent onWrongAnswer;
    public UnityEvent onEnable;
	public int progressAmount;

	public int progressCounter;

	public void Progress()
	{
		ProgressValidator validator = GetComponent<ProgressValidator>();
		if (validator == null || validator.Progressed())
		{
			progressCounter++;
		}
		else
		{
			ProgressFail();
			return;
		}

		if (progressCounter >= progressAmount)
		{
			if (validator == null || validator.Succeeded())
			{ 
				Success();
			}
			else
			{
				Fail();
			}
			return;
		}
		progress.Invoke();
	}

	public void ClearProgress()
	{
		progressCounter = 0;
	}


	public void ProgressFail()
	{
		progressFail.Invoke();
	}

	public void Success()
	{
		success.Invoke();
	}

	public void Fail()
	{
		fail.Invoke();
	}

	public void OnEnable()
	{
		onEnable.Invoke();
	}

    public void InvokeRightAnswer()
    {
        onRightAnswer.Invoke();
    }

    public void InvokeWrongAnswer()
    {
        onWrongAnswer.Invoke();
    }
}
