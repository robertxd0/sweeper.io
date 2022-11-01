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
        StartCoroutine(CollectorCoroutine(particles, engagedParticleSystems.Count - 1));
    }

    IEnumerator CollectorCoroutine(List<ParticleSystem.Particle> particles, int index)
    {
        var end = Time.time + 50f;
        while(Time.time < end)
        {
            for(int i = 0; i < particles.Count; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * 5f);
                p.remainingLifetime = 2.5f;
                particles[i] = p;
            }
            if(engagedParticleSystems[index].GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Enter) == particles.Count && engagedParticleSystems.Count > index)
            {
                engagedParticleSystems[index].SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
            }
            yield return null;
        }
        engagedParticleSystems.Remove(engagedParticleSystems[index]);
    }
}
