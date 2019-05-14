using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arcolabs.General;

namespace Arcolabs.Managers
{
	/// <summary>
	/// <para>Manager for tracing lines on paths.</para>
	/// 
	/// <para>
	/// The paths are predefined images and require an EdgeCollider
	/// in order to get the points.
	/// </para>
	/// TODO add edge collider and generate circles in runtime
	/// </summary>
	public class LineTracingManager : MonoBehaviour, IManager
	{
		[SerializeField]
		private List<CircleCollider2D> masks;
		[SerializeField]
		private EdgeCollider2D maskSpawnPoints;
		[SerializeField]
		private GameObject maskPrefab;
		[SerializeField]
		private Transform inputReference;
		[SerializeField]
		private UnityEvent onTracingFinish;
		
		private int activeMaskIndex;

		private bool touchPressed = false;

		private Vector3 targetInputReferencePosition;

		private void Awake()
		{
			if (maskSpawnPoints)
			{
				CreateMaskPoints();
			}
			else
			{
				for (int i = masks.Count - 1; i > 0; i--)
				{
					masks[i].enabled = false;
				}
			}
			activeMaskIndex = 0;
			Color c = GetComponent<SpriteRenderer>().color;
			c.a = 0.7f;
			inputReference.GetComponent<SpriteRenderer>().color = c;
			inputReference.position = targetInputReferencePosition = new Vector3(masks[activeMaskIndex].transform.position.x, masks[activeMaskIndex].transform.position.y);
		}

		private void CreateMaskPoints()
		{
			masks = new List<CircleCollider2D>();
			float count = maskSpawnPoints.pointCount;
			Vector2[] points = maskSpawnPoints.points;

			for (int i = 0; i < count; i++)
			{
				GameObject mask = Instantiate(maskPrefab, transform);
				mask.transform.localPosition = new Vector3(
					points[i].x + maskSpawnPoints.transform.localPosition.x,
					points[i].y + maskSpawnPoints.transform.localPosition.y);
				// mask.transform.position = points[i];
				masks.Add(mask.GetComponent<CircleCollider2D>());
				masks[i].enabled = false;
			}
			masks[0].enabled = true;
		}

		private bool maskPressed = false;

		private float stateTime = 0f;

		private void Update()
		{
			// TODO move this to a callback
			if (Input.GetMouseButtonUp(0))
			{
				touchPressed = false;
			}
			inputReference.position = Vector3.Lerp(inputReference.position, targetInputReferencePosition, .1f);
			inputReference.localScale += Vector3.one * (Mathf.Sin(stateTime) * .0025f);
			stateTime += Time.deltaTime * 2.5f;
		}

		public void AddTracingFinishCallback(UnityAction action)
		{
			onTracingFinish.AddListener(action);
		}

		public void ResetPath()
		{
			int count = masks.Count;
			Debug.Log(gameObject.name + " " + count);
			for (int i = 0; i < count; i++)
			{
				masks[i].enabled = false;
				masks[i].gameObject.SetActive(true);
				// Debug.Log(gameObject.name + " " + i);
			}
			
			masks[0].enabled = true;
			activeMaskIndex = 0;
			inputReference.position = targetInputReferencePosition = new Vector3(masks[activeMaskIndex].transform.position.x, masks[activeMaskIndex].transform.position.y);
		}
	
		public void Notify(ObservableItem o)
		{
			if (o.GetStatus("MouseDown"))
			{
				touchPressed = true;
			}
			if (!o.GetStatus("MouseDown") && !o.GetStatus("MouseOver") || !touchPressed) return;

			masks[activeMaskIndex].enabled = false;
			masks[activeMaskIndex].gameObject.SetActive(false);
			activeMaskIndex++;
			if (activeMaskIndex >= masks.Count)
			{
				onTracingFinish.Invoke();
				return;
			}
			masks[activeMaskIndex].enabled = true;
			if (inputReference)
				targetInputReferencePosition = masks[activeMaskIndex].transform.position;
		}
	}
}
