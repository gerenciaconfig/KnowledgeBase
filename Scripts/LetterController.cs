using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using TracingPackage;
using UnityEngine.UI;
using Fungus;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

//Todo: Remover os 'FindGameObject' e os 'GetComponent' que s�o chamados durante o tempo de execu��o.
namespace Letra
{
	public class LetterController : MonoBehaviour
	{
		//public bool isRunning = true;

		/// <summary>
		/// The current pencil.
		/// </summary>
		//public Pencil currentPencil;
		[Header("Letter Setup")]
		[Tooltip("Se for ajustar a letra manualmente deixe esse campo vazio e delete o objeto 'Reference']")] [SerializeField] private GameObject letterToInvoke;
		[SerializeField] private Sprite renderedSprite;
		[SerializeField] private Color colorPallet = Color.grey;

		//[Header("Starting")]
		//[SerializeField] private BlockReference onStartBlock;
		//[SerializeField] private BlockReference onStartBlock;


		[Header("Win Behaviors")]
		[SerializeField] [Range(1, 3)] private float winEffectDuration = 1;
		[SerializeField] private RectTransform targetPosOnWin;
		[SerializeField] private Animator smokeAnimator;
		[SerializeField] private float timeToReachTarget;
		[SerializeField] private BlockReference onWinImediateBlock;
		[Tooltip("Bloco a ser chamado dentro de uma flowchart assim que a anima��o de vit�ria terminar")] [SerializeField] private BlockReference afterWinBlock;
		[Tooltip("Executado durante as anima��es de vit�ria")][SerializeField] private UnityEvent onWinAdittionalEvents;

		private CanvasGroup brightCG;
		private ParticleSystem winParticles;
		//private FindObjectLogic findGameLogic;

		

		/// <summary>
		/// The path.
		/// </summary>
		private Path path;

		/// <summary>
		/// The shape parent.
		/// </summary>
		//public Transform shapeParent;

		/// <summary>
		/// The shape reference.
		/// </summary>
		[HideInInspector]
		public Shape shape;

		/// <summary>
		/// The path fill image.
		/// </summary>
		private Image pathFillImage;

		/// <summary>
		/// The click postion.
		/// </summary>
		private Vector3 clickPostion;

		/// <summary>
		/// The direction between click and shape.
		/// </summary>
		private Vector2 direction;

		/// <summary>
		/// The current angle , angleOffset and fill amount.
		/// </summary>
		private float angle, angleOffset, fillAmount;

		/// <summary>
		/// The clock wise sign.
		/// </summary>
		private float clockWiseSign;


		/// <summary>
		/// The target quarter of the radial fill.
		/// </summary>
		private float targetQuarter;

		/// <summary>
		/// The shape label.
		/// </summary>
		//public string shapeLabel = "Shape";

		/// <summary>
		/// The hit2d reference.
		/// </summary>
		private RaycastHit2D hit2d;

		/// <summary>
		/// Static instance of this class.
		/// </summary>
		public static LetterController instance;

		public void SetupGame()
		{
			//Initiate GameManager instance 
			if (instance == null)
			{
				instance = this;
			}

			//findGameLogic = FindObjectOfType<FindObjectLogic>();

			if (letterToInvoke)
			{
				Shape reference = transform.GetComponentInChildren<Shape>(true);
				shape = Instantiate(letterToInvoke, transform).GetComponent<Shape>();
				shape.gameObject.name = letterToInvoke.name;

				RectTransform refRect = reference.GetComponent<RectTransform>();
				RectTransform shpRect = shape.GetComponent<RectTransform>();

				shpRect.anchorMin = refRect.anchorMin;
				shpRect.anchorMax = refRect.anchorMax;
				shpRect.anchoredPosition = refRect.anchoredPosition;
				shpRect.sizeDelta = refRect.sizeDelta;
				//shpRect.transform.localScale = refRect.transform.localScale;

				//Destroy(reference.gameObject);

				//StartGame();
			}
		}

		// Use this for initialization
		public void StartGame ()
		{
			ResetTargetQuarter ();
			shape = GameObject.FindObjectOfType<Shape>();
			// transform.GetComponentInChildren<Shape>(true);
			winParticles = transform.GetComponentInChildren<ParticleSystem>(true);
			brightCG = transform.GetComponentInChildren<CanvasGroup>(true);

			//onStartBlock.Execute();
		}

