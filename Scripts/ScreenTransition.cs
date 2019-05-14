using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Arcolabs.General
{
	/// <summary>
	/// Finishes the previous screen and fades to the new one
	/// </summary>
	public class ScreenTransition : MonoBehaviour
	{
		[SerializeField]
		private bool lockFade = true;
		[SerializeField]
		[Range(0, 8f)]
		private float fadeInDuration = 2f;
		[SerializeField]
		[Range(0, 8f)]
		private float fadeOutDuration = 2f;
		private CanvasGroup canvasGroup;

		private float lockFadeValue = 2f;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void OnValidate()
		{
			if (lockFade)
			{
				if (fadeInDuration != lockFadeValue)
				{
					lockFadeValue = fadeInDuration;
					fadeOutDuration = fadeInDuration;
				}
				else if (fadeOutDuration != lockFadeValue)
				{
					lockFadeValue = fadeOutDuration;
					fadeInDuration = fadeOutDuration;
				}
			}
		}

		public void BlockInteraction(bool block)
		{
			canvasGroup.blocksRaycasts = !block;
		}

		public IEnumerator Transition(GameObject from, GameObject to)
		{
			Debug.Log("Starting transition!");
			canvasGroup.blocksRaycasts = false;
			yield return StartCoroutine(FadeOut());
			from.SetActive(false);
			to.SetActive(true);
			yield return StartCoroutine(FadeIn());
			canvasGroup.blocksRaycasts = true;
		}

		public IEnumerator FadeOut()
		{
			Debug.Log("Fading out!");
			float elapsedTime = 0f;

			canvasGroup.alpha = 1f;

			while (canvasGroup.alpha > 0f)
			{
				elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime / fadeInDuration, 0f, 1f);
				canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime);
				yield return new WaitForEndOfFrame();
			}
			canvasGroup.alpha = 0f;
		}

		public IEnumerator FadeIn()
		{
			Debug.Log("Fading in!");
			float elapsedTime = 0f;

			canvasGroup.alpha = 0f;

			while (canvasGroup.alpha < 1f)
			{
				elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime / fadeOutDuration, 0f, 1f);
				canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime);
				// Debug.Log("Fading in - alpha: " + canvasGroup.alpha);
				yield return new WaitForEndOfFrame(); //WaitUntil (() => { return canvasGroup.alpha < 1f; });
			}
			canvasGroup.alpha = 1f;
			Debug.Log("Finished Fading in!");
			yield break;
		}
	}
}
