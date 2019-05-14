using UnityEngine;

namespace Arcolabs.Utilities
{
	public class AspectRatioAdapter : MonoBehaviour
	{
		private float baseAspectRatio;
		private Vector3 baseScale;

		void Awake()
		{
			baseAspectRatio = 16f / 9f;
			baseScale = transform.localScale;
		}

		private void OnEnable()
		{
			float scale = (Camera.main.aspect * baseScale.x) / baseAspectRatio;
			Debug.Log("Camera aspect: " + Camera.main.aspect + " scale: " + scale + " baseAspectRatio: " + baseAspectRatio + " base scale: " + baseScale);
			transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}
