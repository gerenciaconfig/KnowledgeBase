using UnityEngine;

namespace Mkey
{
    public class DontDestroyObj : MonoBehaviour
    {

        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}