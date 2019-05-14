using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
	private Vector2 touchOrigin = -Vector2.one;
	private bool touch;
	public Vector3 initialPosition;
	public Vector3 finalPosition;

    public float velocity = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // if (Input.touchCount > 0)
		{
			Vector3 pos = Input.mousePosition;
			if (Input.GetMouseButtonDown(0))
			{
				touch = true;
				touchOrigin = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(0))
			{
				touch = false;
			}
			if (touch)
			{
				if (Input.mousePosition.x > touchOrigin.x)
				{
					transform.Translate(Vector3.right * velocity);
				}
				else
				{
					transform.Translate(Vector3.left * velocity);
				}
			}
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, initialPosition.x, finalPosition.x), transform.position.y, transform.position.z);
			//Debug.Log("touche!");
			//Touch touchy = Input.touches[0];

			//if (touchy.phase == TouchPhase.Began)
			//{
			//	Debug.Log("Touch started!");
			//	touchOrigin = touchy.position;
			//	touch = true;
			//}
			//else if (touchy.phase == TouchPhase.Moved)
			//{

			//}
			//else if (touchy.phase == TouchPhase.Ended)
			//{
			//	Debug.Log("Quit touching me!");
			//	touch = false;
			//}

			//if (touch)
			//{
			//	Debug.Log("You should be moving!!!");
			//	if (touchy.position.x > touchOrigin.x)
			//	{
			//		transform.Translate(Vector3.right);
			//	}
			//	else
			//	{
			//		transform.Translate(Vector3.left);
			//	}
			//}
		}
    }
}
