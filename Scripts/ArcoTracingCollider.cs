using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ArcoTracingCollider : MonoBehaviour
{
    [Tooltip("Distância máxima entre o dedo da criança e o colisor do Tracing para que a interação seja considerada")]
    [SerializeField]
    private float detectionDistance = .35f;
	[SerializeField]
	private RectTransform referencePointer;
	[SerializeField]
	private Transform goalReference;
	[SerializeField]
	private GameObject tracingPointsParent;

	[SerializeField]
	[ReadOnly]
	private Vector3 initialReferencePosition;

	[SerializeField]
	[ReadOnly]
	private GameObject[] childObjects;
	private int currentObjectIndex = 0;

	[SerializeField]
	[ReadOnly]
	private RectTransform[] goalPoints;
	int currentGoalPointIndex = 0;

	private Image fillImage;

	private Vector2 touchPoint;
	private RectTransform fillObject;
	private string fillOrigin;

	private bool finished = false;
	public bool Finished { get { return finished; } }

	[SerializeField]
	private UnityEvent onFinishCallbacks;

	private Transform nextInputTransform;

    void Awake()
    {
		UpdateCurrentTracingSegment(childObjects[0].transform);
        ResetEverything();

    }

	[Button]
	private void CreateMaskPoints()
	{
		EdgeCollider2D pathCollider = GetComponent<EdgeCollider2D>();
		int count = pathCollider.pointCount;
		goalPoints = new RectTransform[count];
		GameObject emptyGoalPoint = new GameObject();
		emptyGoalPoint.AddComponent<RectTransform>();
		for (int i = 0; i < count; i++)
		{
			GameObject go = Instantiate(emptyGoalPoint, tracingPointsParent.transform);
			go.transform.localPosition = pathCollider.points[i];
			SnapAnchors(go);
			go.name = string.Format("Goal Point ({0})", i);
			goalPoints[i] = go.GetComponent<RectTransform>();
		}
	}

	// TODO remover esse código daqui
	public static void SnapAnchors(GameObject g)
	{
		RectTransform recTransform = null;
		RectTransform parentTransform = null;

		if (g.transform.parent != null)
		{
			if (g.GetComponent<RectTransform>() != null)
				recTransform = g.GetComponent<RectTransform>();
			else
				return;

			if (parentTransform == null)
				parentTransform = g.transform.parent.GetComponent<RectTransform>();

			//Undo.RecordObject(recTransform, "Snap Anchors");

			Vector2 offsetMin = recTransform.offsetMin;
			Vector2 offsetMax = recTransform.offsetMax;
			Vector2 anchorMin = recTransform.anchorMin;
			Vector2 anchorMax = recTransform.anchorMax;
			Vector2 parent_scale = new Vector2(parentTransform.rect.width, parentTransform.rect.height);
			recTransform.anchorMin = new Vector2(anchorMin.x + (offsetMin.x / parent_scale.x), anchorMin.y + (offsetMin.y / parent_scale.y));
			recTransform.anchorMax = new Vector2(anchorMax.x + (offsetMax.x / parent_scale.x), anchorMax.y + (offsetMax.y / parent_scale.y));
			recTransform.offsetMin = Vector2.zero;
			recTransform.offsetMax = Vector2.zero;
		}
	}

	public void ResetChildren()
	{
		currentObjectIndex = 0;
		int count = transform.childCount;
		childObjects = new GameObject[count];
		for (int i = 0; i < count; i++)
		{
			childObjects[i] = transform.GetChild(i).gameObject;
			childObjects[i].GetComponent<Image>().fillAmount = 0f;
		}
		childObjects[0].gameObject.SetActive(true);
		UpdateCurrentTracingSegment(childObjects[0].transform);
		if (initialReferencePosition != null)
		{
			referencePointer.position = initialReferencePosition;
		}
		// yield return null;
	}

	[Button]
	public void GetChildren()
	{
		initialReferencePosition = referencePointer.position;
		ResetChildren();
	}

	// Update is called once per frame
	void Update()
    {
		referencePointer.localPosition = Vector3.Lerp(referencePointer.localPosition, goalReference.localPosition, .2f);
		if (finished) return;
		Vector3 inputPosition = GetCurrentPlatformClickPosition(Camera.main);
		if (currentGoalPointIndex < goalPoints.Length)
		{
			// if (Vector2.Distance(inputPosition, goalReference.position) < .75f)
			if (Vector2.Distance(inputPosition, nextInputTransform.position) < detectionDistance)
            {
                goalReference = nextInputTransform;
                if (currentGoalPointIndex < goalPoints.Length - 1)
                {
                    nextInputTransform.localPosition = goalPoints[currentGoalPointIndex + 1].localPosition;
                    Vector3 referencePointerToScreenPoint = Camera.main.WorldToScreenPoint(nextInputTransform.position);
    				RectTransformUtility.ScreenPointToLocalPointInRectangle(
    				 	fillObject, referencePointerToScreenPoint, Camera.main, out touchPoint);
    				// goalReference.localPosition = goalPoints[currentGoalPointIndex].localPosition;
    				// Vector3 referencePointerToScreenPoint = Camera.main.WorldToScreenPoint(goalReference.position);
    				// RectTransformUtility.ScreenPointToLocalPointInRectangle(
    				// 	fillObject, referencePointerToScreenPoint, Camera.main, out touchPoint);

    				UpdateFillValue(fillImage.fillAmount);
    				currentGoalPointIndex++;
                }
            }
		}
	}

	private void UpdateFillValue(float value)
	{
		value = CalculateFillFromOrigin();
		// Updating the fill amount value
		if (value > 0.9f)
		{
			fillImage.fillAmount = 1f;
			value = 0f;
			currentObjectIndex++;
			if (currentObjectIndex < childObjects.Length)
			{
				// we're finished
				childObjects[currentObjectIndex].gameObject.SetActive(true);
				UpdateCurrentTracingSegment(childObjects[currentObjectIndex].transform);
			}
			else
			{
				finished = true;
				onFinishCallbacks.Invoke();
				Debug.Log("I think we're done here, chief...");
			}
		}
		else
		{
			fillImage.fillAmount = value;
		}
	}

	private float CalculateFillFromOrigin()
	{
		if (fillOrigin == "Top")
		{
			return (Mathf.Abs(fillObject.rect.y) - touchPoint.y) / fillObject.rect.height;
		}
		else if (fillOrigin == "Bottom")
		{
			return (Mathf.Abs(fillObject.rect.y) + touchPoint.y) / fillObject.rect.height;
		}
		else if (fillOrigin == "Left")
		{
			return (Mathf.Abs(fillObject.rect.x) + touchPoint.x) / fillObject.rect.width;
		}
		else if (fillOrigin == "Right")
		{
			return (Mathf.Abs(fillObject.rect.x) - touchPoint.x) / fillObject.rect.width;
		}
		return 0f;
	}

	public void UpdateCursorColor()
	{
		referencePointer.GetComponent<Image>().color = 
			fillImage.color;
	}

	public void ResetEverything()
	{
		ResetChildren();
        initialReferencePosition = goalPoints[0].position;
		referencePointer.position = initialReferencePosition;
		goalReference.position = referencePointer.position;
		currentObjectIndex = currentGoalPointIndex = 0;
		nextInputTransform = goalPoints[currentObjectIndex + 1].transform;
		finished = false;
	}

	private void UpdateCurrentTracingSegment(Transform t)
	{
		fillObject = t.GetComponent<RectTransform>();
		fillImage = fillObject.GetComponent<Image>();
		fillOrigin = GetFillOrigin(fillImage);
	}

	private string GetFillOrigin(Image image)
	{
		string fillOriginName = "";

		switch (image.fillMethod)
		{
			case Image.FillMethod.Horizontal:
				fillOriginName = ((Image.OriginHorizontal)image.fillOrigin).ToString();
				break;
			case Image.FillMethod.Vertical:
				fillOriginName = ((Image.OriginVertical)image.fillOrigin).ToString();
				break;
			case Image.FillMethod.Radial90:
				fillOriginName = ((Image.Origin90)image.fillOrigin).ToString();
				break;
			case Image.FillMethod.Radial180:
				fillOriginName = ((Image.Origin180)image.fillOrigin).ToString();
				break;
			case Image.FillMethod.Radial360:
				fillOriginName = ((Image.Origin360)image.fillOrigin).ToString();
				break;
		}
		// Debug.Log(string.Format("{0} is using {1} fill method with the origin on {2}", name, image.fillMethod, fillOriginName));
		return fillOriginName;
	}

	/// <summary>
	/// Get the current platform click position.
	/// Note: taken from the Letters asset
	/// </summary>
	/// <returns>The current platform click position.</returns>
	private Vector3 GetCurrentPlatformClickPosition(Camera camera)
	{
		Vector3 clickPosition = Vector3.zero;

		if (Application.isMobilePlatform)
		{//current platform is mobile
			if (Input.touchCount != 0)
			{
				Touch touch = Input.GetTouch(0);
				clickPosition = touch.position;
			}
		}
		else
		{//others
			clickPosition = Input.mousePosition;
		}

		clickPosition = camera.ScreenToWorldPoint(clickPosition);//get click position in the world space
		clickPosition.z = 0;
		return clickPosition;
	}
}
