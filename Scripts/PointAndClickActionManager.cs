using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Arcolabs.General;
using Sirenix.OdinInspector;
using Fungus;

namespace Arcolabs.Disney.Patterns
{
	[System.Serializable]
	public class PointAndClickEvent
	{
		[SerializeField]
		private string[] signals = { "Start", "Restart", "Correct Item", "Wrong Item" };
		[SerializeField]
		[ValueDropdown("signals")]
		private string signal;
		[SerializeField]
		private UnityEvent actions;
	}

	public class PointAndClickActionManager : MonoBehaviour
	{
		private bool busy = false;
		// PointAndClickController buttonActionController;
		// talvez o pai desse GameObject possui referência para essa propriedade?
		private ScreenTransition screenTransition;
		private PointAndClickScreenManagement screenManagement;

		// [SerializeField]
		private PointAndClickEvent[] events;

		[SerializeField]
		private Text itemsSelectedReference;
		[SerializeField]
		private Flowchart gameMenuFlowchart;

		private PointAndClickController currentScreenController;

		private void Start()
		{
			PointAndClickObserver.OnObserverInteraction += Observer_OnObserverInteraction;
			// buttonActionController = GetComponent<PointAndClickController>();
			FadeIn();
		}

		private void OnEnable()
		{
			screenManagement = GetComponent<PointAndClickScreenManagement>();
			screenTransition = GetComponent<ScreenTransition>();
		}

		public void FadeIn()
		{
			StartCoroutine(IntroFadeIn());
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
			screenTransition.BlockInteraction(true);
			// yield return StartCoroutine(screenTransition.FadeOut());
			ResetGame();
			AudioManager.instance.StopAllSounds();
			yield return null;
		}

		private void ResetGame()
		{
			itemsSelectedReference.text = "";
			screenManagement.SetCurrentScreenIndex(0);
			screenManagement.FocusOnCurrentScreen();
		}

		private IEnumerator IntroFadeIn()
		{
            Debug.Log("Bloqueando a Interação no começo");
			screenTransition.BlockInteraction(true);
			int index = (screenManagement.GetCurrentScreenIndex());
            currentScreenController = screenManagement.GetScreenAt(index).GetComponent<PointAndClickController>();
            itemsSelectedReference.transform.parent.gameObject.SetActive(currentScreenController.Type == PointAndClickType.FindItem);
			yield return StartCoroutine(screenTransition.FadeIn());
			Debug.Log("Fade in finished and we're starting the game");
			string prefix = (currentScreenController.Type == PointAndClickType.FindItem) ? "Pergunta " : "";
			yield return StartCoroutine(PlayAudio(prefix + currentScreenController.AudioIdentifier));
			screenTransition.BlockInteraction(false);
            Debug.Log("Desbloqueando a Interação no começo");
        }

		/// <summary>
		/// On click we
		/// * play the sfx (one controller knows how to do this)
		/// * show the circle (another knows how to do this)
		/// * animate (another knows how to do this)
		/// at the same time (one controller), after that we
		/// * play the audio cue
		/// and then we
		/// * finish the instructions
		/// * give the control back to other types of input
		/// </summary>
		/// <param name="e"></param>
		private void Observer_OnObserverInteraction(PointAndClickObserver e)
		{
			// TODO remover essa chamada de método e entender porque
			// o fade in fica preso num loop infinito
			StopAllCoroutines();
			// buttonActionController.observer = e;
			// faz verificações para garantir se quer utilizar apenas uma vez
			// TODO change this to an enum
			
			if (e.ObserverName == "Start")
			{
				// transitions to the next screen
				Debug.Log("Starting!");
				if (!screenManagement.IsThisTheLastScreen())
				{
					// StopAllCoroutines();
					StartCoroutine(TransitionScenes(screenManagement.GetCurrentScreenIndex(), screenManagement.GetCurrentScreenIndex() + 1));
				}
				else
				{
					StartCoroutine(TransitionToPlayAgain());
					Debug.Log("Game over! We're fading out to Play again. Babai!");
				}
			}
			else if (e.ObserverName == "FoundItem")
			{
				e.Controller.AddVictory();
				itemsSelectedReference.text = e.Controller.CurrentVictories.ToString();
				e.Controller.RequestResetHints();
				StartCoroutine(FoundItem(e.GetComponent<Image>(), e.Controller.Finished));
			}
			else if (e.ObserverName == "CorrectItem")
			{
				// StopAllCoroutines();
				StartCoroutine(CorrectItem(e.gameObject));
				// bloquear interação
				// play sound effect
				// tocar narração
				// transição de cena
			}
			else if (e.ObserverName == "WrongItem")
			{
				// StopAllCoroutines();
				StartCoroutine(WrongItem(e.gameObject));
				// play wrong sound effect
				// tocar narração
			}
			else if (e.ObserverName == "Restart")
			{
				// StopAllCoroutines();
				StartCoroutine(RestartGame());
				// mover para a primeira cena
				// transicionar entre elas
			}
			else if (e.ObserverName == "Continue")
			{
				StartCoroutine(ContinueGame());
			}
			Debug.Log("HA! Got it! " + e.ObserverName);
		}

