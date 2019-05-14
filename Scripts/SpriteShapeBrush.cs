using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arcolabs.Utilities;
using UnityEngine.Experimental.U2D;

namespace Arcolabs.PathTracing
{
	public class SpriteShapeBrush : SerializedMonoBehaviour
	{
		[SerializeField]
		private GameObject maskBrush;
		[SerializeField]
		private Transform maskBrushReference;
		[SerializeField]
		private float maskReferenceRadius = .5f;
		[SerializeField]
		private string patternLoopName;
		public bool isPatternLoop = false;
		[BoxGroup("The drum pattern for this instrument", true, true, 1)]
		public bool[,] pattern = new bool[16, 1];
		
		[SerializeField]
		private UnityEvent onFinish;
		[SerializeField]
		private Animator[] instrumentsAnimator;
		[SerializeField]
		private string highlightAnimationName = "InstrumentHighlightExpansion";
		[SerializeField]
		private Transform debugReference;

		private int instrumentsLength;
		private int currentInstrument;

		private static float beatTime = 0f;
		private static float currentTime = 0f;

		// private List<Vector2> colliderPoints = new List<Vector2>();
		private List<Vector2> colliderWorldPoints = new List<Vector2>();

		private List<GameObject> masks;

		private float stateTime = 0f;

		private Vector2 lastPointerWorldPosition;
		private bool finishedTracing = false;

		private void Start()
		{
			masks = new List<GameObject>();
			// ReversePoints(); // Use this if edge collider points are inverted
			CreateColliderPoints();
			lastPointerWorldPosition = new Vector2();
			instrumentsLength = instrumentsAnimator.Length;
			currentInstrument = 0;
		}

		private void ReversePoints()
		{
			EdgeCollider2D cPoints = GetComponent<EdgeCollider2D>();
			List<Vector2> reversedVertices = new List<Vector2>();
			int count = cPoints.pointCount;
			for (int i = count - 1; i >= 0; i--)
			{
				reversedVertices.Add(cPoints.points[i]);
			}
			cPoints.points = reversedVertices.ToArray();
		}

		// TODO Update the name of this method
		public void BeatUpdate()
		{
			int beatStep = MusicPatternController.GetCurrentBeatIndex();
			if (isPatternLoop && finishedTracing && pattern[beatStep, 0])
			{
				AudioManager.instance.PlaySound(patternLoopName);
				instrumentsAnimator[currentInstrument].Play(highlightAnimationName);
				currentInstrument = (currentInstrument + 1) % instrumentsLength;
			}
		}

		private void CreateColliderPoints()
		{
			// colliderPoints.Clear();
			colliderWorldPoints.Clear();
			EdgeCollider2D cPoints = GetComponent<EdgeCollider2D>();
			int count = cPoints.pointCount;
			for (int i = 0; i < count; i++)
			{
				// colliderPoints.Add(cPoints.points[i]);
				colliderWorldPoints.Add(transform.TransformPoint(cPoints.points[i]));
			}
			// maskBrushReference.localPosition = colliderPoints[0];
			maskBrushReference.position = colliderWorldPoints[0];
		}

		private void Update()
		{
			stateTime += Time.deltaTime * 3f;
			maskBrushReference.localScale = Vector3.one * maskReferenceRadius + Vector3.one * Mathf.Sin(stateTime) * .05f;

			if (dragging || Vector2.Distance(colliderWorldPoints[0], lastPointerWorldPosition) < 1.5f) UpdateTracing();
		}

		public void LoopPatternWhenFinished(bool b)
		{
			isPatternLoop = b;
		}

		private Texture2D GetTexture2DFrom(Texture mainTexture)
		{
			// Texture mainTexture = renderer.material.mainTexture;
			Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

			RenderTexture currentRT = RenderTexture.active;

			RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
			Graphics.Blit(mainTexture, renderTexture);

			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();

			Color[] pixels = texture2D.GetPixels();

			RenderTexture.active = currentRT;
			return texture2D;
		}
	
		private bool dragging = false;

		private void OnMouseDown()
		{
			Vector3 inputPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
			// float distance = Vector2.Distance(transform.TransformPoint(colliderPoints[0]), inputPos);
			// float distance = Vector2.Distance(colliderWorldPoints[0], inputPos);
			float distance = Vector2.Distance(maskBrushReference.position, inputPos);
			dragging = (distance < .2f);
			lastPointerWorldPosition = inputPos;
		}

