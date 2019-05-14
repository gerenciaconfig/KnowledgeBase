using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Controllers
{
	public class LineTracingController : MonoBehaviour
	{
		private LineRenderer drawingLine;
		private Transform referenceBrush;

		private Vector3[] curvePoints;
		private int currentCurvePointIndex;
		// The first child is always the one with the LineRenderer component
		// the second child has the brush reference guide
		// the third child contains the traceable path. It is separated because the curve editor
		// currently can't work with a scale other than one
		void Awake()
		{
			drawingLine = transform.GetChild(0).GetComponent<LineRenderer>();
			referenceBrush = transform.GetChild(1);

			int count = drawingLine.positionCount;
			curvePoints = new Vector3[count];
			currentCurvePointIndex = 1;
			drawingLine.GetPositions(curvePoints);
			drawingLine.positionCount = 1;
			drawingLine.SetPositions(new Vector3[] { drawingLine.GetPosition(0) });
		}

	
		// Update is called once per frame
		void Update ()
		{
			Vector3 inputPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
			inputPos.z = 0;
		
			float inputDistanceToNextPoint = Vector3.Distance(inputPos, curvePoints[currentCurvePointIndex]);
			while (currentCurvePointIndex < curvePoints.Length)
			{
				// Debug.Log("Looping! input: " + inputPos + " curvePoints: " + curvePoints[currentCurvePointIndex]);
				inputDistanceToNextPoint = Vector3.Distance(inputPos, curvePoints[currentCurvePointIndex]);
				if (inputDistanceToNextPoint < .5f)
				{
					Debug.Log("Connected! input: " + inputPos + " curvePoints: " + curvePoints[currentCurvePointIndex]);
					drawingLine.positionCount++;
					drawingLine.SetPosition(currentCurvePointIndex, curvePoints[currentCurvePointIndex]);
					currentCurvePointIndex++;
				}
				else
				{
					break;
				}
			}
		}

		private void OnEnable()
		{
			currentCurvePointIndex = 0;
			drawingLine.positionCount = 1;
			drawingLine.SetPositions(new Vector3[] { drawingLine.GetPosition(0) });
		}
	}
}
