namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraMovement : MonoBehaviour
    {
        public enum MovementDirection { Vertical, Horizontal};

        public MovementDirection movementDirection;

        public float dragSpeed = 2;
        private Vector3 dragOrigin;

        public Vector3 minPos;
        public Vector3 maxPos;

        private float clickTime;
        private float movementDelta;
        private float inertiaVelocity;
        private float toushPosStart;

        public float inertiaDelay;

        private Vector3 newPosCamera;

        private bool rollInertia;

        public float inertiaPercentualLoss;

        Vector3 move;
        
        private bool CheckInsideBoundaries()
        {
            switch(movementDirection)
            {
                case MovementDirection.Horizontal:
                    if(transform.position.x > minPos.x && transform.position.x < maxPos.x)
                    {
                        return true;
                    }
                    break;

                case MovementDirection.Vertical:
                    if (transform.position.y > minPos.y && transform.position.y < maxPos.y)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void SetMaxPos(Vector3 newMaxPos)
        {
            maxPos = newMaxPos;
        }

        void Update()
        {
            //Adiciona movimento ao objeto caso a inertia ainda esteja agindo
            if (rollInertia)
            {
                //Canela a inertia caso o objeto já tenha passado das posições limites
                if (!CheckInsideBoundaries())
                {
                    rollInertia = false;
                    inertiaVelocity = 0;
                }
                else
                {
                    
                    //Caso da velocidade positiva
                    if (inertiaVelocity > 0)
                    {
                        //Diminui a inertia conforme o tempo passa, dando a sensação de "freiada"
                        //intertiaVelocity -= inertiaDelay;
                        inertiaVelocity = inertiaVelocity - inertiaVelocity * inertiaPercentualLoss;

                        //Para a inertia caso a velocidade tenha atingido 0 ou menos
                        if (inertiaVelocity <= 0)
                        {
                            rollInertia = false;
                            inertiaVelocity = 0;
                        }
                    }
                    //Caso da velocidade negativa
                    else
                    {
                        //Diminui a inertia conforme o tempo passa, dando a sensação de "freiada"
                        //intertiaVelocity += inertiaDelay;
                        inertiaVelocity = inertiaVelocity - inertiaVelocity * inertiaPercentualLoss;
                        //Para a inertia caso a velocidade tenha atingido 0 ou mais
                        if (inertiaVelocity >= 0)
                        {
                            rollInertia = false;
                            inertiaVelocity = 0;
                        }
                    }


                    
                    Vector3 moveInertia = new Vector3(inertiaVelocity, 0, 0);
                    transform.Translate(-moveInertia);
                }
            }


            if (Input.GetMouseButtonUp(0))
            {
                clickTime = Time.time - clickTime;

                switch (movementDirection)
                {
                    case MovementDirection.Horizontal:
                        movementDelta = Input.mousePosition.x - dragOrigin.x;
                        break;

                    case MovementDirection.Vertical:
                        movementDelta = Input.mousePosition.y - dragOrigin.y;
                        break;
                }

                //Se o movimento for maior que o mínimo parar rolar inertia
                if (Mathf.Abs(movementDelta) > 15)
                {
                    rollInertia = true;
                    if (movementDelta > 0)
                    {
                        inertiaVelocity = dragSpeed * (50 / clickTime + movementDelta/2) / 1000;
                    }
                    else
                    {
                        inertiaVelocity = dragSpeed * (movementDelta/2 - 50 / clickTime) / 1000;
                    }

                    inertiaVelocity *= 2.5f;
                }
            }

            //Momento em que o botão foi apertado
            if (Input.GetMouseButtonDown(0))
            {        
                //Guarda a posição do toque do momento em que clicou na tela
                dragOrigin = Input.mousePosition;
                //Guarda o tempo exato de quando clicou na tela
                clickTime = Time.time;

                return;
            }

            if (!Input.GetMouseButton(0))
            {
                return;
            }


            if ((Time.time - clickTime) > 0.2f)
            {
                rollInertia = false;
            }

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

            if(movementDirection == MovementDirection.Horizontal)
            {
                move = new Vector3(-pos.x * dragSpeed, 0, 0);
                
                if (move.x > 0f)
                {
                    if (this.transform.position.x < maxPos.x)
                    {
                        transform.Translate(move, Space.World);
                    }
                }
                else
                {
                    if (this.transform.position.x > minPos.x)
                    {
                        transform.Translate(move, Space.World);
                    }
                }
            }
            else
            {
                move = new Vector3(0, -pos.y * dragSpeed, 0);

                if (move.y > 0f)
                {
                    if (this.transform.position.y < maxPos.y)
                    {
                        transform.Translate(move, Space.World);
                    }
                }
                else
                {
                    if (this.transform.position.y > minPos.y)
                    {
                        transform.Translate(move, Space.World);
                    }
                }
            }
        }
    }
}
