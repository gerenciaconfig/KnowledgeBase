using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectFromAnim : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
