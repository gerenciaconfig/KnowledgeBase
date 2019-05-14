using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectManager : MonoBehaviour
{
    public List<NewButtonBehaviour> buttonsList;

    public bool sortPositions;

    private List<Vector2> positionPivots;

    private List<Animator> buttonsAnimators;

    List<RectTransform> buttonsRects;

    public float waitTimeBetwenButtonFade;

    [SerializeField] private bool useAnchorOnSort = false;

	// Use this for initialization
	void OnEnable ()
    {
        if (sortPositions)
        {
            SortButtonsOrder();
        }
    }

    private void Awake()
    {
        SetPivotsPositions();
    }

    public void SortButtonsOrder()
    {
        //Lista contendo o rect de cada botão
        buttonsRects = new List<RectTransform>();
        positionPivots = new List<Vector2>();

        buttonsAnimators = new List<Animator>();

        List<Vector2> minAnchorPositions = new List<Vector2>();
        List<Vector2> maxAnchorPositions = new List<Vector2>();

        //Popula as listas
        for (int i = 0; i < buttonsList.Count; i++)
        {
            if (!useAnchorOnSort)
            {
                positionPivots.Add(buttonsList[i].GetComponent<RectTransform>().anchoredPosition);
                positionPivots.Add(buttonsList[i].GetComponent<RectTransform>().localPosition);
            }
            else
            {
                minAnchorPositions.Add(buttonsList[i].GetComponent<RectTransform>().anchorMin);
                maxAnchorPositions.Add(buttonsList[i].GetComponent<RectTransform>().anchorMax);
            }

            buttonsRects.Add(buttonsList[i].GetComponent<RectTransform>());

        }

        //Sorteia a lista de ordem
        for (int i = 0; i < buttonsList.Count; i++)
        {
            int aux = Random.Range(0, buttonsRects.Count);

            if (!useAnchorOnSort)
            {
                buttonsRects[aux].anchoredPosition = positionPivots[i];
                buttonsRects[aux].localPosition = positionPivots[i];
            }
            else
            {
                buttonsRects[aux].anchorMin = minAnchorPositions[i];
                buttonsRects[aux].anchorMax = maxAnchorPositions[i];
            }

            buttonsAnimators.Add(buttonsRects[aux].GetComponent<Animator>());

            buttonsRects.RemoveAt(aux);
        }        
    }

    private void SetPivotsPositions()
    {
        //Cria uma nova lista com as posições originais de cada botão
        positionPivots = new List<Vector2>();

        buttonsAnimators = new List<Animator>();

        //Popula as listas
        for (int i = 0; i < buttonsList.Count; i++)
        {
            positionPivots.Add(buttonsList[i].GetComponent<RectTransform>().anchoredPosition);
            buttonsAnimators.Add(buttonsList[i].GetComponent<Animator>());
        }
    }

    public void FadeInButtons()
    {
        StartCoroutine(FadeInButtonsCoroutine());
    }

    private IEnumerator FadeInButtonsCoroutine()
    {
        for (int i = 0; i < buttonsAnimators.Count; i++)
        {
            buttonsAnimators[i].SetTrigger("fadeIn");
            yield return new WaitForSeconds(waitTimeBetwenButtonFade);
        }
    }

    public void FadeOutButtons()
    {
        for (int i = 0; i < buttonsAnimators.Count; i++)
        {
            buttonsAnimators[i].SetTrigger("fadeOut");
        }
    }

    private IEnumerator FadeOutButtonsCoroutine()
    {
        for (int i = 0; i < buttonsAnimators.Count; i++)
        {
            buttonsAnimators[i].SetTrigger("fadeOut");
            yield return new WaitForSeconds(waitTimeBetwenButtonFade);
        }
    }
}
