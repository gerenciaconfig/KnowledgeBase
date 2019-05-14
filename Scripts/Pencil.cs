using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
{
	public class Pencil : MonoBehaviour
	{
		/// <summary>
		/// The color of the pencil.
		/// </summary>
		public Color value;

		void Start(){
			GetComponent<Button> ().onClick.AddListener (() => GameObject.FindObjectOfType<UIEvents> ().PencilClickEvent (this));
		}

		/// <summary>
		/// Enable pencil selection.
		/// </summary>
		public void EnableSelection(){
			GetComponent<Animator>().SetBool("RunScale",true);
		}

		/// <summary>
		/// Disable pencil selection.
		/// </summary>
		public void DisableSelection(){
			GetComponent<Animator>().SetBool("RunScale",false);
		}
	}
}