using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    public UnityEvent OnTapeteMatched;//Add +1 no MuppetsRimarGameController.instance.tapeteCount caso não tenha adicionado ainda;
    public UnityEvent OnRimaWin;
    //public string charSound;
    public bool isTapeteMatched;
    public bool isRimaWin;
    public Transform finalPosition;
    public Image checkImage;
    public float velocity = 200;
    public GameObject rimaPanel;
    public GameObject whiteBorder;
    public GameObject purpleBorder;

    public RectTransform initialPivot;

    public enum CharState
    {
        Waiting
    }

    private void Awake()
    {
        try
        {
            MuppetsRimarGameController.instance.OnGameReset.AddListener(ResetThisChar);
        }
        catch (System.Exception)
        {

            //throw;
        }

    }

    private void OnEnable()
    {
        this.GetComponent<RectTransform>().anchoredPosition = initialPivot.anchoredPosition;

        ResetThisChar();
    }

    void ResetThisChar()
    {
        whiteBorder.SetActive(false);
        purpleBorder.SetActive(false);
        isTapeteMatched = false;
        isRimaWin = false;
        checkImage.enabled = false;
    }

    public void AddTapeteVictory()
    {
        if (isTapeteMatched == false)
        {
            MuppetsRimarGameController.instance.tapeteCount++;
            OnTapeteMatched.Invoke();
            isTapeteMatched = true;

            if (MuppetsRimarGameController.instance.tapeteCount == MuppetsRimarGameController.instance.tapeteTarget)
            {
                MuppetsRimarGameController.instance.OnTapeteComplete.Invoke();
                StartCoroutine(IntroRimaCoroutine());
            }
        }
    }

    public void AddRimaVictory()
    {
        if (isRimaWin == false)
        {
            MuppetsRimarGameController.instance.rimaCount++;
            Debug.Log(MuppetsRimarGameController.instance.rimaTarget + "-----" + MuppetsRimarGameController.instance.rimaCount);
            OnRimaWin.Invoke();
            isRimaWin = true;

            if (MuppetsRimarGameController.instance.rimaCount != MuppetsRimarGameController.instance.rimaTarget)
            {
                AudioManager.instance.PlaySound(MuppetsRimarGameController.instance.chooseAnotherMuppet);
            }

            if (MuppetsRimarGameController.instance.rimaCount == MuppetsRimarGameController.instance.rimaTarget)
            {
                MuppetsRimarGameController.instance.OnFinishedAllRimas.Invoke();
                Debug.Log(MuppetsRimarGameController.instance.rimaTarget + "-----" + MuppetsRimarGameController.instance.rimaCount);

            }
        }
    }

    public void FloorClick()
    {
        if (this != MuppetsRimarGameController.instance.currentChar && !isTapeteMatched)
        {
            AudioManager.instance.StopAllSounds();
            AudioManager.instance.PlaySound("Buzz");
            AudioManager.instance.PlayRandomFailSound();
        }
        else if (this == MuppetsRimarGameController.instance.currentChar && !isTapeteMatched)
        {
            //Movo o char até seu ponto final
            transform.SetAsLastSibling();
            StartCoroutine(MoveCharToFinalPosition());
        }
    }

    public void CharClick()
    {
        //StartCoroutine(CharClickCoroutine());
    }

    IEnumerator MoveCharToFinalPosition()
    {
        MuppetsRimarGameController.instance.SetClickActive(false);

        while (Vector2.Distance(transform.localPosition, finalPosition.localPosition) > 0.1)
        {
            this.transform.localPosition = Vector2.MoveTowards(this.transform.localPosition, finalPosition.localPosition, Time.deltaTime * velocity);
            yield return null;
        }

        AddTapeteVictory();
        if (MuppetsRimarGameController.instance.tapeteCount != MuppetsRimarGameController.instance.tapeteTarget)
        {
            AudioManager.instance.StopAllSounds();
            AudioManager.instance.PlayRandomSuccessSound();
        }
        else
        {
            AudioManager.instance.PlayRandomSuccessSound();
        }

        checkImage.enabled = true;

        yield return new WaitForSeconds(1);

        MuppetsRimarGameController.instance.SetClickActive(true);
        MuppetsRimarGameController.instance.SelectRandomGame();

        yield return null;
    }
    /*
    IEnumerator CharClickCoroutine()
    {
        if (MuppetsRimarGameController.instance.tapeteCount == MuppetsRimarGameController.instance.tapeteTarget)
        {
            foreach (var item in MuppetsRimarGameController.instance.charList)
            {
                item.GetComponent<Button>().interactable = false;
            }

            AudioManager.instance.StopAllSounds();
            AudioManager.instance.PlaySound(charSound);
            yield return new WaitForSeconds(AudioManager.instance.GetAudioSource(charSound).clip.length);
            FadeController.instance.SetOpen(rimaPanel);
            MuppetsRimarGameController.instance.currentRimaChar = this;
            FadeController.instance.FadeScreen(FadeController.FadeTypes.fadeOpen);

            foreach (var item in MuppetsRimarGameController.instance.charList)
            {
                item.GetComponent<Button>().interactable = true;
            }
        }
    }
    */
    IEnumerator IntroRimaCoroutine()
    {
        Debug.Log("Entrou aqui");
        foreach (var item in MuppetsRimarGameController.instance.charList)
        {
            item.GetComponent<Button>().interactable = false;
        }

        yield return new WaitForSeconds(2);

        MuppetsRimarGameController.instance.gameFlow.ExecuteBlock("ChangeFirstGame");

        yield return new WaitForSeconds(1);

        foreach (var item in MuppetsRimarGameController.instance.charList)
        {
            item.GetComponent<Button>().interactable = true;
        }
    }
}
