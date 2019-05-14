using UnityEngine;
using Arcolabs.General;
using Arcolabs.Managers;

namespace Arcolabs.Utilities
{
	/// <summary>
	/// Sends information about mouse clicks
	/// </summary>
	public class TouchAreaObserver : ObservableItem
	{
		[SerializeField]
		private IManager manager;

		private void Start()
		{
			manager = GetComponentInParent<LineTracingManager>();
			status.Add("MouseDown", false);
			status.Add("MouseOver", false);
			status.Add("MouseDrag", false);
		}

		private void OnMouseDown()
		{
			Debug.Log("Pressed!");
			NotifyManager("MouseDown", true);
		}

		private void OnMouseDrag()
		{
			Debug.Log("Draggin!");
			NotifyManager("MouseDrag", true);
		}

		private void OnMouseOver()
		{
			NotifyManager("MouseOver", true);
		}

		private void OnMouseUp()
		{
			Debug.Log("Released");
			NotifyManager("MouseDown", false);
		}

		private void OnDisable()
		{
			status["MouseDown"] = false;
			status["MouseOver"] = false;
			status["MouseDrag"] = false;
		}

		private void NotifyManager(string name, bool b)
		{
			status[name] = b;
			if (manager == null) return;
			manager.Notify(this);
		}
	}
}
