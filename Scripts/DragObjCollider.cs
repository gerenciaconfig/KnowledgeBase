using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjCollider : MonoBehaviour
{
    private void OnMouseDrag()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    private void OnMouseEnter()
    {
        Debug.Log("Tocou");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("slote"))
        {

        }
    }
}
