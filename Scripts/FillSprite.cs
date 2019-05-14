using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney.Patterns
{
	public class FillSprite : MonoBehaviour
	{
		[SerializeField]
		private Image referencePointerImage;
		private RectTransform referencePointer;
		[SerializeField]
		private Transform[] trailObjects;
		private int currentTrailIndex = 0;
		[SerializeField]
		private Transform currentTrailObject;
		
		Vector2 touchPoint;
		Vector2 screenPoint;
		RectTransform rect;
		Image image;

		[SerializeField]
		[ReadOnly]
		private GameObject[] maskObjects;
		private int currentMaskIndex;

		TirandoUmSomGameManager gameManager;

		public void NextTrailObject()
		{
			ResetTracing();
			trailObjects[currentTrailIndex].parent.gameObject.SetActive(false);
			currentTrailIndex = (currentTrailIndex + 1) % trailObjects.Length;
			currentTrailObject = trailObjects[currentTrailIndex];
			currentTrailObject.parent.gameObject.SetActive(true);
			GetObjects();
			ResetTracing();
		}

		public void UpdateTrailObject(Transform next, bool isLast = false)
		{
			ResetTracing();
			if (currentTrailObject.GetComponent<TracingInformation>()) // then this is the last one (needs to improve this code)
			{
				currentTrailObject.gameObject.SetActive(false);
			}
			else
			{
				currentTrailObject.parent.gameObject.SetActive(false);
			}
			currentTrailObject = next;
			if (isLast)
			{
				currentTrailObject.gameObject.SetActive(true);
			}
			else
			{
				currentTrailObject.parent.gameObject.SetActive(true);
			}
			GetObjects(isLast);
			ResetTracing();
		}

		public void UpdateReferencePointerColor()
		{
			// use a method to check for position within the rect transform
			referencePointerImage.color = image.color;
		}

		[Button]
		public void GetObjects(bool lastObject = false)
		{
			if (!currentTrailObject)
			{
				Debug.LogError("Sem objeto pai das máscaras");
				return;
			}
			currentMaskIndex = 0;
			int count = 0;
			if (lastObject)
			{
				List<GameObject> children = new List<GameObject>();
				for (int i = 0; i < currentTrailObject.childCount; i++)
				{
					int len = currentTrailObject.GetChild(i).GetChild(0).childCount;
					count += len;
					for (int k = 0; k < len; k++)
					{
						children.Add(currentTrailObject.GetChild(i).GetChild(0).GetChild(k).gameObject);
					}
				}
				maskObjects = new GameObject[count];
				for (int i = 0; i < count; i++)
				{
					maskObjects[i] = children[i];
					maskObjects[i].SetActive(false);
				}
			}
			else
			{
				count = currentTrailObject.childCount;
				maskObjects = new GameObject[count];
				for (int i = 0; i < count; i++)
				{
					maskObjects[i] = currentTrailObject.GetChild(i).gameObject;
					maskObjects[i].SetActive(false);
				}
			}
			maskObjects[0].SetActive(true);
			referencePointer.position = maskObjects[0].GetComponent<RectTransform>().position;
			referencePointerImage.color = maskObjects[0].GetComponent<Image>().color;
			// Vector3 referenceWorldPoint = new Vector3(maskObjects[0].GetComponent<RectTransform>().position.x, maskObjects[0].GetComponent<RectTransform>().position.y, rect.position.z);
			// referencePointer.position = Camera.main.ScreenToWorldPoint(referenceWorldPoint);
		}

		private void Start()
		{
			rect = GetComponent<RectTransform>();
			gameManager = GetComponent<TirandoUmSomGameManager>();
			referencePointer = referencePointerImage.rectTransform;
		}

		bool entered = false;

		private void Update()
		{
			if (entered)
			{
				screenPoint = Input.mousePosition;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, Camera.main, out touchPoint);
				Vector3 referenceWorldPoint = new Vector3(screenPoint.x, screenPoint.y, rect.position.z);
				referencePointer.position = Vector3.Lerp(referencePointer.position, Camera.main.ScreenToWorldPoint(referenceWorldPoint), .3f);
				// Debug.Log("I'm in!!! At screenPoint " + screenPoint + " but local point " + touchPoint);
				float currentAmount = (touchPoint.x + Mathf.Abs(rect.rect.x)) / rect.rect.width;
				image.fillAmount = Mathf.Clamp(currentAmount, image.fillAmount, image.fillAmount + .15f);
				if (image.fillAmount > 0.85f)
				{
					image.fillAmount = 1f;
					// activate next point
					++currentMaskIndex;
					if (currentMaskIndex < maskObjects.Length)
					{
						Debug.Log("Switching lanes!!!");
						image.raycastTarget = false;
						maskObjects[currentMaskIndex].SetActive(true);
						rect = maskObjects[currentMaskIndex].GetComponent<RectTransform>();
						image = maskObjects[currentMaskIndex].GetComponent<Image>();
					}
					else
					{
						image.raycastTarget = false;
						Debug.Log("End of the road, bro. Maybe the tracing has ended?");
						entered = false;
						gameManager.FinishedTracing();
					}
				}
			}
		}

		public void ResetTracing()
		{
			currentMaskIndex = 0;
			for (int i = 0; i < maskObjects.Length; i++)
			{
				maskObjects[i].GetComponent<Image>().fillAmount = 0;
				maskObjects[i].GetComponent<Image>().raycastTarget = true;
				maskObjects[i].SetActive(false);
			}
			maskObjects[currentMaskIndex].SetActive(true);
		}

		public void EnterArea(RectTransform r)
		{
			entered = true;
			rect = r;
			image = r.GetComponent<Image>();
			screenPoint = Input.mousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, Camera.main, out touchPoint);
			// Debug.Log("I'm in!!! At screenPoint " + screenPoint + " but local point " + touchPoint);
		}

		public void ExitArea()
		{
			entered = false;
			// Debug.Log("I'm out! width: " + rect.rect.width + " left x: " + rect.rect.x);
		}
	}
}
