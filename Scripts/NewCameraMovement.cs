using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{
    public Camera camera;

    Vector2 StartPosition;

    bool isZooming;

    public float multiplier;

    Vector3 newPos;

    float deltaX;
    

    public Vector3 minPos;
    public Vector3 maxPos;

    private float clickStartTime;
    private float clickStartPos;
    private float deltaClickTime;
    private float deltaClickDrag;

    private bool rollInertia;

    private float inertiaVelocity;

    public float inertiaDecreaser;

    // Update is called once per frame
    void Update()
    {
        if(rollInertia)
        {
            newPos = camera.transform.position;
            newPos.x += inertiaVelocity * Time.deltaTime;

            if (newPos.x > minPos.x && newPos.x < maxPos.x)
            {
                camera.transform.position = newPos;
            }

            if (inertiaVelocity > 0)
            {
                inertiaVelocity -= inertiaDecreaser * Time.deltaTime;

                if (inertiaVelocity <= 0)
                {
                    rollInertia = false;
                    inertiaVelocity = 0;
                }
            }
            else
            {
                inertiaVelocity += inertiaDecreaser * Time.deltaTime;

                if (inertiaVelocity >= 0)
                {
                    rollInertia = false;
                    inertiaVelocity = 0;
                }
            }
        }

        if (Input.touchCount == 0 && isZooming)
        {
            isZooming = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            StartPosition = GetWorldPosition();

            clickStartTime = Time.time;

            clickStartPos = Input.mousePosition.x;
        }

        
        if (Input.GetMouseButton(0))
        {
            deltaClickTime = Time.time - clickStartTime;

            rollInertia = false;
            //inertiaVelocity = 0;

            deltaX = -(GetWorldPosition().x - StartPosition.x);

            newPos = camera.transform.position;
            newPos.x += (deltaX * multiplier);

            if (newPos.x > minPos.x && newPos.x < maxPos.x)
            {
                camera.transform.position = newPos;
            }

            StartPosition = GetWorldPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            deltaClickTime = Time.time - clickStartTime;
            deltaClickDrag = Input.mousePosition.x - clickStartPos;

            //print("drag size " + deltaClickDrag);

            if(deltaClickTime < 0.35f && Mathf.Abs(deltaClickDrag) > 100)
            {
                rollInertia = true;

                //print("a: " + 5 / deltaClickTime);
                //print("b: " + Mathf.Abs(deltaClickDrag) / 15);

                if (deltaClickDrag < 0)
                {
                    if(inertiaVelocity < 0)
                    {
                        inertiaVelocity = 0;
                    }

                    inertiaVelocity += (3 / deltaClickTime + Mathf.Abs(deltaClickDrag)/20);
                }
                else
                {
                    if (inertiaVelocity > 0)
                    {
                        inertiaVelocity = 0;
                    }

                    inertiaVelocity += - (3 / deltaClickTime + Mathf.Abs(deltaClickDrag)/20);
                }
            }
        }
    }

    Vector2 GetWorldPosition()
    {
        return camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
    }

    public void SetMaxPos(Vector3 newMaxPos)
    {
        maxPos = newMaxPos;
    }

    public void SetMinPos(Vector3 newMinPos)
    {
        minPos = newMinPos;
    }
}