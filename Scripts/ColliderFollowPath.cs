using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.Utilities;

namespace Arcolabs.PathTracing
{
	public class ColliderFollowPath : MonoBehaviour
	{
		private TouchAreaObserver touchObserver;

		private void Start()
		{
			touchObserver = GetComponent<TouchAreaObserver>();
		}

		private void Update()
		{
			if (touchObserver.GetStatus("MouseDown"))
			{
				Debug.Log("Draggin");
				Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
				v.z = 0;
				// transform.position = v;
			}
		}
	}
}
