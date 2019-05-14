using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Disney.Patterns
{
	public class GameController : MonoBehaviour
	{
		protected PointAndClickController controller;
		public PointAndClickController Controller { get { return controller; } set { controller = value; } }

		protected GameObject itemsContainer;
		public GameObject ItemsContainer { get { return itemsContainer; } set { itemsContainer = value; } }

		public static void SnapAnchors(GameObject g)
		{
			RectTransform recTransform = null;
			RectTransform parentTransform = null;

			if (g.transform.parent != null)
			{
				if (g.GetComponent<RectTransform>() != null)
					recTransform = g.GetComponent<RectTransform>();
				else
					return;

				if (parentTransform == null)
					parentTransform = g.transform.parent.GetComponent<RectTransform>();

				Vector2 offsetMin = recTransform.offsetMin;
				Vector2 offsetMax = recTransform.offsetMax;
				Vector2 anchorMin = recTransform.anchorMin;
				Vector2 anchorMax = recTransform.anchorMax;
				Vector2 parent_scale = new Vector2(parentTransform.rect.width, parentTransform.rect.height);
				recTransform.anchorMin = new Vector2(anchorMin.x + (offsetMin.x / parent_scale.x), anchorMin.y + (offsetMin.y / parent_scale.y));
				recTransform.anchorMax = new Vector2(anchorMax.x + (offsetMax.x / parent_scale.x), anchorMax.y + (offsetMax.y / parent_scale.y));
				recTransform.offsetMin = Vector2.zero;
				recTransform.offsetMax = Vector2.zero;
			}
		}
	}
}