		private IEnumerator TransitionToPlayAgain()
		{
			yield return StartCoroutine(screenTransition.FadeOut());
			ResetGame();
			gameMenuFlowchart.ExecuteBlock("Start Play Again");
			// precisa chamar StopAllCoroutines?
		}

		private IEnumerator ContinueGame()
		{
			if (screenManagement.IsThisTheLastScreen())
			{
				yield return StartCoroutine(TransitionToPlayAgain());
				Debug.Log("Game over! We're fading out to Play again. Babai!");
			}
			else
			{
				yield return StartCoroutine(TransitionScenes(screenManagement.GetCurrentScreenIndex(), screenManagement.GetCurrentScreenIndex() + 1));
				string prefix = (currentScreenController.Type == PointAndClickType.None) ? "" : "Pergunta ";
				StartCoroutine(PlayAudio(prefix + currentScreenController.AudioIdentifier));
			}
		}

		// TODO migrar isso para um controlador.
		// A não ser que seja interessante colocar esse tipo de lógica mais alto nível aqui
		// (até para evitar mais chamadas de GetComponent().
		private IEnumerator TransitionScenes(int fromIndex, int toIndex)
		{
			GameObject from = screenManagement.GetScreenAt(fromIndex);
			GameObject to = screenManagement.GetScreenAt(toIndex);
			Debug.Log("Starting transition from " + from + " to " + to);
			screenTransition.BlockInteraction(true);
			yield return StartCoroutine(screenTransition.FadeOut());
			currentScreenController = to.GetComponent<PointAndClickController>();
			// switching scenes
			//screenManagement.UpdateToNextScreen();
			// === TODO mover esse texto para outro lugar caso não seja aqui o seu local ideal
			itemsSelectedReference.text = "";
			itemsSelectedReference.transform.parent.gameObject.SetActive(currentScreenController.Type == PointAndClickType.FindItem);
			// end todo ===
			screenManagement.SetCurrentScreenIndex(toIndex);
			from.SetActive(false);
			to.SetActive(true);
			// showing new scene
			yield return StartCoroutine(screenTransition.FadeIn());
			screenTransition.BlockInteraction(false);
			// yield ;
		}

		Color whiteNoAlpha = new Color(1f, 1f, 1f, 0f);

