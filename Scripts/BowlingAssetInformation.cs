using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arcolabs.Disney
{
	public class BowlingAssetInformation : ScriptableObject
	{
		public enum GameAsset { Donald, Mickey, Minnie, Pateta, DoisDedos, TresDedos, QuatroDedos, CincoDedos, SeisDedos, SeteDedos, OitoDedos }
		[OnValueChanged("UpdatePortrait")]
		public GameAsset character;

		public void UpdatePortrait()
		{
			if (character == GameAsset.Donald)
			{

			}
			else if (character == GameAsset.Mickey)
			{

			}
			else if (character == GameAsset.Minnie)
			{

			}
			else if (character == GameAsset.Pateta)
			{

			}
		}
	}
}
