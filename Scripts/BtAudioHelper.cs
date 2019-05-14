using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtAudioHelper : MonoBehaviour
{
    private Animator animatorConfig;
    public string checkPrefs;

    private void OnEnable()
    {
        animatorConfig = this.gameObject.GetComponent<Animator>();
        if (PlayerPrefs.GetInt(checkPrefs) == 0)
        {
            animatorConfig.SetInteger("state", 2);
        }
        if (PlayerPrefs.GetInt(checkPrefs) == 1)
        {
            animatorConfig.SetInteger("state", 1);
        }
    }

    public void ChangeState()
    {
        if (animatorConfig.GetInteger("state") == 0 || animatorConfig.GetInteger("state") == 2)
        {
            animatorConfig.SetInteger("state", 1);
        }
        else
        {
            if (animatorConfig.GetInteger("state") == 1)
            {
                animatorConfig.SetInteger("state", 2);
            }
        }

    }
}
