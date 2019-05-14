using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	private ParticleSystem particleSystem;

    public string particleSound;

	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
	}

	/// <summary>
	/// Use this as a simple start 
	/// </summary>
	public void StartParticles() {
		particleSystem.Play();
		
        if (particleSound != "")
        {
            AudioManager.instance.PlaySound(particleSound);
            
        }
	}
	
	/// <summary>
	/// Use this to restart the particle system
	/// </summary>
	public void StartParticlesFromBeginning() {
		if (particleSystem.isEmitting || particleSystem.IsAlive()) {
			particleSystem.Clear();
			particleSystem.Stop();
		}

        StartParticles();
    }
	
	/// <summary>
	/// Use this to stop the particle system and reset it's state to 0
	/// </summary>
	public void StopParticles() {
		particleSystem.Stop();
	}

	/// <summary>
	/// Use this to pause the particle system in current state
	/// </summary>
	public void PauseParticles() {
		particleSystem.Pause();
	}
	
}
