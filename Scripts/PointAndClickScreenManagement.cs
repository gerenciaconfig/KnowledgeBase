using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PointAndClickScreenManagement : MonoBehaviour
{
	[SerializeField]
	[OnValueChanged("OnCurrentScreenChanged")]
	private int currentScreenIndex = 0;
	private int activeIndex = 0;

	[SerializeField]
	private bool highlightCurrentScreen = false;

	[Button]
	public void ReadyForProduction()
	{
		currentScreenIndex = 0;
		activeIndex = 0;
		FocusOnCurrentScreen();
		GetComponent<CanvasGroup>().alpha = 0;
	}

	[Button]
	public void FocusOnCurrentScreen()
	{
		int c = screens.Length;
		int i = 0;
		while (i < c)
		{
			screens[i].SetActive(i == currentScreenIndex);
			i++;
		}
	}

	[SerializeField]
	private GameObject[] screens;

	[Button]
	private void ResetWithChildrenScreens()
	{
		int c = transform.childCount;
		currentScreenIndex = Mathf.Clamp(currentScreenIndex, 0, c - 1);
		screens = new GameObject[c];
		int i = 0;
		while (i < c)
		{
			screens[i] = transform.GetChild(i).gameObject;
			screens[i].SetActive(i == currentScreenIndex);
			i++;
		}
	}

	/// <summary>
	/// Notas:
	/// Se pressionarmos o botão no Odin, ele não é chamado. Mas se desfizermos e refizermos
	/// as ações do botão do Odin no Editor (Ctrl+z ou Ctrl+Y), ele chama OnValidate
	/// </summary>
	private void OnValidate()
	{
		if (screens.Length == 0)
		{
			currentScreenIndex = 0;
			return;
		}
		
	}

	public void OnCurrentScreenChanged()
	{
		currentScreenIndex = Mathf.Clamp(currentScreenIndex, 0, screens.Length - 1);
		if (activeIndex > screens.Length - 1)
		{
			activeIndex = screens.Length - 1;
		}
		if (currentScreenIndex != activeIndex)
		{
			if (highlightCurrentScreen)
			{
				screens[activeIndex].SetActive(false);
				screens[currentScreenIndex].SetActive(true);
			}
			activeIndex = currentScreenIndex;
			// por enquanto alterando diretamente por aqui, mas talvez no futuro
			// notificar o Manager que troca cenas que esses valores foram alterados

		}
		// just in case we're running a scene at this moment
		StopAllCoroutines();
	}

	public GameObject GetCurrentScreen()
	{
		return screens[activeIndex];
	}

	public int GetCurrentScreenIndex()
	{
		return currentScreenIndex;
	}

	public void SetCurrentScreenIndex(int index) { currentScreenIndex = index; }

	public GameObject GetNextScreen()
	{
		//activeIndex = Mathf.Clamp(activeIndex + 1, 0, screens.Length);
		//currentScreen = activeIndex;
		int nextScreen = Mathf.Clamp(activeIndex + 1, 0, screens.Length);
		return screens[nextScreen];
	}

	public bool IsThisTheLastScreen()
	{
		return currentScreenIndex == screens.Length - 1;
	}

	public void UpdateToNextScreen()
	{
		GetCurrentScreen().SetActive(false);
		GetNextScreen().SetActive(true);
		activeIndex = Mathf.Clamp(activeIndex + 1, 0, screens.Length);
	}

	public void UpdateToFirstScreen()
	{
		GetCurrentScreen().SetActive(false);
		activeIndex = 0;
	}

	public GameObject GetScreenAt(int index)
	{
		if (index >= screens.Length) return null;
		return screens[index];
	}
}
