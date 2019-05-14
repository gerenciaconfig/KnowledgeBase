using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MuppetsRimarGameController : MonoBehaviour
{
    public static MuppetsRimarGameController instance;
    public UnityEvent OnStartGame;
    public UnityEvent OnTapeteComplete; //Todos os personagens no tapete -> Libera o jogo da rima;
    public UnityEvent OnFinishedAllRimas; //Todos os personagens terminaram a rima -> condição de final de jogo.

    [HideInInspector]
    public UnityEvent OnGameReset;
    public string chooseAnotherMuppet;
    public string introGame;
    public string introRima;
    public string introRima2;
    public Flowchart gameFlow;
    public int tapeteTarget = 6;
    public int rimaTarget = 6;

    [HideInInspector]
    public int tapeteCount = 0;
    [HideInInspector]
    public int rimaCount = 0;

    public List<CharController> charList;
    public List<Button> floorButtonList;

    private List<CharController> manipulatedList = new List<CharController>();
    public GameObject purplePanel;
    public CharController currentChar;
    public CharController currentRimaChar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnStartGame.Invoke();
        ResetGame();
        StartCoroutine(StartGame());
        //currentChar.whiteBorder.SetActive(true);
    }

    private void OnDisable()
    {
        currentChar = null;
    }

    void ResetGame()
    {
        tapeteCount = 0;
        rimaCount = 0;
        OnGameReset.Invoke();
        SetClickActive(true);
        manipulatedList.Clear();
        manipulatedList.AddRange(charList);
        purplePanel.SetActive(true);
    }

    public void SelectRandomGame()
    {
        try
        {
            currentChar.whiteBorder.SetActive(false);
        }
        catch (System.Exception)
        {

            //throw;
        }

        if (manipulatedList.Count > 0)
        {
            currentChar = manipulatedList[Random.Range(0, manipulatedList.Count)];
            currentChar.whiteBorder.SetActive(true);
            manipulatedList.Remove(currentChar);
        }
    }

    public void SetClickActive(bool active)
    {
        foreach (var item in charList)
        {
            item.finalPosition.GetComponent<Image>().raycastTarget = active;
        }
    }

    IEnumerator StartGame()
    {
        foreach (var item in floorButtonList)
        {
            item.interactable = false;
        }

        AudioManager.instance.PlayAudioDescription(introGame);
        yield return new WaitForSeconds(AudioManager.instance.GetAudioSource(introGame).clip.length);

        SelectRandomGame();

        foreach (var item in floorButtonList)
        {
            item.interactable = true;
        }
    }
}
