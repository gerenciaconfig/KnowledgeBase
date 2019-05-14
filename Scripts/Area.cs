using UnityEngine;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
	{
	[DisallowMultipleComponent]
	public class Area : MonoBehaviour
	{
			/// <summary>
			/// Area animator.
			/// </summary>
			private static Animator AreaAnimator;

			// Use this for initialization
			void Awake ()
			{
					///Setting up the references
					if (AreaAnimator == null) {
							AreaAnimator = GetComponent<Animator> ();
					}
			}

			/// <summary>
			/// When the GameObject becomes visible
			/// </summary>
			void OnEnable ()
			{
					///Hide the Area
					Hide ();
			}

			///Show the Area
			public static void Show ()
			{
					if (AreaAnimator == null) {
							return;
					}
					AreaAnimator.SetTrigger ("Running");
			}
			///Hide the Area
			public static void Hide ()
			{
				if(AreaAnimator!=null)
					AreaAnimator.SetBool ("Running", false);
			}
	}
}