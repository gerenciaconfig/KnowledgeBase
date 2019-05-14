using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtConfigHelper : MonoBehaviour
{
    Camera cam;
    private int menuState;
    private Animator animatorConfig;
    public bool clicked = false;

    private void Awake()
    {
        cam = Camera.main;   
    }

    private void OnEnable()
    {
        animatorConfig = this.gameObject.GetComponent<Animator>();
        animatorConfig.SetInteger("state", 0);
    }

    public void ChangeState()
    {
        if (animatorConfig.GetInteger("state") == 0 || animatorConfig.GetInteger("state") == 2)
        {
            animatorConfig.SetInteger("state", 1);
            cam.GetComponent<IntroHelper>().DisableColliders();
        }
        else
        {
            if (animatorConfig.GetInteger("state") == 1)
            {
                animatorConfig.SetInteger("state", 2);
                if (!clicked)
                {
                    cam.GetComponent<IntroHelper>().EnableColliders();
                }
                
            }
        }

    }

    public void ChangeClicked()
    {
        clicked = clicked ? clicked = false : clicked = true;
    }
}
