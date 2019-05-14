using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapunzelParticleUIScaler : MonoBehaviour
{
    [SerializeField] private float orbitalZ_16x9 = 0.44f;
    [SerializeField] private float orbitalZ_16x10 = 0.5f;
    [SerializeField] private float orbitalZ_3x2 = 0.56f;
    [SerializeField] private float orbitalZ_5x4 = 0.62f;
    [SerializeField] private float orbitalZ_4x3 = 0.6f;

    private ParticleSystem ps;
    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.VelocityOverLifetimeModule vot = ps.velocityOverLifetime;

        if (Mathf.Abs(Camera.main.aspect - 16f / 9f) <= 0.01f)
        {
            vot.orbitalZ = orbitalZ_16x9;
            print("16x9");
        }
        else if (Mathf.Abs(Camera.main.aspect - 16f / 10f) <= 0.01f)
        {
            vot.orbitalZ = orbitalZ_16x10;
            print("16x10");
        }
        else if (Mathf.Abs(Camera.main.aspect - 3f / 2f) <= 0.01f)
        {
            vot.orbitalZ = orbitalZ_3x2;
            print("3x2");
        }
        else if (Mathf.Abs(Camera.main.aspect - 5f / 4f) <= 0.01f)
        {
            vot.orbitalZ = orbitalZ_5x4;
            print("5x4");
        }
        else if (Mathf.Abs(Camera.main.aspect - 4f / 3f) <= 0.01f)
        {
            vot.orbitalZ = orbitalZ_4x3;
            print("4x3");
        }
    }
}