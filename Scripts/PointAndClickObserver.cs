using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arcolabs.Disney.Patterns
{
	public class PointAndClickObserver: MonoBehaviour
	{
		public static event Action<PointAndClickObserver> OnObserverInteraction;

		[SerializeField]
		private string observerName;
		public string ObserverName { get { return observerName; } }

		private bool finished = false;
		public bool Finished { get { return finished; } }

		private Image image;
		public Image IconImage { get { return image; } }

		[SerializeField]
		private ItemProperties properties;

		private Button button;

		private PointAndClickController controller;
		public PointAndClickController Controller { get { return controller; } }

		private bool resetHint = false;

		public void ResetHint()
		{
			resetHint = true;
		}

		private void Awake()
		{
			image = GetComponent<Image>();
			button = GetComponent<Button>();
			// TODO the controller criará a instância desse prefab e passará a si mesmo como parâmetro
			controller = transform.parent.parent.GetComponent<PointAndClickController>();
			if (button)
			{
				// button.onClick.RemoveAllListeners();
				// button.onClick.AddListener(() => { NotifyManager(); });
			}
		}

		Color hintColor = new Color(1f, 1f, 1f, 0f);
		private bool breakAnimation = false;

		private IEnumerator DoHint()
		{
			while (!breakAnimation)
			{
				yield return new WaitForSeconds(10f);
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
				//while (elapsedTime < 2f && !breakAnimation)
				//{
					
				//	elapsedTime += Time.deltaTime;

				//	hintColor.a = (Mathf.Sin(elapsedTime + Mathf.PI * 1.5f) * 1.5f) + .5f;
				//	image.color = hintColor;
				//	Debug.Log("Inside of coroutine, hint value: " + hintColor + " image.color: " + image.color);
				//	yield return new WaitForEndOfFrame();
				//}
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

		public void NotifyManager()
		{
			breakAnimation = true;
			if (OnObserverInteraction != null)
			{
				OnObserverInteraction(this);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (OnObserverInteraction != null)
			{
				OnObserverInteraction(this);
			}
		}

		public void SetObserverName(string obs)
		{
			observerName = obs;
		}

		// TODO mover esse código de OnDisable para outro lugar que faça mais sentido
		private void OnDisable()
		{
			if (observerName == "FoundItem")
			{
				image.color = new Color(1, 1, 1, 0f);
				Debug.Log("Changed the color to default " + image.color);
			}
		}

		// TODO mover esse código de OnDisable para outro lugar que faça mais sentido
		private void OnEnable()
		{
            Debug.Log("Surgiu o item!");
			image.raycastTarget = true;
			if (observerName == "FoundItem")
			{
				StartCoroutine(DoHint());
				image.color = new Color(1, 1, 1, 0f);
				Debug.Log("Changed the color to default " + image.color);
			}
			if (button)
			{
				button.interactable = true;
			}
		}
	}

}
