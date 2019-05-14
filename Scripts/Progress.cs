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
	public class Progress : MonoBehaviour
	{
		/// <summary>
		/// The star off sprite.
		/// </summary>
		public Sprite starOff;
			
		/// <summary>
		/// The star on sprite.
		/// </summary>
		public Sprite starOn;

		/// <summary>
		/// The level stars.
		/// </summary>
		public Image[] levelStars;

		/// <summary>
		/// The progress image.
		/// </summary>
		public Image progressImage;

		/// <summary>
		/// The stars number.
		/// </summary>
		[HideInInspector]
		public ShapesManager.Shape.StarsNumber starsNumber;

		/// <summary>
		/// Static instance of this class.
		/// </summary>
		public static Progress instance;

		void Awake(){
			if (instance == null)
				instance = this;
		}

		// Use this for initialization
		void Start ()
		{
			if (progressImage == null) {
				progressImage = GetComponent<Image> ();
			}
		}

		/// <summary>
		/// Set the value of the progress.
		/// </summary>
		/// <param name="currentTime">Current time.</param>
		public void SetProgress (float currentTime)
		{
			if (AlphabetGameManager.instance == null) {
				return;
			}

			if (AlphabetGameManager.instance.shape == null) {
				return;
			}

			if (progressImage != null)
				progressImage.fillAmount = 1 - (currentTime / (ShapesManager.instance.GetCurrentShape().starsTimePeriod * 3));

			if (currentTime >= 0 && currentTime <= ShapesManager.instance.GetCurrentShape().starsTimePeriod)
			{
				if (levelStars [0] != null) {
					levelStars [0].sprite = starOn;
				}
				if (levelStars [1] != null) {
					levelStars [1].sprite = starOn;
				}
				if (levelStars [2] != null) {
					levelStars [2].sprite = starOn;
				}
				if (progressImage != null)
					progressImage.color = Colors.greenColor;

				starsNumber = ShapesManager.Shape.StarsNumber.THREE;
			}
			else if (currentTime > ShapesManager.instance.GetCurrentShape().starsTimePeriod && currentTime <= 2 * ShapesManager.instance.GetCurrentShape().starsTimePeriod)
			{
				if (levelStars [2] != null) {
					levelStars [2].sprite = starOff;
				}
				if (progressImage != null)
					progressImage.color = Colors.yellowColor;
				starsNumber = ShapesManager.Shape.StarsNumber.TWO;

			} else {
				if (levelStars [1] != null) {
					levelStars [1].sprite = starOff;
				}
				if (levelStars [2] != null) {
					levelStars [2].sprite = starOff;
				}
				if (progressImage != null)
					progressImage.color = Colors.redColor;
				starsNumber = ShapesManager.Shape.StarsNumber.ONE;
			}
		}

	}
}