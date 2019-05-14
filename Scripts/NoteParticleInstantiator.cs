using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteParticleInstantiator : MonoBehaviour
{
    public GameObject particle;

    public List<Material> materials;

    public int duration;

    public void InstantiateParticleSortMaterial(Color color)
    {
        GameObject particleGO = Instantiate(particle);

        int aux = Random.Range(0, materials.Count);


        particleGO.GetComponent<ParticleSystemRenderer>().material = materials[aux];
        particleGO.GetComponent<ParticleSystem>().startColor = color;

        Destroy(particleGO, duration);
    }


    public void InstantiateParticle()
    {
        GameObject particleGO = Instantiate(particle);
        Destroy(particleGO, duration);
    }
}
