using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericParameter : MonoBehaviour 
{
	private GameObject parameter;

    private GameObject parameter2;

    public void SetParameter(GameObject parameter)
    {
        this.parameter = parameter;
    }

    public void SetParameter2(GameObject parameter)
    {
        this.parameter2 = parameter;
    }

    public GameObject GetParameter(int index)
    {
        if(index == 0)
        {
            return parameter;
        }
        else
        {
            return parameter2;
        }

    }
}