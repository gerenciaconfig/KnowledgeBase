using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraMovement : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float minPosX = -8.55f;
    public float maxPosX = 46f;

    private float clickTime;
    private float deltaY;
    private float intertiaVelocity;
    private float toushPosXStart;
    private float inertiaVelocity;

    public float inertiaDelay;

    private Vector3 newPosCamera;
    private Vector3 newPosBackLayer;
    private Vector3 newPosMidleLayer;

    private bool rollInertia;

    private void OnEnable()
    {
        dragSpeed *= Time.deltaTime;
    }

    void Update()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.x);

        if (rollInertia)
        {

            if (this.transform.position.x > maxPosX || this.transform.position.x < minPosX)
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
                    //print("CARALHO");
                }
            }
            else
            {
                intertiaVelocity += inertiaDelay;

                //print(" adsada  " + intertiaVelocity);
                if (intertiaVelocity >= 0)
                {
                    rollInertia = false;
                    intertiaVelocity = 0;
                    //print("BUCETA");
                }
            }

            newPosCamera = transform.position;
            newPosCamera.x -= intertiaVelocity * Time.deltaTime;


            if (this.transform.position.x < maxPosX && this.transform.position.x > minPosX)
            {
                transform.position = newPosCamera;
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            clickTime = Time.time - clickTime;
            rollInertia = true;
            deltaY = Input.mousePosition.x - toushPosXStart;

            if (Mathf.Abs(deltaY) > 10)
            {
                if (deltaY > 0)
                {
                    intertiaVelocity = (400 / clickTime + deltaY * 3) / 50;
                }
                else
                {
                    intertiaVelocity = (deltaY * 3 - 400 / clickTime) / 50;
                }
            }

        }


        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            //Guarda o tempo exato de quando clicou na tela
            clickTime = Time.time;
            //Guarda a posição do toque do momento em que clicou na tela
            toushPosXStart = Input.mousePosition.x;
            return;
        }



        if (!Input.GetMouseButton(0))
        {
            return;
        }

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.x * dragSpeed, 0, 0);

        if (move.x > 0f)
        {
            if (this.transform.position.x < maxPosX)
            {
                transform.Translate(move, Space.World);
            }
        }
        else
        {
            if (this.transform.position.x > minPosX)
            {
                transform.Translate(move, Space.World);
            }
        }
    }

    public void SetMaxPos(Vector3 newMaxPos)
    {
        maxPosX = newMaxPos.x;
    }
}