		// Update is called once per frame
		void Update ()
		{
			//Game Logic is here

			DrawBrightEffect (GetCurrentPlatformClickPosition (Camera.main));
			if (shape == null) {
				return;
			}	

			if (shape.completed) {
				return;
			}

			if (Input.GetMouseButtonDown (0)) {
				//if (!shape.completed)
					//brightEffect.GetComponent<ParticleEmitter> ().emit = true;

				hit2d = Physics2D.Raycast (GetCurrentPlatformClickPosition (Camera.main), Vector2.zero);
				if (hit2d.collider != null) {
					if (hit2d.transform.tag == "Start") {
						OnStartHitCollider (hit2d);
						shape.CancelInvoke ();
						shape.DisableTracingHand ();
					} else if (hit2d.transform.tag == "Collider") {
						shape.DisableTracingHand ();
					}
				}

			} else if (Input.GetMouseButtonUp (0)) {
				//brightEffect.GetComponent<ParticleEmitter> ().emit = false;
				shape.Invoke ("EnableTracingHand", 1);
				ResetPath ();
			}

			if (path == null || pathFillImage == null) {
				return;
			}

			if (path.completed) {
				return;
			}

			hit2d = Physics2D.Raycast (GetCurrentPlatformClickPosition (Camera.main), Vector2.zero);
			if (hit2d.collider == null) {

				ResetPath ();
				return;
			}

			if (path.fillMethod == Path.FillMethod.Radial) {
				RadialFill ();
			} else if (path.fillMethod == Path.FillMethod.Linear) {
				LinearFill ();
			} else if (path.fillMethod == Path.FillMethod.Point) {
				PointFill ();
			}
		}

		/// <summary>
		/// On the start hit collider event.
		/// </summary>
		/// <param name="hit2d">Hit2d.</param>
		private void OnStartHitCollider (RaycastHit2D hit2d)
		{
			path = hit2d.transform.GetComponentInParent<Path> ();
			pathFillImage = CommonUtil.FindChildByTag (path.transform, "Fill").GetComponent<Image> ();
			if (path.completed || !shape.IsCurrentPath (path)) {
				ReleasePath ();
			} else {
				path.StopAllCoroutines ();
				CommonUtil.FindChildByTag (path.transform, "Fill").GetComponent<Image> ().color = colorPallet;
			}
		}

		/*Old Shape Creator
		/// <summary>
		/// Create new shape.
		/// </summary>
		private void CreateShape ()
		{
			//Area.Hide ();
			//GameObject.Find ("NextButton").GetComponent<Animator> ().SetBool ("Select", false);
			Shape shapeComponent = GameObject.FindObjectOfType<Shape> ();
			if (shapeComponent != null) {
				Destroy (shapeComponent.gameObject);
			}

			try {
				shape = GameObject.FindObjectOfType<Shape> ();
			} catch (System.Exception ex) {
				//Catch the exception or display an alert
			}

			if (shape == null) {
				return;
			}


			//Transform restConfirmMessage = CommonUtil.FindChildByTag (GameObject.Find ("ResetConfirmDialog").transform, "Message");
			//restConfirmMessage.GetComponent<Text> ().text = "Reset " + shapeLabel + " " + shape.GetTitle () + " ?";
			//EnableGameManager ();
		}*/

		/// <summary>
		/// Get the current platform click position.
		/// </summary>
		/// <returns>The current platform click position.</returns>
		private Vector3 GetCurrentPlatformClickPosition (Camera camera)
		{
			Vector3 clickPosition = Vector3.zero;

			if (Application.isMobilePlatform) {//current platform is mobile
				if (Input.touchCount != 0) {
					Touch touch = Input.GetTouch (0);
					clickPosition = touch.position;
				}
			} else {//others
				clickPosition = Input.mousePosition;
			}

			clickPosition = camera.ScreenToWorldPoint (clickPosition);//get click position in the world space
			clickPosition.z = 0;
			return clickPosition;
		}

		/// <summary>
		/// Radial the fill method.
		/// </summary>
		private void RadialFill ()
		{
			clickPostion = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			direction = clickPostion - path.transform.position;

			angleOffset = 0;
			clockWiseSign = (pathFillImage.fillClockwise ? 1 : -1);

			if (pathFillImage.fillOrigin == 0) {//Bottom
				angleOffset = 0;
			} else if (pathFillImage.fillOrigin == 1) {//Right
				angleOffset = clockWiseSign * 90;
			} else if (pathFillImage.fillOrigin == 2) {//Top
				angleOffset = -180;
			} else if (pathFillImage.fillOrigin == 3) {//left
				angleOffset = -clockWiseSign * 90;
			}

			angle = Mathf.Atan2 (-clockWiseSign * direction.x, -direction.y) * Mathf.Rad2Deg + angleOffset;

			if (angle < 0)
				angle += 360;

			angle = Mathf.Clamp (angle, 0, 360);
			angle -= path.radialAngleOffset;

			if (path.quarterRestriction) {
				if (!(angle >= 0 && angle <= targetQuarter)) {
					pathFillImage.fillAmount = 0;
					return;
				}

				if (angle >= targetQuarter / 2) {
					targetQuarter += 90;
				} else if (angle < 45) {
					targetQuarter = 90;
				}

				targetQuarter = Mathf.Clamp (targetQuarter, 90, 360);
			}

			fillAmount = Mathf.Abs (angle / 360.0f);
			pathFillImage.fillAmount = fillAmount;
			CheckPathComplete ();
		}