		// TODO transformar isso em duas funções, uma para a exibição dos itens e outra para 
		// a transição de cenas
		private IEnumerator FoundItem(Image item, bool finished)
		{
			Debug.Log("Correct!");
			screenTransition.BlockInteraction(true);
			float elapsedTime = 0f;
			// yield return StartCoroutine(PlayAudio("Right Click"));
			string id = currentScreenController.AudioIdentifier;

			if (!finished && currentScreenController.CurrentVictories < 3 && 
                (id == "Garrafa" || id == "Espada" || id == "Pedra" || id == "Pedrinha" || id == "Flor" || id == "Moeda"))
			{
				Debug.Log("Uma ou Duas!!!");
				if (currentScreenController.CurrentVictories == 1)
				{
					AudioManager.instance.PlaySound("Contar Uma");
				}
				else if (currentScreenController.CurrentVictories == 2)
				{
					AudioManager.instance.PlaySound("Contar Duas");
				}
			}
			else if (finished)
			{
				AudioManager.instance.PlaySound("Contar " + currentScreenController.CurrentVictories + " Final");
			}
			else
			{
				Debug.Log("Normal!!!");
				AudioManager.instance.PlaySound("Contar " + currentScreenController.CurrentVictories);
			}

			while (item.color != Color.white)
			{
				elapsedTime += Time.deltaTime;
				item.color = Color.Lerp(whiteNoAlpha, Color.white, elapsedTime * 2.15f);
				yield return new WaitForEndOfFrame();
			}
			// iTween.PunchScale(item.gameObject, Vector3.one * .4f, 1.25f);
			// yield return new WaitForSeconds(1.5f);
			
			if (finished) // TODO organizar essa lista de ações para que fiquem mais generalizadas
			{
				yield return new WaitForSeconds(1f);
				int index = screenManagement.GetCurrentScreenIndex() + 1;
				AudioManager.instance.PlayRandomSuccessSound();
				yield return new WaitForSeconds(1f);
				// yield return StartCoroutine(PlayAudio("Resposta " + currentScreenController.AudioIdentifier));

				if (screenManagement.IsThisTheLastScreen())
				{
					yield return StartCoroutine(TransitionToPlayAgain());
					Debug.Log("Game over! We're fading out to Play again. Babai!");
				}
				else
				{
					yield return StartCoroutine(TransitionScenes(screenManagement.GetCurrentScreenIndex(), screenManagement.GetCurrentScreenIndex() + 1));

					string prefix = (currentScreenController.Type == PointAndClickType.None) ? "" : "Pergunta ";
					yield return StartCoroutine(PlayAudio(prefix + currentScreenController.AudioIdentifier));

				}
			}
			// item.color = new Color(1f, 1f, 1f, 0f);
			
			screenTransition.BlockInteraction(false);
		}

		private IEnumerator PlayAudio(string audioDescription)
		{
			Debug.Log("Playing the audio: " + audioDescription);
			AudioManager.instance.StopAllSounds();
			float waitTime = AudioManager.instance.PlayAudioDescriptionWaitTime(audioDescription);
			yield return new WaitForSeconds(waitTime);
		}

		private IEnumerator CorrectItem(GameObject go)
		{
			screenTransition.BlockInteraction(true);
			iTween.PunchScale(go, Vector3.one * .4f, 1f);
			AudioManager.instance.PlaySound("Right Click");
			yield return new WaitForSeconds(1f);
			yield return StartCoroutine(PlayAudio("Resposta " + currentScreenController.AudioIdentifier));
			if (screenManagement.IsThisTheLastScreen())
			{
				yield return StartCoroutine(TransitionToPlayAgain());
				Debug.Log("Game over! We're fading out to Play again. Babai!");
			}
			else
			{
				yield return StartCoroutine(TransitionScenes(screenManagement.GetCurrentScreenIndex(), screenManagement.GetCurrentScreenIndex() + 1));
				screenTransition.BlockInteraction(false);
				string prefix = (currentScreenController.Type == PointAndClickType.None) ? "" : "Pergunta ";
				StartCoroutine(PlayAudio(prefix + currentScreenController.AudioIdentifier));
			}
		}

		// string[] encouraginPhrases = { "Só mais um pouquinho", "Não desista!", "Quase lá!", "O céu está estrelado!" };

		private IEnumerator WrongItem(GameObject go)
		{
			screenTransition.BlockInteraction(true);
			// Debug.Log(encouraginPhrases[Random.Range(0, encouraginPhrases.Length - 1)]);
			Debug.Log("You are so dumb! You are really dumb! Fo' real!");
			iTween.ShakePosition(go, Vector3.one * .25f, .42f);
			AudioManager.instance.PlayRandomFailSound();
			yield return new WaitForSeconds(1.5f);
			screenTransition.BlockInteraction(false);
		}

		private IEnumerator RestartGame()
		{
			yield return StartCoroutine(TransitionScenes(screenManagement.GetCurrentScreenIndex(), 1));
		}
	}
}
