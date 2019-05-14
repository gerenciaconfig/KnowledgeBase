using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Arcolabs.General;
using Fungus;

namespace Arcolabs.Disney.Patterns
{
	public class TirandoUmSomGameManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject patetaLeft;
		[SerializeField]
		private GameObject patetaRight;

		[SerializeField]
		private Flowchart gameMenuFlowchart;

		// TODO hard coded, needs to be generalized
		private int[] screenOrder = { 0, 1, 2, 3 };

		[Button]
		public void SwitchPateta()
		{
			if (patetaLeft.activeSelf)
			{
				patetaLeft.SetActive(false);
				patetaRight.SetActive(true);
			}
			else
			{
				patetaLeft.SetActive(true);
				patetaRight.SetActive(false);
			}
		}

		private PointAndClickScreenManagement screenManagement;
		private FillSprite fillController;
		private ScreenTransition transition;
		private TracingInformation currentTracingInfo;

		public static void Randomize<T>(T[] items)
		{
			// For each spot in the array, pick
			// a random item to swap into that spot.
			for (int i = 0; i < items.Length - 1; i++)
			{
				int j = Random.Range(i, items.Length-1);
				T temp = items[i];
				items[i] = items[j];
				items[j] = temp;
			}
		}

		private void Awake()
		{
			screenManagement = GetComponent<PointAndClickScreenManagement>();
			fillController = GetComponent<FillSprite>();
			transition = GetComponent<ScreenTransition>();
			screenManagement.ReadyForProduction();
		}

		private void Start()
		{
			StopAllCoroutines();
			Randomize(screenOrder);
			currentScreenIndex = 0;
			screenManagement.ReadyForProduction();
			screenManagement.SetCurrentScreenIndex(screenOrder[currentScreenIndex++]);
			screenManagement.OnCurrentScreenChanged();
			screenManagement.FocusOnCurrentScreen();
			StartCoroutine(Intro());
		}

		private void OnEnable()
		{
			StopAllCoroutines();
			Randomize(screenOrder);
			currentScreenIndex = 0;
			screenManagement.ReadyForProduction();
			screenManagement.SetCurrentScreenIndex(screenOrder[currentScreenIndex++]);
			screenManagement.OnCurrentScreenChanged();
			screenManagement.FocusOnCurrentScreen();
			StartCoroutine(Intro());
		}

		private int currentScreenIndex = 0;

		private IEnumerator Intro()
		{
			yield return new WaitForSeconds(1f);
			transition.BlockInteraction(true);
			currentTracingInfo = screenManagement.GetCurrentScreen().GetComponent<TracingInformation>();
			Debug.Log("Intro calling: " + currentTracingInfo.gameObject.name);
			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			// fillController.UpdateTrailObject(screenManagement.GetCurrentScreen().transform.GetChild(0));
			fillController.UpdateTrailObject(currentTracingInfo.transform.GetChild(0));
			yield return StartCoroutine(transition.FadeIn());
			// play audio cue
			transition.BlockInteraction(false);
			yield return StartCoroutine(PlayAudio(currentTracingInfo.IntroAudioDescription));
		}

		public void QuitGame()
		{
			Debug.Log("Quit!!!");
			StopAllCoroutines();
			StartCoroutine(Quit());
		}

		private IEnumerator Quit()
		{
			Debug.Log("Quitting to main menu!");
			screenManagement.ReadyForProduction();
			transition.BlockInteraction(true);
			// yield return StartCoroutine(screenTransition.FadeOut());
			// ResetGame();
			AudioManager.instance.StopAllSounds();
			yield return null;
		}

		public void FinishedTracing()
		{
			Debug.Log("Finished tracing!");
			StopAllCoroutines();
			if (currentTracingInfo.IsLastTracing)// screenManagement.IsThisTheLastScreen())
			{
				Debug.Log("This is the end...");
				AudioManager.instance.PlaySound("Right Click");
				StartCoroutine(EndingLoop());
			}
			else
			{
				Debug.Log("Getting the next screen...");
				StartCoroutine(UpdateScreens());
			}
		}

		private IEnumerator EndingLoop()
		{
			yield return new WaitForSeconds(4f);

			// yield return StartCoroutine(screenTransition.FadeOut());
			// ResetGame();
			Debug.Log("Going to Start Again!");
			gameMenuFlowchart.ExecuteBlock("Start Play Again");
		}

		private IEnumerator UpdateScreens()
		{
			transition.BlockInteraction(true);
			AudioManager.instance.PlaySound("Right Click");
			AudioManager.instance.PlayRandomSuccessSound();
			yield return new WaitForSeconds(1f);

			yield return StartCoroutine(transition.FadeOut());
			if (currentScreenIndex == screenOrder.Length)
			{
				screenManagement.SetCurrentScreenIndex(screenOrder.Length);
				screenManagement.OnCurrentScreenChanged();
				screenManagement.FocusOnCurrentScreen();
				// this is the last drum looping screen
				currentTracingInfo = screenManagement.GetCurrentScreen().GetComponent<TracingInformation>();
				fillController.UpdateTrailObject(screenManagement.GetCurrentScreen().transform, currentTracingInfo.IsLastTracing);
			}
			else
			{				
				screenManagement.SetCurrentScreenIndex(screenOrder[currentScreenIndex++]);
				screenManagement.OnCurrentScreenChanged();
				screenManagement.FocusOnCurrentScreen();

				currentTracingInfo = screenManagement.GetCurrentScreen().GetComponent<TracingInformation>();
				fillController.UpdateTrailObject(screenManagement.GetCurrentScreen().transform.GetChild(0));
			}

			patetaLeft.SetActive(currentTracingInfo.PatetaOnTheLeft);
			patetaRight.SetActive(!currentTracingInfo.PatetaOnTheLeft);
			
			// get the info of the current screen in order to put the pateta in the right position
			yield return StartCoroutine(transition.FadeIn());
			transition.BlockInteraction(false);
			// Debug.Log("Playing the audio: " + currentTracingInfo.IntroAudioDescription);
			yield return StartCoroutine(PlayAudio(currentTracingInfo.IntroAudioDescription));
		}

		private IEnumerator PlayAudio(string audioDescription)
		{
			Debug.Log("Playing the audio: " + audioDescription);
			AudioManager.instance.StopAllSounds();
			float waitTime = AudioManager.instance.PlayAudioDescriptionWaitTime(audioDescription);
			yield return new WaitForSeconds(waitTime);
		}
	}
}
