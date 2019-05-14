using System.Collections;
using UnityEngine;
using Fungus;
using Arcolabs.General;

namespace Arcolabs.Disney.Patterns
{
	public class PatetaTirandoUmSom : MonoBehaviour
	{
		[SerializeField]
		private GameObject patetaLeft;
		[SerializeField]
		private GameObject patetaRight;

		[SerializeField]
		private ArcoTracingCollider lastScene;

		RandomChildren scenes;
		private int currentIndex = 0;

		private ScreenTransition transition;
		private TracingInformation currentTracingInfo;

		[SerializeField]
		private Flowchart gameMenuFlowchart;

		private bool calledPlayAgainBlock = false;

		private void Awake()
		{
			scenes = GetComponent<RandomChildren>();
			transition = GetComponent<ScreenTransition>();
			calledPlayAgainBlock = false;
		}

		private void OnEnable()
		{
			currentIndex = 0;
			calledPlayAgainBlock = false;
			StartCoroutine(Intro());
		}

		public void ResetGame()
		{
			currentIndex = 0;
			calledPlayAgainBlock = false;
			lastScene.ResetEverything();
			lastScene.transform.parent.gameObject.SetActive(false);
		}

		private IEnumerator Intro()
		{
			yield return new WaitForSeconds(.125f);
			currentTracingInfo = scenes.GetChildrenIndex(currentIndex).
				transform.parent.GetComponent<TracingInformation>();
			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			yield return StartCoroutine(
				PlayAudio(currentTracingInfo.IntroAudioDescription));
		}

		void Update()
		{
			if (currentIndex < scenes.Length && 
				scenes.GetChildrenIndex(currentIndex).Finished)
			{
				currentIndex++;
				if (currentIndex < scenes.Length)
				{
					StopAllCoroutines();
					// StartCoroutine(NextNormalScene());
					StartCoroutine(TransitionScenes(
						scenes.GetChildrenIndex(currentIndex - 1),
						scenes.GetChildrenIndex(currentIndex)));
				}
				else
				{
					// StartCoroutine(NextFinalScene());
					StartCoroutine(TransitionScenes(
						scenes.GetChildrenIndex(currentIndex - 1), lastScene));
				}
			}
			else if (lastScene.Finished && !calledPlayAgainBlock)
			{
				calledPlayAgainBlock = true;
				Debug.Log("Going to start again!");
				gameMenuFlowchart.ExecuteBlock("Start Play Again");
			}
		}

		private IEnumerator TransitionScenes(ArcoTracingCollider from, ArcoTracingCollider to)
		{
			// Block Screen
			transition.BlockInteraction(true);
			// Play Audio
			AudioManager.instance.PlaySound("Right Click");
			AudioManager.instance.PlayRandomSuccessSound();
			yield return new WaitForSeconds(1f);
			// Fade Out
			yield return StartCoroutine(transition.FadeOut());
			// Change Items
			from.ResetEverything();
			from.transform.parent.gameObject.SetActive(false);
			to.ResetEverything();
			to.transform.parent.gameObject.SetActive(true);
			currentTracingInfo = to.transform.parent.GetComponent<TracingInformation>();
			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			// Fade In
			yield return StartCoroutine(transition.FadeIn());
			// retrieve scene
			transition.BlockInteraction(false);
			yield return StartCoroutine(PlayAudio(
				currentTracingInfo.IntroAudioDescription));
		}

		private IEnumerator NextNormalScene()
		{
			// Block Screen
			transition.BlockInteraction(true);
			// Play Audio
			AudioManager.instance.PlaySound("Right Click");
			AudioManager.instance.PlayRandomSuccessSound();
			yield return new WaitForSeconds(1f);
			// Fade Out
			yield return StartCoroutine(transition.FadeOut());
			// Change Items
			scenes.GetChildrenIndex(currentIndex - 1).ResetEverything();
			scenes.GetChildrenIndex(currentIndex - 1).transform.parent.
				gameObject.SetActive(false);
			scenes.GetChildrenIndex(currentIndex).ResetEverything();
			scenes.GetChildrenIndex(currentIndex).transform.parent.
				gameObject.SetActive(true);
			currentTracingInfo = scenes.GetChildrenIndex(currentIndex).
				transform.parent.GetComponent<TracingInformation>();
			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			// Fade In
			yield return StartCoroutine(transition.FadeIn());
			// retrieve scene
			transition.BlockInteraction(false);
			yield return StartCoroutine(PlayAudio(
				currentTracingInfo.IntroAudioDescription));
		}

		private IEnumerator NextFinalScene()
		{
			// Block Screen
			transition.BlockInteraction(true);
			// Play Audio
			AudioManager.instance.PlaySound("Right Click");
			AudioManager.instance.PlayRandomSuccessSound();
			yield return new WaitForSeconds(1f);
			// Fade Out
			yield return StartCoroutine(transition.FadeOut());
			// Change Items
			scenes.GetChildrenIndex(currentIndex - 1).ResetEverything();
			scenes.GetChildrenIndex(currentIndex - 1).transform.parent.
				gameObject.SetActive(false);
			lastScene.ResetEverything();
			lastScene.transform.parent.gameObject.SetActive(true);
			currentTracingInfo = lastScene.transform.parent.
				GetComponent<TracingInformation>();
			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			// Fade In
			yield return StartCoroutine(transition.FadeIn());
			// retrieve scene
			transition.BlockInteraction(false);
			yield return StartCoroutine(PlayAudio(
				currentTracingInfo.IntroAudioDescription));
		}

		private IEnumerator PlayAudio(string audioDescription)
		{
			Debug.Log("Playing the audio: " + audioDescription);
			AudioManager.instance.StopAllSounds();
			float waitTime = AudioManager.instance.
				PlayAudioDescriptionWaitTime(audioDescription);
			yield return new WaitForSeconds(waitTime);
		}
	}
}