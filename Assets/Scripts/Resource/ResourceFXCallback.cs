using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFXCallback : MonoBehaviour
{
    private Resource resource;
    private ParticleSystem particleSystem;

    void Start() {
        this.resource = GetComponentInParent<Resource>();
        this.particleSystem = GetComponent<ParticleSystem>(); 
    } 

    public void OnParticleSystemStopped()
    {
        this.resource.OnParticleSystemStopped(particleSystem);
    }
}
