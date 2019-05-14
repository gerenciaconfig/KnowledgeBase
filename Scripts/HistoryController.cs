using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Page
{
    public GameObject pageObj;
    public Animator pageAnimator;
    public List<string> animatorStateId;
}

public class HistoryController : MonoBehaviour
{
    public FadeControllerHist fadeController;
    public float fadeChangeTime;
    public List<Page> pageList;

    public UnityEvent OnHistoryFinished;
    public UnityEvent OnRegressAtZeroIndex;

    int pageIndex = 0;
    int pageAnimatorIndex = 0;

    private void OnEnable()
    {
        StartHistory();
    }

    public void Advance()
    {
        StartCoroutine(AdvanceCoroutine());
    }

    public void Regress()
    {
        StartCoroutine(RegressCoroutine());
    }

    IEnumerator AdvanceCoroutine()
    {
        pageAnimatorIndex++;

        //Passando de página pra frente
        if (pageAnimatorIndex == pageList[pageIndex].animatorStateId.Count)
        {
            Page lastPage = pageList[pageIndex];

            pageIndex++;

            if (pageIndex < pageList.Count)
            {
                fadeController.gameObject.SetActive(true);
                yield return new WaitForSeconds(fadeChangeTime);
                lastPage.pageObj.SetActive(false);
                pageList[pageIndex].pageObj.SetActive(true);
            }
            else
            {
                fadeController.gameObject.SetActive(true);
                yield return new WaitForSeconds(fadeChangeTime);
                lastPage.pageObj.SetActive(false);
                OnHistoryFinished.Invoke();
                yield break;
            }
            /*
            try
            {
                pageList[pageIndex].pageObj.SetActive(true);
            }
            catch
            {
                OnHistoryFinished.Invoke();
                return null;
            }
            */

            pageAnimatorIndex = 0;
        }

        PlayAnimation(pageIndex, pageAnimatorIndex);
    }

    IEnumerator RegressCoroutine()
    {
        pageAnimatorIndex--;

        //Passando de página para trás
        if (pageAnimatorIndex < 0)
        {
            Page lastPage = pageList[pageIndex];
            pageIndex--;

            if (pageAnimatorIndex < 0 && pageIndex >= 0)
            {
                fadeController.gameObject.SetActive(true);
                yield return new WaitForSeconds(fadeChangeTime);
                lastPage.pageObj.SetActive(false);
                pageList[pageIndex].pageObj.SetActive(true);
            }
            else if (pageIndex < 0 && pageAnimatorIndex < 0)
            {
                fadeController.gameObject.SetActive(true);
                yield return new WaitForSeconds(fadeChangeTime);
                lastPage.pageObj.SetActive(false);
                OnRegressAtZeroIndex.Invoke();
                yield break;
            }

            /*
            try
            {
                pageList[pageIndex].pageObj.SetActive(true);
            }
            catch
            {
                OnRegressAtZeroIndex.Invoke();
                return null;
            }
            */

            pageAnimatorIndex = pageList[pageIndex].animatorStateId.Count - 1;
        }

        PlayAnimation(pageIndex, pageAnimatorIndex);
    }

    public void StartHistory()
    {
        fadeController.gameObject.SetActive(false);
        pageIndex = 0;
        pageAnimatorIndex = 0;

        //Foreach para desativar todas as páginas e ativar a primeira
        foreach (var item in pageList)
        {
            item.pageObj.SetActive(false);
        }

        pageList[0].pageObj.SetActive(true);

        PlayAnimation(pageIndex, pageAnimatorIndex);
    }

    public void PlayAnimation(int pageIndex, int animIndex)
    {
        AudioManager.instance.StopAllSounds();
        pageList[pageIndex].pageAnimator.Play(pageList[pageIndex].animatorStateId[animIndex], -1, 0);
    }
}