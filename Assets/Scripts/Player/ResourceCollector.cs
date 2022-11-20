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

    private void CollectStandard(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        if (!manager.ResourceLimitReached())
        {
            engagedParticleSystems.Add(originSystem);
            StartCoroutine(CollectorCoroutine(particles, type));
        }else
        {
            StopAllCoroutines();
            CancelCurrentCollectionProcess();  
        } 
    }

    private void CollectGold(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        engagedParticleSystems.Add(originSystem);
        StartCoroutine(CollectorCoroutine(particles, type));
    }

    public void Collect(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        switch(type)
        {
            case ResourceType.STANDARD_RES:
                this.CollectStandard(particles, ref originSystem, type);
            break;
            case ResourceType.GOLD_RES:
                this.CollectGold(particles, ref originSystem, type);
            break;
        }
    }

    IEnumerator CollectorCoroutine(List<ParticleSystem.Particle> particles, ResourceType type)
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
                    if(type == ResourceType.STANDARD_RES)
                        manager.IncrementResources();
                    else if(type == ResourceType.GOLD_RES)
                        manager.IncrementGoldResources();
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
