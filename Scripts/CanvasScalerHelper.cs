using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerHelper : MonoBehaviour
{
    Camera cam;
    public GameObject[] canvasScalerObjects;

    // Start is called before the first frame update
    void Start()
    {

        cam = Camera.main;
        canvasScalerObjects =  GameObject.FindGameObjectsWithTag("Canvas");

        if (cam.aspect<1.7f)
        {
            foreach (GameObject item in canvasScalerObjects)
            {
                item.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            }
        }
        else
        {
            foreach (GameObject item in canvasScalerObjects)
            {
                item.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            }
        }
        
    }

// Update is called once per frame
void Update()
    {
        
    }
}
