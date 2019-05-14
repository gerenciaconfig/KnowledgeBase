using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.General
{
	/// <summary>
	/// Items that stay under observation and have to pass information
	/// about their state
	/// </summary>
	public abstract class ObservableItem : MonoBehaviour
	{
		protected Dictionary<string, bool> status = new Dictionary<string, bool>();

		public bool GetStatus(string name)
		{
			return status[name];
		}
	}
}
