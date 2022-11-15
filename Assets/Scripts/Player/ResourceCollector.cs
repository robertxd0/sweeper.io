using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    private List<ParticleSystem> engagedParticleSystems = new List<ParticleSystem>();

    private GameManager manager;

    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
    private void Start()
    {
        manager = GameManager.instance;
    }

    public void Collect(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem)
    {
        if (!manager.ResourceLimitReached())
        {
            engagedParticleSystems.Add(originSystem);
            StartCoroutine(CollectorCoroutine(particles));
        }else
        {
            StopAllCoroutines();
            CancelCurrentCollectionProcess();  
        } 
    }

    IEnumerator CollectorCoroutine(List<ParticleSystem.Particle> particles)
    {
        var end = Time.time + 0.5f;
        while (Time.time < end)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * 5f);
                if (p.remainingLifetime > 0.35f) //TODO: adapt lifetime based on particle distance.
                {
                    p.remainingLifetime = 0.35f;
                    manager.IncrementResources();
                }
                particles[i] = p;
            }
            if (engagedParticleSystems[engagedParticleSystems.Count - 1].GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
            {
                engagedParticleSystems[engagedParticleSystems.Count - 1].SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
            }
            yield return null;
        }
        engagedParticleSystems.Remove(engagedParticleSystems[engagedParticleSystems.Count - 1]);
    }

    void CancelCurrentCollectionProcess()
    {
        foreach(ParticleSystem engagedSys in engagedParticleSystems)
        {
            ParticleSystem.Particle[] availableParticles = new ParticleSystem.Particle[engagedSys.particleCount];

            engagedSys.GetParticles(availableParticles, engagedSys.particleCount);

            for (int i = 0; i < availableParticles.Length; i++)
            {
                if (availableParticles[i].remainingLifetime > 10f) continue;
                availableParticles[i].remainingLifetime = Mathf.Infinity;
            }
            engagedSys.SetParticles(availableParticles, availableParticles.Length);
        }
    }
}
