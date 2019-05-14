using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
namespace Letra
{
	[DisallowMultipleComponent]
	public class WinDialog : MonoBehaviour
	{
		/// <summary>
		/// Number of stars for the WinDialog.
		/// </summary>
		private ShapesManager.Shape.StarsNumber starsNumber;

		/// <summary>
		/// Star sound effect.
		/// </summary>
		public AudioClip starSoundEffect;

		/// <summary>
		/// Win dialog animator.
		/// </summary>
		public Animator WinDialogAnimator;

		/// <summary>
		/// First star fading animator.
		/// </summary>
		public Animator firstStarFading;

		/// <summary>
		/// Second star fading animator.
		/// </summary>
		public Animator secondStarFading;

		/// <summary>
		/// Third star fading animator.
		/// </summary>
		public Animator thirdStarFading;

		/// <summary>
		/// The level title text.
		/// </summary>
		public Text levelTitle;
			
		/// <summary>
		/// The timer reference.
		/// </summary>
		public Timer timer;

		/// <summary>
		/// The effects audio source.
		/// </summary>
		private AudioSource effectsAudioSource;

		/// <summary>
		/// Whether the dialog is visible or not.
		/// </summary>
		private bool visible;

		// Use this for initialization
		void Start ()
		{
			///Setting up the references
			if (WinDialogAnimator == null) {
				WinDialogAnimator = GetComponent<Animator> ();
			}

			if (firstStarFading == null) {
				firstStarFading = transform.Find ("Stars").Find ("FirstStarFading").GetComponent<Animator> ();
			}

			if (secondStarFading == null) {
				secondStarFading = transform.Find ("Stars").Find ("SecondStarFading").GetComponent<Animator> ();
			}

			if (thirdStarFading == null) {
				thirdStarFading = transform.Find ("Stars").Find ("ThirdStarFading").GetComponent<Animator> ();
			}

			if (effectsAudioSource == null) {
				effectsAudioSource = GameObject.Find ("AudioSources").GetComponents<AudioSource> () [1];
			}
					
			if (levelTitle == null) {
				levelTitle = transform.Find ("Level").GetComponent<Text> ();
			}

			if (timer == null) {
				timer = GameObject.Find ("Time").GetComponent<Timer> ();
			}
		}

		/// <summary>
		/// When the GameObject becomes visible
		/// </summary>
		void OnEnable ()
		{
			//Hide the Win Dialog
			Hide ();
		}

		/// <summary>
		/// Show the Win Dialog.
		/// </summary>
		public void Show ()
		{
			if (WinDialogAnimator == null) {
				return;
			}
			visible = true;
			WinDialogAnimator.SetTrigger ("Running");
		}

		/// <summary>
		/// Hide the Win Dialog.
		/// </summary>
		public void Hide ()
		{
			visible = false;
			StopAllCoroutines ();
			WinDialogAnimator.SetBool ("Running", false);
			firstStarFading.SetBool ("Running", false);
			secondStarFading.SetBool ("Running", false);
			thirdStarFading.SetBool ("Running", false);
		}

		/// <summary>
		/// Fade stars Coroutine.
		/// </summary>
		/// <returns>The stars.</returns>
		public IEnumerator FadeStars ()
		{
			if (visible) {
				starsNumber = timer.progress.starsNumber;
				float delayBetweenStars = 0.5f;
				if (starsNumber == ShapesManager.Shape.StarsNumber.ONE) {//Fade with One Star
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					firstStarFading.SetTrigger ("Running");
				} else if (starsNumber == ShapesManager.Shape.StarsNumber.TWO) {//Fade with Two Stars
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					firstStarFading.SetTrigger ("Running");
					yield return new WaitForSeconds (delayBetweenStars);
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					secondStarFading.SetTrigger ("Running");
				} else if (starsNumber == ShapesManager.Shape.StarsNumber.THREE) {//Fade with Three Stars
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					firstStarFading.SetTrigger ("Running");
					yield return new WaitForSeconds (delayBetweenStars);
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					secondStarFading.SetTrigger ("Running");
					yield return new WaitForSeconds (delayBetweenStars);
					if (effectsAudioSource != null)
						CommonUtil.PlayOneShotClipAt (starSoundEffect, Vector3.zero, effectsAudioSource.volume);
					thirdStarFading.SetTrigger ("Running");

				}
			}
			yield return 0;
		}


		/// <summary>
		/// Set the level title.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetLevelTitle (string value)
		{
			if (string.IsNullOrEmpty (value) || levelTitle == null) {
				return;
			}
			levelTitle.text = value;
		}
	}
}