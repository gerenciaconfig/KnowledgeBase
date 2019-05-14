using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintHighlight : MonoBehaviour
{
	Color hintColor = new Color(1f, 1f, 1f, 0f);
	private bool breakAnimation = false;

	private bool resetHint = false;

	private Image image;
	public Image IconImage { get { return image; } }

	[SerializeField]
	private float waitTime = 10f;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void ResetHint()
	{
		resetHint = true;
		breakAnimation = false;
	}

	public void StopHint()
	{
		breakAnimation = true;
	}

	public void PlayHint()
	{
		StopAllCoroutines();
		StartCoroutine(DoHint());
	}

	private IEnumerator DoHint()
	{
		while (!breakAnimation)
		{
			yield return new WaitForSeconds(waitTime);
			// float elapsedTime = 0f;
			for (int i = 0; i < 2f; i++)
			{
				Debug.Log("Hinting!");
				if (resetHint)
				{
					resetHint = false;
					break;
				}
				yield return StartCoroutine(Flick());
			}
		}
	}

	private IEnumerator Flick()
	{
		float elapsedTime = 0;
		hintColor.a = .01f;
		while (hintColor.a > 0 && !breakAnimation)
		{
			elapsedTime += Time.deltaTime;

			hintColor.a = (Mathf.Sin(elapsedTime * 4f + Mathf.PI * 1.7f) * .25f) + .24f;
			image.color = hintColor;
			Debug.Log("Inside of coroutine, hint value: " + hintColor + " image.color: " + image.color);
			yield return new WaitForEndOfFrame();
		}
	}
}
