using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class ParticlesDestroy : MonoBehaviour
{
    private void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if(ps)
        Destroy(gameObject, (ps.main.duration + ps.main.startLifetime.constantMax) * 1.1f);
    }

}