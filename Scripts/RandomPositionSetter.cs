using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionSetter : MonoBehaviour
{
    [Range(5, 10)]
    public int randomSeed = 5;

    [Space(10)]
    public List<RectTransform> pivotList;
    public List<RectTransform> objList;

    public void RandomizeObjPosition()
    {
        int randomNumber1;
        int randomNumber2;
        RectTransform rect1;
        RectTransform rect2;

        for (int i = 0; i < randomSeed; i++)
        {
            randomNumber1 = Random.Range(0, objList.Count);
            rect1 = objList[randomNumber1];

            randomNumber2 = Random.Range(0, objList.Count);
            rect2 = objList[randomNumber2];

            objList[randomNumber1] = rect2;
            objList[randomNumber2] = rect1;
        }

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.position = pivotList[i].transform.position;
        }

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetSiblingIndex(pivotList[i].GetSiblingIndex());
        }
    }

    public void NewRandomize()
    {
        /*
        List<RectTransform> pivotListAux = new List<RectTransform>();
        pivotListAux.AddRange(pivotList);

        for (int i = 0; i < 20; i++)
        {
            int index = Random.Range(0, pivotListAux.Count);


            print(pivotListAux[index].anchoredPosition);
            objList[i].position = pivotListAux[index].position;
            pivotListAux.RemoveAt(index);
        }*/



        for (int i = 0; i < 20; i++)
        {
            int index = Random.Range(0, objList.Count);

            objList[index].SetAsLastSibling();
        }
    }
}
