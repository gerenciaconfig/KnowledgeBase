using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float destroyTime = 1.0f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

}