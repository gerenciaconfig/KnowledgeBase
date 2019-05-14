using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs
{
	public class PathCreator : MonoBehaviour
	{
		[HideInInspector]
		public Path path;

		public Color anchorCol = Color.red;
		public Color controlCol = Color.white;
		public Color segmentCol = Color.green;
		public Color selectedSegmentCol = Color.yellow;
		public float anchorDiameter = .1f;
		public float controlDiameter = .075f;
		public bool displayControlPoints = true;
		public float lineRendererSpacing = 0.2f;

		public void CreatePath()
		{
			path = new Path(transform.position);

		}

		void Reset()
		{
			CreatePath();
		}

		public void UpdateLineRenderer()
		{
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			Debug.Log("Updating the Line Renderer... Somehow...");
			// lineRenderer.positionCount = path.NumPoints / 3 + 1;
			// Vector3[] positions = new Vector3[path.NumPoints / 3 + 1];

			//for (int i = 0; i < path.NumPoints; i += 3)
			//{
			//	print("Passing the index " + i + " of " + path.NumPoints + " " + (i / 3) + " count: " + lineRenderer.positionCount);
			//	positions[i / 3] = path[i];
			//}
			// Converting the Vector2[] to Vector3[]
			Vector2[] positions = path.CalculateEvenlySpacedPoints(lineRendererSpacing);
			lineRenderer.positionCount = positions.Length;
			Vector3[] lineRendererPositions = new Vector3[positions.Length];
			for (int i = 0; i < lineRendererPositions.Length; i++)
			{
				lineRendererPositions[i] = positions[i];
			}
			lineRenderer.SetPositions(lineRendererPositions);
		}
	}
}