		/// <summary>
		/// Linear fill method.
		/// </summary>
		private void LinearFill ()
		{
			clickPostion = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			Vector3 rotation = path.transform.eulerAngles;
			rotation.z -= path.offset;

			Rect rect = CommonUtil.RectTransformToScreenSpace (path.GetComponent<RectTransform> ());

			Vector3 pos1 = Vector3.zero, pos2 = Vector3.zero;

			if (path.type == Path.ShapeType.Horizontal) {
				pos1.x = path.transform.position.x - Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
				pos1.y = path.transform.position.y - Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;

				pos2.x = path.transform.position.x + Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
				pos2.y = path.transform.position.y + Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.width / 2.0f;
			} else {

				pos1.x = path.transform.position.x - Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
				pos1.y = path.transform.position.y - Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;

				pos2.x = path.transform.position.x + Mathf.Cos (rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
				pos2.y = path.transform.position.y + Mathf.Sin (rotation.z * Mathf.Deg2Rad) * rect.height / 2.0f;
			}

			pos1.z = path.transform.position.z;
			pos2.z = path.transform.position.z;

			GameObject obj1 = GameObject.Find ("obj1");
			if (obj1 == null) {
				obj1 = new GameObject ("obj1");
			}

			GameObject obj2 = GameObject.Find ("obj2");
			if (obj2 == null) {
				obj2 = new GameObject ("obj2");
			}

			if (path.flip) {
				Vector3 temp = pos2;
				pos2 = pos1;
				pos1 = temp;
			}

			obj1.transform.position = pos1;
			obj2.transform.position = pos2;

			clickPostion.x = Mathf.Clamp (clickPostion.x, Mathf.Min (pos1.x, pos2.x), Mathf.Max (pos1.x, pos2.x));
			clickPostion.y = Mathf.Clamp (clickPostion.y, Mathf.Min (pos1.y, pos2.y), Mathf.Max (pos1.y, pos2.y));
			fillAmount = Vector2.Distance (clickPostion, pos1) / Vector2.Distance (pos1, pos2);
			pathFillImage.fillAmount = fillAmount;
			CheckPathComplete ();
		}

		/// <summary>
		/// Point fill.
		/// </summary>
		private void PointFill ()
		{
			pathFillImage.fillAmount = 1;
			CheckPathComplete ();
		}

		/// <summary>
		/// Checks wehther path completed or not.
		/// </summary>
		private void CheckPathComplete ()
		{
			if (fillAmount >= path.completeOffset) {

				path.completed = true;
				path.AutoFill ();
				path.SetNumbersVisibility (false);
				ReleasePath ();
				if (CheckShapeComplete ()) {
					shape.completed = true;
					OnShapeComplete ();
				} else {
					//Originalmente aqui se toca o audio de vit�ria...
				}

				shape.ShowPathNumbers (shape.GetCurrentPathIndex ());

				hit2d = Physics2D.Raycast (GetCurrentPlatformClickPosition (Camera.main), Vector2.zero);
				if (hit2d.collider != null) {
					if (hit2d.transform.tag == "Start") {
						if (shape.IsCurrentPath (hit2d.transform.GetComponentInParent<Path> ())) {
							ResetPath ();
							OnStartHitCollider (hit2d);
						}
					}
				}
			}
		}

		/// <summary>
		/// Check whether the shape completed or not.
		/// </summary>
		/// <returns><c>true</c>, if shape completed, <c>false</c> otherwise.</returns>
		private bool CheckShapeComplete ()
		{
			bool shapeCompleted = true;
			Path [] paths = GameObject.FindObjectsOfType<Path> ();
			foreach (Path path in paths) {
				if (!path.completed) {
					shapeCompleted = false;
					break;
				}
			}
			return shapeCompleted;
		}

		/// <summary>
		/// On shape completed event.
		/// </summary>
		private void OnShapeComplete ()
		{
			//brightEffect.GetComponent<ParticleEmitter> ().emit = false;

			Animator shapeAnimator = shape.GetComponent<Animator> ();
			shapeAnimator.SetBool (shape.name, false);
			shapeAnimator.SetTrigger ("Completed");

			//ShapesManager.Shape.StarsNumber collectedStars = Progress.instance.starsNumber;
			//AlphabetDataManager.SaveShapeStars(ShapesManager.Shape.selectedShapeID, collectedStars);

			//if (ShapesManager.Shape.selectedShapeID + 1 < ShapesManager.instance.shapes.Count)
			//{
				//AlphabetDataManager.SaveShapeLockedStatus (ShapesManager.Shape.selectedShapeID+ 1, false);
			//}

			List <Transform> paths = CommonUtil.FindChildrenByTag (shape.transform.Find ("Paths"), "Path");
			int from, to;
			string [] slices;
			foreach (Transform p in paths) {
				slices = p.name.Split ('-');
				from = int.Parse (slices [1]);
				to = int.Parse (slices [2]);
				AlphabetDataManager.SaveShapePathColor (ShapesManager.Shape.selectedShapeID, from, to, CommonUtil.FindChildByTag (p, "Fill").GetComponent<Image> ().color);
			}

			winParticles.Play();
			brightCG.alpha = 1;
			onWinImediateBlock.Execute();

			//StartCoroutine(AfterComplete());
			
		}

		public void AfterWinStartAnimation()
		{
			StartCoroutine(AfterComplete());
		}

		private IEnumerator AfterComplete()
		{
			float timer = 0;
			//yield return new WaitForSeconds(1+(winEffectDuration-1));

			do
			{
				yield return new WaitForSeconds(0);
				timer += Time.deltaTime;
				brightCG.alpha = 1 - timer;

			} while (brightCG.alpha > 0);


			if (targetPosOnWin)
			{
				StartCoroutine(GoToTargetPosition(timeToReachTarget));
			}
			else
			{
				afterWinBlock.Execute();
			}

			onWinAdittionalEvents.Invoke();

		}

		/// <summary>
		/// Draw the bright effect.
		/// </summary>
		/// <param name="clickPosition">Click position.</param>
		private void DrawBrightEffect (Vector3 clickPosition)
		{
			/*
			if (brightEffect == null) {
				return;
			}

			clickPosition.z = 0;
			brightEffect.transform.position = clickPosition;
			*/
		}

		/// <summary>
		/// Reset the shape.
		/// </summary>
		public void ResetShape ()
		{
			if (shape == null) {
				return;
			}

			shape.completed = false;
			shape.GetComponent<Animator> ().SetBool ("Completed", false);
			shape.CancelInvoke ();
			shape.DisableTracingHand ();
			Path [] paths = GameObject.FindObjectsOfType<Path> ();
			foreach (Path path in paths) {
				path.Reset ();
			}
			shape.Invoke ("EnableTracingHand", 2);
			shape.ShowPathNumbers (shape.GetCurrentPathIndex ());
		}

		/// <summary>
		/// Reset the path.
		/// </summary>
		private void ResetPath ()
		{
			if (path != null) 
				path.Reset ();
			ReleasePath ();
			ResetTargetQuarter ();
		}

		/// <summary>
		/// Reset the target quarter.
		/// </summary>
		private void ResetTargetQuarter ()
		{
			targetQuarter = 90;
		}

		/// <summary>
		/// Release the path.
		/// </summary>
		private void ReleasePath ()
		{
			path = null;
			pathFillImage = null;
		}

		private IEnumerator GoToTargetPosition(float flyTime)
		{
			smokeAnimator.SetFloat("FadeTime", 1/flyTime);
			smokeAnimator.SetTrigger("FadeOut");
			shape.GetComponent<Animator>().enabled = false;

			float timer = 0;
			float timingPercent = 0;

			Vector3 targetPos = targetPosOnWin.localPosition;
			Vector3 targetScale = targetPosOnWin.localScale;

			Vector3 originalPos = shape.gameObject.transform.localPosition;
			Vector3 originalScale = shape.gameObject.transform.localScale;

			do
			{
				yield return new WaitForSeconds(0);
				timer+=Time.deltaTime;

				timingPercent = Mathf.InverseLerp(0, flyTime, timer);

				shape.gameObject.transform.localPosition = Vector3.Lerp(originalPos, targetPos, timingPercent);
				shape.gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, timingPercent);
			} while (timingPercent < 1f);

			afterWinBlock.Execute();

		}

		private void OnDisable()
		{
			if (shape)
			{
				Destroy(shape.gameObject);
				shape = null;
			}
		}

	}
}