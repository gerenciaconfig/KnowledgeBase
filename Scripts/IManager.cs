using UnityEngine;

namespace Arcolabs.General
{
	/// <summary>
	/// Manager classes are notified by observable items
	/// </summary>
	public interface IManager
	{
		void Notify(ObservableItem o);
	}
}
