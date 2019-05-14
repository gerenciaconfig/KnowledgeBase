using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LineRendererAdjust : MonoBehaviour
{
	private LineRenderer lr;
	private List<Vector3> outlinePoints = new List<Vector3>();

	Vector3[] points;
	
	void Awake()
	{
		Debug.Log("AAAAAAJUSTANDO");
		EdgeCollider2D col = GetComponent<EdgeCollider2D>();
		// converting vector2 points from edgecollider2d to vector3 list
		col.points.ToList().ForEach(v2 => outlinePoints.Add(v2));

		lr = GetComponent<LineRenderer>();
		// points = new Vector3[lr.positionCount];
		// lr.GetPositions(points);
		// points = MakeSmoothCurve(points, 1f);
		points = outlinePoints.ToArray();
		lr.positionCount = outlinePoints.Count;
		lr.SetPositions(points);
		Debug.Log("Terminado os ajustes iniciais");
	}

	bool debugFlag = false;

	private void Update()
	{
		if (!debugFlag)
		{
			EdgeCollider2D col = GetComponent<EdgeCollider2D>();
			// converting vector2 points from edgecollider2d to vector3 list
			outlinePoints.Clear();
			col.points.ToList().ForEach(v2 => outlinePoints.Add(v2));
			lr = GetComponent<LineRenderer>();
			// points = new Vector3[lr.positionCount];
			// lr.GetPositions(points);
			points = outlinePoints.ToArray();
			points = MakeSmoothCurve(points, 2f);
			lr.positionCount = points.Length;
			lr.SetPositions(points);
			debugFlag = !debugFlag;
			Debug.Log("Updating the curve!");
		}
	}

	public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
	{
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if (smoothness < 1.0f)
			smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
		{
			t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for (int j = pointsLength - 1; j > 0; j--)
			{
				for (int i = 0; i < j; i++)
				{
					points[i] = (1 - t) * points[i] + t * points[i + 1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return (curvedPoints.ToArray());
	}
}
