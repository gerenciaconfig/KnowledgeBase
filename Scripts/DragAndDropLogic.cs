using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropLogic : MonoBehaviour
{
    public static DragAndDropLogic instance;
    public Game gameReference;
    [Space(10)]
    public string rightSound;
    public string fadeString;
    public float timeBetweenFades;

    [Range(5, 10)]
    public int randomSeed = 5;
    public List<DragAndDropObj> dragObjects;
    [Space(10)]
    public List<RectTransform> dragPivots;

    public bool anim = true;
    public bool doNotRandomize = false;
    public bool resetDragScales = false;
    public string blockName;
    public Flowchart flowChart;

    public enum GamePlate
    {
    hasPlate,
    hasNoPlate
    }

    public GamePlate gamePlate;


    [HideInInspector]

    private void OnEnable()
    {
        instance = this;
        ResetGameProperties();
        CheckAnim();
    }

    public void MakeProgress()
    {
        gameReference.AddVictory(true);
    }

    public void ResetGameProperties()
    {
        if (!doNotRandomize)
        {
            RandomizeObjPosition();
        } else
        {
            SetObjPosition();
        }
        gameReference.ResetGame();
    }

    public void ResetGamePropertiesAndButtons()
    {
        RenableButtons();
        gameReference.ResetGame();
    }

    public void RandomizeObjPosition()
    {
        int randomNumber1;
        int randomNumber2;
        DragAndDropObj drag1;
        DragAndDropObj drag2;

        for (int i = 0; i < randomSeed; i++)
        {
            randomNumber1 = Random.Range(0, dragObjects.Count);
            drag1 = dragObjects[randomNumber1];

            randomNumber2 = Random.Range(0, dragObjects.Count);
            drag2 = dragObjects[randomNumber2];

            dragObjects[randomNumber1] = drag2;
            dragObjects[randomNumber2] = drag1;
        }

        foreach (var item in dragObjects)
        {
            item.pivot = dragPivots[dragObjects.IndexOf(item)];
            item.currentSlot = null;
            if(gamePlate == GamePlate.hasPlate)
            {
                item.transform.parent.position = item.pivot.transform.position;
            }

            if(gamePlate == GamePlate.hasNoPlate)
            {
                item.transform.position = item.pivot.transform.position;
                if (resetDragScales)
                {
                    item.transform.localScale = Vector3.one;
                }
            }
         
        }
    }

    public void SetObjPosition()
    {
        for (int i = 0; i < dragObjects.Count; i++)
        {
            dragObjects[i].pivot = dragPivots[i];
            dragObjects[i].currentSlot = null;
            if (gamePlate == GamePlate.hasPlate)
            {
                dragObjects[i].transform.parent.position = dragObjects[i].transform.parent.position;
            }

            if (gamePlate == GamePlate.hasNoPlate)
            {
                dragObjects[i].transform.position = dragObjects[i].pivot.transform.position;
                if (resetDragScales)
                {
                    dragObjects[i].transform.localScale = Vector3.one;
                }
            }
        }
    }

    public void SetDragActive(bool active)
    {
        foreach (var item in dragObjects)
        {
            item.GetComponent<Image>().raycastTarget = active;
        }
    }

    public IEnumerator DragEnterAnim()
    {
        if(anim)
        {
            yield return new WaitForSeconds(timeBetweenFades);
            if (!doNotRandomize)
            {
                RandomizeObjPosition();
            }
            else
            {
                SetObjPosition();
            }

            foreach (var item in dragObjects)
            {
                yield return new WaitForSeconds(timeBetweenFades);
                item.GetComponentInParent<Animator>().SetTrigger(fadeString);
            }
        }
    }

    public void CheckAnim()
    {
        if(anim)
        {
            foreach (var item in dragObjects)
            {
              item.GetComponent<Animator>().SetTrigger("out");
            }
            StartCoroutine(DragEnterAnim());
        }
        else
        {
            foreach (var item in dragObjects)
            {
              item.GetComponent<Animator>().SetTrigger("in");
            }
        }
    }

    public void RenableButtons() //Versão de randomizebuttons que tbm reativa os botões
    {
        int randomNumber1;
        int randomNumber2;
        DragAndDropObj drag1;
        DragAndDropObj drag2;

        for (int i = 0; i < randomSeed; i++)
        {
            randomNumber1 = Random.Range(0, dragObjects.Count);
            drag1 = dragObjects[randomNumber1];

            randomNumber2 = Random.Range(0, dragObjects.Count);
            drag2 = dragObjects[randomNumber2];

            dragObjects[randomNumber1] = drag2;
            dragObjects[randomNumber2] = drag1;
        }

        foreach (var item in dragObjects)
        {
            item.pivot = dragPivots[dragObjects.IndexOf(item)];
            if (gamePlate == GamePlate.hasPlate)
            {
                item.ResetDragObj();
                item.transform.parent.position = item.pivot.transform.position;
            }

            if (gamePlate == GamePlate.hasNoPlate)
            {
                item.ResetDragObj();
                item.transform.position = item.pivot.transform.position;   
            }

            
        }
    }

    public void ChangeGameReference(Game newGameRef)
    {
        gameReference = newGameRef;
    }

    public void ExecuteBlock()
    {

    }
}