		private void OnMouseUp() { dragging = false; }
		private void OnMouseExit()
		{
			dragging = false;
			// maskBrushReference.position = transform.TransformPoint(colliderPoints[0]);
			// maskBrushReference.position = colliderWorldPoints[0];
			// clamp maskBrush?
		}

		private void OnMouseDrag()
		{
			// UpdateTracing();
		}

		private void UpdateTracing()
		{
			if (colliderWorldPoints.Count < 2)
			{
				if (!finishedTracing)
				{
					onFinish.Invoke();
				}
				finishedTracing = true;
				return;
			}
			Vector3 inputPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
			Vector2 v0 = new Vector2(colliderWorldPoints[0].x, colliderWorldPoints[0].y);
			Vector2 v1 = new Vector2(colliderWorldPoints[1].x, colliderWorldPoints[1].y);
			/*
			// plotting the equation for y = mx + b, where m is the slope of the perpendicular line
			float m = (v1.y - v0.y) / ((v1.x != v0.x) ? (v1.x - v0.x) : 1);
			// equation for b = y - mx
			float b = v0.y - m * v0.x;
			// finding the m and b for the perpendicular
			float pm = -(1 / m);
			// equation for pb = y - pmx
			float pb = inputPos.y - inputPos.x * pm;
			// equalizing both y's for intersecting x = b + mx - pb - pmx
			float ix = b + m * inputPos.x - pm * inputPos.x - pb;
			float iy = pb + pm * ix;
			Vector2 normal = Vector2.Perpendicular(inputPos);
			Vector2 calcNormal = new Vector2(ix, iy);
			Debug.Log("calculatedNormal: " + calcNormal + " Vector2.Perpendicular(): " + normal);
			Vector2 clampNormal = new Vector2(
				Mathf.Clamp(calcNormal.x, 
					Mathf.Min(v0.x, v1.x), Mathf.Max(v0.x, v1.x)),
				Mathf.Clamp(calcNormal.y,
					Mathf.Min(v0.y, v1.y), Mathf.Max(v0.y, v1.y))
			);
			// maskBrushReference.position = clampNormal; // new Vector2(ix, iy);
			debugReference.position = Vector2.Lerp(debugReference.position, clampNormal, .15f);
			*/
			float maxDistance = Vector2.Distance(v0, v1);
			float currentDistance = Vector2.Distance(inputPos, v1);
			float lerpFactor = 1 - (Mathf.Clamp(currentDistance, 0.01f, maxDistance) / maxDistance);
			Debug.Log(currentDistance + " " + maxDistance + " " + lerpFactor);
			//debugReference.position = Vector2.Lerp(v0, v1, lerpFactor);

			maskBrushReference.position = new Vector2(
					Mathf.Clamp(
						Mathf.Lerp(maskBrushReference.position.x, inputPos.x, .15f),
						Mathf.Min(v0.x, v1.x), Mathf.Max(v0.x, v1.x)),
					Mathf.Clamp(
						Mathf.Lerp(maskBrushReference.position.y, inputPos.y, .15f),
						Mathf.Min(v0.y, v1.y), Mathf.Max(v0.y, v1.y))
			);


			// maskBrushReference.position = Vector2.Lerp(v0, v1, lerpFactor);

			float distance = Vector2.Distance(maskBrushReference.position, v1);

			if (distance < .08f)
			{
// 				Debug.Log("Removing points! " + colliderWorldPoints[0]);
				AddNewMaskAt(new Vector3(colliderWorldPoints[1].x, colliderWorldPoints[1].y));
				colliderWorldPoints.RemoveAt(0);
			}
			lastPointerWorldPosition = inputPos;
		}

		private void AddNewMaskAt(Vector3 position)
		{
			masks.Add(Instantiate(maskBrush, position, transform.rotation, transform));
		}

		public void OnEnable()
		{
			ResetPaths();
		}

		public void ResetPaths()
		{
			CreateColliderPoints();
			finishedTracing = false;
			if (masks == null || masks.Count == 0) return;
			foreach (GameObject go in masks)
			{
				Destroy(go);
			}
			masks.Clear();
		}
	}
}
