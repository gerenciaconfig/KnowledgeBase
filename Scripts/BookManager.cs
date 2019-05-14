using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Fungus;
using Sirenix.OdinInspector;
using System.Linq;

public class BookManager : MonoBehaviour
{
    public static BookManager instance;
    [Header("Elements")]
    public Camera bookCamera;
	public Canvas bookCanvas;
	public RectTransform sysContainer;
    [SerializeField] private Flowchart gameplayFlowchart;
    //[SerializeField] private View standardView;
    private PageFlipTransition flipRenderer;
    private int currentIndex;

    [Header("Pages")]
    [SerializeField] private List<BookPage> pages;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;

    private Animator nextButtonAmtr;
    private Animator prevButtonAmtr;

    private void Awake()
    {
        instance = this;
        flipRenderer = FindObjectOfType<PageFlipTransition>();
    }

    private void Start()
    {
		bookCanvas.renderMode = RenderMode.WorldSpace;

		SkipToPage(0);

        nextButtonAmtr = nextButton.GetComponent<Animator>();
        prevButtonAmtr = prevButton.GetComponent<Animator>();

        prevButtonAmtr.SetTrigger("StartOUT");
        nextButtonAmtr.SetTrigger("StartOUT");

        startButton.onClick.AddListener(
            delegate 
            {
                GoToNextPage();
                startButton.GetComponent<Animator>().SetTrigger("PopEND");
				startButton.interactable = false;
            });

        UnityAction btAction = delegate
        {
            //GoToNextPage();
            //prevButtonAmtr.SetTrigger("Shine");
            //nextButtonAmtr.SetTrigger("Shine");
            //prevButtonAmtr.enabled = false;
            //nextButtonAmtr.enabled = false;
            nextButton.interactable = false;
            prevButton.interactable = false;
        };

        nextButton.onClick.AddListener(delegate { btAction.Invoke(); GoToNextPage(); });
        prevButton.onClick.AddListener(delegate { btAction.Invoke(); GoToPrevPage(); });

		nextButtonAmtr.SetTrigger("StartOUT");
        prevButtonAmtr.SetTrigger("StartOUT");
    }

    public void PageTellingEnded()
    {
        StopAllCoroutines();
        StartCoroutine(tellingEnded());
    }

    private IEnumerator tellingEnded()
    {
        if (currentIndex != 1)
        {
            prevButtonAmtr.SetTrigger("PopUP");
            yield return new WaitForSeconds(prevButtonAmtr.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            nextButtonAmtr.SetTrigger("PopUP");
            yield return new WaitForSeconds(nextButtonAmtr.GetCurrentAnimatorStateInfo(0).length);
        }

        nextButtonAmtr.SetTrigger("Shine");

        nextButton.interactable = true;
        prevButton.interactable = true;

		BookPage.currentPage.ResetTextElement();
    }

    public void SkipToPage(int page)
    {
        currentIndex = page;
        pages[currentIndex].EnablePage();
        pages[currentIndex].StartPage();
    }

    public void GoToNextPage()
    {
        if (currentIndex == pages.Count - 1)
            return;

        currentIndex++;
        flipRenderer.GoToNextPage(pages[currentIndex]);
    }

    public void GoToPrevPage()
    {
        if (currentIndex == 1)
            return;

        currentIndex--;
        flipRenderer.GoToPrevPage(pages[currentIndex]);

		if (currentIndex == 1)
		{
			prevButtonAmtr.SetTrigger("PopOUT");
			//nextButtonAmtr.SetTrigger("PopOUT");
		}
    }

    [Button]
    public void FindPages()
    {
        pages.Clear();

        foreach (BookPage page in FindObjectsOfType<BookPage>())
        {
            pages.Add(page);
        }

		pages = pages.OrderBy(tile => tile.gameObject.name).ToList();
	}

	public void NextTextPage()
	{
		BookPage.currentPage.NextText();
	}

}
