using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameProperties
{
    [Space(15)]
    public Sprite referenceImage;
    public GameObject slotImage;
    public Transform slot;
    public GameObject lineItem;
    public GameObject colItem;
}

public class MatrizGameLogic : MonoBehaviour
{
    public static MatrizGameLogic instance;

    public Game gameReference;
    [Space(10)]
    public bool playTutorial;
    public float tutorialVelocity;
    public float secondsBeforeTuto;

    [Space(10)]
    public int matchesToAdvance;
    public Image referenceImage;
    public Animator referenceImageAnimator;
    public string fadeInTrigger, fadeOutTrigger;
    public string rightSound;
    public string wrongSound;
    public string buzzSound;

    [Space(10)]
    [Range(5, 10)]
    public int randomSeed = 5;
    public List<GameProperties> gameProperties;

    private int currentProgress = 0;
    private int currentGamePropertiesIndex = 0;
    [HideInInspector]
    public GameProperties currentGameProperties;

    private void OnEnable()
    {
        instance = this;
        ResetGameProperties();
    }

    public void MakeProgress()
    {
        currentProgress++;

        if (currentProgress == matchesToAdvance)
        {
            gameReference.AddVictory(true);

            currentGameProperties.lineItem.SetActive(false);
            currentGameProperties.colItem.SetActive(false);

            currentGameProperties.lineItem.GetComponent<DragObjMatrizGame>();
            currentGameProperties.colItem.GetComponent<DragObjMatrizGame>();

            currentGameProperties.slotImage.SetActive(true);

            currentProgress = 0;
            currentGamePropertiesIndex++;
            try
            {
                currentGameProperties = gameProperties[currentGamePropertiesIndex];
            }
            catch (System.Exception)
            {
                //throw;
            }

            currentGameProperties.lineItem.transform.SetAsLastSibling();
            currentGameProperties.colItem.transform.SetAsLastSibling();

            StartCoroutine(ChangeRaferenceImage(currentGameProperties.referenceImage, 1f));
        }
    }

    public void ResetGameProperties()
    {
        RandomizeObjPosition();

        foreach (var item in gameProperties)
        {
            item.slotImage.SetActive(false);

            item.lineItem.SetActive(true);
            item.colItem.SetActive(true);

            //item.lineItem.GetComponent<DragObjMatrizGame>().ResetDragObj();
            //item.colItem.GetComponent<DragObjMatrizGame>().ResetDragObj();
        }

        currentProgress = 0;
        currentGamePropertiesIndex = 0;
        currentGameProperties = gameProperties[currentGamePropertiesIndex];

        currentGameProperties.lineItem.transform.SetAsLastSibling();
        currentGameProperties.colItem.transform.SetAsLastSibling();

        referenceImage.sprite = currentGameProperties.referenceImage;
        gameReference.ResetGame();

        if (playTutorial)
        {
            StartCoroutine(Tutorial(currentGameProperties));
        }
    }

    public void RandomizeObjPosition()
    {
        int randomNumber1;
        int randomNumber2;
        GameProperties prop1;
        GameProperties prop2;

        for (int i = 0; i < randomSeed; i++)
        {
            randomNumber1 = Random.Range(0, gameProperties.Count);
            prop1 = gameProperties[randomNumber1];

            randomNumber2 = Random.Range(0, gameProperties.Count);
            prop2 = gameProperties[randomNumber2];

            gameProperties[randomNumber1] = prop2;
            gameProperties[randomNumber2] = prop1;
        }
    }

    IEnumerator ChangeRaferenceImage(Sprite newSprite, float animDuration)
    {
        referenceImageAnimator.SetTrigger(fadeOutTrigger);
        yield return new WaitForSeconds(animDuration);
        referenceImage.sprite = newSprite;
        referenceImageAnimator.SetTrigger(fadeInTrigger);
    }

    IEnumerator Tutorial(GameProperties properties)
    {
        
        DragObjMatrizGame colDrag = properties.colItem.GetComponent<DragObjMatrizGame>();
        DragObjMatrizGame lineDrag = properties.lineItem.GetComponent<DragObjMatrizGame>();

        DragObjMatrizGame.dragState = DragObjMatrizGame.DragStates.tutorial;

        yield return new WaitForSeconds(secondsBeforeTuto + 1);

        while (colDrag.AvailableDistance() == false)
        {
            Debug.Log(DragObjMatrizGame.dragState);
            properties.colItem.transform.position = Vector3.MoveTowards(properties.colItem.transform.position, properties.slot.position, tutorialVelocity * Time.deltaTime);
            yield return null;
        }

        colDrag.CheckCorrectItem();

        DragObjMatrizGame.dragState = DragObjMatrizGame.DragStates.tutorial;

        while (lineDrag.AvailableDistance() == false)
        {
            Debug.Log(DragObjMatrizGame.dragState);
            properties.lineItem.transform.position = Vector3.MoveTowards(properties.lineItem.transform.position, properties.slot.position, tutorialVelocity * Time.deltaTime);
            yield return null;
        }

        lineDrag.CheckCorrectItem();

        DragObjMatrizGame.dragState = DragObjMatrizGame.DragStates.notDragging;
    }

    public void SetDragActive(bool active)
    {
        foreach (var item in gameProperties)
        {
            item.lineItem.GetComponent<Image>().raycastTarget = active;
            item.colItem.GetComponent<Image>().raycastTarget = active;
        }
    }
}
