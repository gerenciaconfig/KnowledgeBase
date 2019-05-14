using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMovement : MonoBehaviour
{
    public Transform scrollTransform;


    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    private float minPosY = 0f;
    private float maxPosY = 460f;

    //Inertia Variables
    private float clickTime;
    private float deltaY;
    private float intertiaVelocity;
    private float touchPosYStart;
    private float inertiaVelocity;

    public float inertiaDelay;

    private Vector3 newPosCamera;
    private Vector3 newPosBackLayer;
    private Vector3 newPosMidleLayer;

    private bool rollInertia;

    private bool canDrag;

    private void Start()
    {
        canDrag = true;
        dragSpeed *= Time.deltaTime;
    }

    public void SetCanDrag(bool newValue)
    {
        canDrag = newValue;
        rollInertia = false;
    }

    public void SetBoundaries(float maxPosY)
    {
        this.maxPosY = maxPosY;

        if(scrollTransform.position.y > maxPosY)
        {
            scrollTransform.position = new Vector3(scrollTransform.position.x, maxPosY, scrollTransform.position.z);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (rollInertia && canDrag)
        {
            if (scrollTransform.position.y > maxPosY || scrollTransform.position.y < minPosY)
            {
                rollInertia = false;
                intertiaVelocity = 0;
            }

            if (intertiaVelocity > 0)
            {
                intertiaVelocity -= inertiaDelay;

                if (intertiaVelocity <= 0)
                {
                    rollInertia = false;
                    intertiaVelocity = 0;
                }
            }
            else
            {
                intertiaVelocity += inertiaDelay;

                if (intertiaVelocity >= 0)
                {
                    rollInertia = false;
                    intertiaVelocity = 0;
                }
            }

            newPosCamera = scrollTransform.position;

            newPosCamera.y += intertiaVelocity * Time.deltaTime;


            if (scrollTransform.position.y < maxPosY && scrollTransform.position.y > minPosY)
            {
                scrollTransform.position = newPosCamera;
            }
        }
    }

    public void StartDragFromOutside(Vector3 origin, float clickTime)
    {
        dragOrigin = origin;
        this.clickTime = clickTime;
        touchPosYStart = origin.y;

        canDrag = true;
    }

    private void OnMouseDown()
    {
        if(canDrag)
        {
            dragOrigin = Input.mousePosition;

            rollInertia = false;

            //Guarda o tempo exato de quando clicou na tela
            clickTime = Time.time;
            //Guarda a posição do toque do momento em que clicou na tela
            touchPosYStart = Input.mousePosition.y;

            //Zera a velocidade da inercia 
            intertiaVelocity = 0;
        }
        
    }

    private void OnMouseDrag()
    {
        if(canDrag)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(0, pos.y * dragSpeed, 0);

            if (move.y > 0f)
            {
                if (scrollTransform.position.y < maxPosY)
                {
                    scrollTransform.Translate(move, Space.World);
                }
            }
            else
            {
                if (scrollTransform.position.y > minPosY)
                {
                    scrollTransform.Translate(move, Space.World);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        clickTime = Time.time - clickTime;
        rollInertia = true;
        deltaY = Input.mousePosition.y - touchPosYStart;

        if (Mathf.Abs(deltaY) > 25)
        {
            if (deltaY > 0)
            {
                intertiaVelocity = dragSpeed * (30 / clickTime + deltaY / 1.5f) / 10;
            }
            else
            {
                intertiaVelocity = dragSpeed * (deltaY / 1.5f - 30 / clickTime) / 10;
            }
        }
    }
}
