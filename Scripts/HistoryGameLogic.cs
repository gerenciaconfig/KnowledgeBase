using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class HistoryPageProperties
{
    [Space(10)]
    public GameObject pageImage;
    public Animator pageAnimator;
    public string pageAudio;
}

public class HistoryGameLogic : MonoBehaviour
{

    public Button previousButton;
    public Button nextButton;
    public Button audioHistoryButton;

    [Space(10)]
    public List<HistoryPageProperties> pageList;

    [Space(10)]
    public UnityEvent OnHistoryFinished;

    HistoryPageProperties currentPage;
    int pageIndex = 0;
    const string fadeOut = "fade";

    public int PageIndex
    {
        get
        {
            return pageIndex;
        }

        set
        {
            pageIndex = value;

            if (value == pageList.Count)
            {
                OnHistoryFinished.Invoke();
            }
        }
    }

    public enum FadeType
    {
        PreviousPage,
        NextPage
    }

    private void OnEnable()
    {
        nextButton.interactable = false;
        PageIndex = 0;
        ValidatePageIndex();

        currentPage = pageList[PageIndex];

        //Deactivate all images
        foreach (var item in pageList)
        {
            item.pageImage.SetActive(false);
        }

        //Activate current image
        currentPage.pageImage.SetActive(true);
    }

    void ValidatePageIndex()
    {
        //PreviousButtonCase
        if (pageIndex <= 0)
        {
            previousButton.interactable = false;
            previousButton.GetComponent<Image>().enabled = false;
            foreach (Image image in previousButton.GetComponentsInChildren<Image>())
            {
                image.enabled = false;
            }
        }
        else
        {
            previousButton.interactable = true;
            previousButton.GetComponent<Image>().enabled = true;
            foreach (Image image in previousButton.GetComponentsInChildren<Image>())
            {
                image.enabled = true;
            }
        }
    }

    public void PreviousPage()
    {
        StartCoroutine(FadePageImage(FadeType.PreviousPage));
    }

    public void NextPage()
    {
        StartCoroutine(FadePageImage(FadeType.NextPage));
    }

    public void PlayCurrentPageAudio()
    {
        StartCoroutine(PlayCurrentPageAudioCoroutine());
    }

    IEnumerator PlayCurrentPageAudioCoroutine()
    {
        previousButton.interactable = false;
        nextButton.interactable = false;
        audioHistoryButton.interactable = false;

        AudioManager.instance.PlayAudioDescription(currentPage.pageAudio);
        yield return new WaitForSeconds(AudioManager.instance.GetAudioSource(currentPage.pageAudio).clip.length);

        nextButton.interactable = true;
        audioHistoryButton.interactable = true;

        ValidatePageIndex();
    }

    IEnumerator FadePageImage(FadeType _fadeType)
    { 
        previousButton.interactable = false;
        nextButton.interactable = false;
        audioHistoryButton.interactable = false;

        //Fade out page image
        if (currentPage.pageAnimator != null)
        {
            currentPage.pageAnimator.SetTrigger(fadeOut);
            yield return new WaitForSeconds(currentPage.pageAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        currentPage.pageImage.SetActive(false);

        if (_fadeType == FadeType.PreviousPage && PageIndex > 0)
        {
            PageIndex--;
            ValidatePageIndex();
        }
        else if (_fadeType == FadeType.NextPage)
        {
            PageIndex++;
            ValidatePageIndex();
        }

        try
        {
            currentPage = pageList[PageIndex];
            currentPage.pageImage.SetActive(true);
        }
        catch
        {
            yield break;
        }
        
        yield return new WaitForSeconds(currentPage.pageAnimator.GetCurrentAnimatorStateInfo(0).length);

        PlayCurrentPageAudio();
    }
}
