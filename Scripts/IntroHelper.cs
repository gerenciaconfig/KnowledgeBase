using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHelper : MonoBehaviour
{

    public PolygonCollider2D col1;
    public PolygonCollider2D col2;
    public PolygonCollider2D col3;
    public PolygonCollider2D col4;
    public PolygonCollider2D col5;


    private void Start()
    {
        DisableColliders();
    }
    public void EndAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        Camera.main.GetComponent<CameraParallax>().enabled = true;
        HudManager.canvasHome.gameObject.SetActive(true);
        EnableColliders();
    }

    public void EnableColliders()
    {
        col1.enabled = true;
        col2.enabled = true;
        col3.enabled = true;
        col4.enabled = true;
        col5.enabled = true;
    }

    public void DisableColliders()
    {
        col1.enabled = false;
        col2.enabled = false;
        col3.enabled = false;
        col4.enabled = false;
        col5.enabled = false;
    }
}
