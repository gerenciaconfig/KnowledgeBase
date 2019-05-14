using UnityEngine;

namespace Arcolabs.Disney
{
	public class RandomChildren : MonoBehaviour
	{
		public ArcoTracingCollider[] children;
		public int Length { get { return children.Length; } }

		public void SortChildren()
		{
			ArcoTracingCollider temp;
			for (int i = 0; i < children.Length; i++)
			{
				children[i].transform.parent.gameObject.SetActive(false);
				children[i].ResetEverything();
				temp = children[i];
				int rand = Random.Range(0, children.Length - 1);
				children[i] = children[rand];
				children[rand] = temp;
				children[rand].ResetEverything();
			}
			children[0].transform.parent.gameObject.SetActive(true);
		}

		public ArcoTracingCollider GetChildrenIndex(int index)
		{
			return children[index];
		}
	}
}
