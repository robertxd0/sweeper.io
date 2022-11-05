using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    private List<ParticleSystem> engagedParticleSystems = new List<ParticleSystem>();

    private void Awake() {
        instance = this;
    }
    
    private void OnDisable() {
        instance = null;
    }

    public void Collect(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem)
    {
        engagedParticleSystems.Add(originSystem);
        StartCoroutine(CollectorCoroutine(particles));
    }

    IEnumerator CollectorCoroutine(List<ParticleSystem.Particle> particles)
    {
        var end = Time.time + 0.5f;
        while(Time.time < end)
        {
            for(int i = 0; i < particles.Count; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * 5f);
                if(p.remainingLifetime > 0.5f) //adapt lifetime based on distance.
                    p.remainingLifetime = 0.5f;
                particles[i] = p;
            }
            if(engagedParticleSystems[engagedParticleSystems.Count - 1].GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
            {
                engagedParticleSystems[engagedParticleSystems.Count - 1].SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
            }
            yield return null;
        }
        engagedParticleSystems.Remove(engagedParticleSystems[engagedParticleSystems.Count - 1]);
    }
}
