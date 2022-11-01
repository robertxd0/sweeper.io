using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionFX;
    [SerializeField] private ResourceDestructionMarker marker;

    private MeshRenderer meshRenderer;

    private void Start() {
        if(meshRenderer == null)
            this.meshRenderer = GetComponent<MeshRenderer>();
        
        this.meshRenderer.enabled = true;
    }

    public void EnableMarker()
    {
        this.marker.gameObject.SetActive(true);
    }

    public void Explode()
    {
        meshRenderer.enabled = false;
        this.marker.gameObject.SetActive(false);
        explosionFX.Play(false);
    }

    // public void OnParticleSystemStopped(ParticleSystem system)
    // {
    //     ParticleSystem.Particle[] particles = new ParticleSystem.Particle[system.particleCount];
    //     system.GetParticles(particles);

    //     //swap particle with real rock
    //     foreach(ParticleSystem.Particle particle in particles)
    //     {
    //         var t = 1;
    //     }

    // }
    
}
