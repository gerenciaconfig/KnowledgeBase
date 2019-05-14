using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroupController))]
public class BookPage : MonoBehaviour
{
    public static BookPage currentPage;
    public static CameraManager cameraManager;

    private CanvasGroupController cgc;

    [SerializeField] private View startView;
    [SerializeField] private BlockReference blockToExecute;
	[SerializeField] private BlockReference onOutBlock;

	[Space]
	[SerializeField] private List<PageText> texts;
	[SerializeField] private bool animatedPage;
	[SerializeField] private RectTransform gameContainer;

	private int textIndex;
	private TextMeshProUGUI uiPageText;
	private CanvasGroupController textCGC;
	private CanvasGroupController txtBGCGC;

	private bool doFadeOnText;

    private void Awake()
    {
        cgc = GetComponent<CanvasGroupController>();

		SetupPageText(gameObject);

	}

	public void SetupPageText(GameObject targetElement)
	{
		if (uiPageText != null)
		{
			txtBGCGC.transform.SetParent(txtBGCGC.transform.parent = transform);
			uiPageText = null;
		}


		if ((uiPageText = targetElement.GetComponentInChildren<TextMeshProUGUI>()) != null)
		{
			textCGC = uiPageText.GetComponent<CanvasGroupController>();
			txtBGCGC = uiPageText.transform.parent.GetComponent<CanvasGroupController>();
		}

		if (animatedPage && BookManager.instance != null)
		{
			txtBGCGC.transform.SetParent(BookManager.instance.sysContainer.transform);
			txtBGCGC.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}

		doFadeOnText = true;
	}

    private void Start()
    {
		if (!cameraManager)
            cameraManager = FungusManager.Instance.CameraManager;
    }

    public void EnablePage()
    {
        if (currentPage)
            currentPage.DisablePage();

        currentPage = this;

        transform.SetAsLastSibling();

		if (animatedPage)
		{
			txtBGCGC.transform.SetParent(BookManager.instance.sysContainer.transform);
			txtBGCGC.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		

		if (startView)
        {
            Vector3 targetPosition = startView.transform.position;
            Quaternion targetRotation = startView.transform.rotation;
            float targetSize = startView.ViewSize;

            cameraManager.PanToPosition(BookManager.instance.bookCamera, targetPosition, targetRotation, targetSize, 0, delegate { });

        }

        cgc.EnableCanvasGroup(true);

		doFadeOnText = true;
    }

	public void ResetTextElement()
	{
		if (animatedPage)
		{
			StartCoroutine(SetupEnd());
		}

		if(gameContainer != null)
			gameContainer.GetComponent<RectTransform>().SetParent(transform);

		
	}

	private IEnumerator SetupEnd()
	{
		yield return StartCoroutine(FadeText());
		txtBGCGC.transform.SetParent(txtBGCGC.transform.parent = transform);
		
	}

	public void FadeOutText()
	{
		StartCoroutine(FadeText());
	}

	private IEnumerator FadeText()
	{
		txtBGCGC.FadeCanvasGroup(false, 0.25f);
		yield return new WaitForSeconds(0.25f);
	}

    public void DisablePage()
    {
        cgc.EnableCanvasGroup(false);
		ResetPage();
		onOutBlock.Execute();
    }

    public void StartPage()
    {
		textIndex = 0;
        blockToExecute.Execute();

		if (gameContainer != null)
		{
			gameContainer.transform.SetParent(BookManager.instance.sysContainer.transform);
			gameContainer.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
	}

	public void NextText()
	{
		StopAllCoroutines();
		StartCoroutine(DrawPageText());
	}

	private IEnumerator DrawPageText()
	{
		float fadeTime = 0.3f;

		if (doFadeOnText)
		{
			uiPageText.text = texts[textIndex].text;
			txtBGCGC.FadeCanvasGroup(true, fadeTime);
			yield return new WaitForSeconds(fadeTime);
			doFadeOnText = false;
		}
		else
		{
			textCGC.FadeCanvasGroup(false, fadeTime);
			yield return new WaitForSeconds(fadeTime);
		}

		yield return new WaitForSeconds(0.1f);
		uiPageText.text = texts[textIndex].text;
		textCGC.FadeCanvasGroup(true, fadeTime);
		yield return new WaitForSeconds(fadeTime);

		textIndex++;
	}

	public void ResetPage()
	{
		if (textCGC != null)
		{
			textCGC.EnableCanvasGroup(false);
			txtBGCGC.EnableCanvasGroup(false);
		}

		if (gameContainer != null)
		{
			LayoutGroup containerGroup;

			if (containerGroup = gameContainer.GetComponent<LayoutGroup>())
			{
				containerGroup.enabled = false;
				containerGroup.enabled = true;
			}
		}
	}

}

[System.Serializable]
public class PageText
{
	[TextArea]
	public string text;
}
