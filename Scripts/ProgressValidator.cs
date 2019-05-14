using UnityEngine;

[RequireComponent(typeof(ProgressController))]
public abstract class ProgressValidator : MonoBehaviour
{
	public abstract bool Succeeded();
	public abstract bool Progressed();
}
