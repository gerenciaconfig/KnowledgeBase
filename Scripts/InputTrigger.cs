using UnityEngine;
using UnityEngine.Events;

namespace Arcolabs.Utilities
{

	public class InputTrigger : MonoBehaviour
	{
		[SerializeField]
		private Transform targetTransform;

		[SerializeField]
		private bool triggerOnce = true;
		[SerializeField]
		private UnityEvent callback;

		private bool triggered = false;

		[SerializeField]
		private float distance = .2f;

		private void OnEnable()
		{
			triggered = false;
		}

		private bool inputDragging = false;

		private void Update()
		{
			float dst = Vector2.Distance(targetTransform.position, transform.position);
			if (dst < distance)
			{
				TriggerCallbacks(true);
			}
		}

		public void TriggerCallbacks(bool onMouseAction)
		{
			if (onMouseAction && (!triggered || !triggerOnce))
			{
				Debug.Log("Invoked Callbacks");
				triggered = true;
				callback.Invoke();
			}
		}
	}
}
