using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonaHalfSingleton : MonoBehaviour
{
    private static DetonaHalfSingleton instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
