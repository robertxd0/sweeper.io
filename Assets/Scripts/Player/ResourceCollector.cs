using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    //private List<ParticleSystem> engagedParticleSystems = new List<ParticleSystem>();

    private Dictionary<ResourceType, List<ParticleSystem>> engagedParticleSystems = new Dictionary<ResourceType, List<ParticleSystem>>();

    private Dictionary<ResourceType, Coroutine> collectorCoroutines = new Dictionary<ResourceType, Coroutine>();

    private GameManager manager;

    int coroutineStartCountDbg = 0;
    int coroutineEndCountDbg = 0;

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

        if (!engagedParticleSystems.ContainsKey(ResourceType.STANDARD_RES))
        {
            engagedParticleSystems.Add(ResourceType.STANDARD_RES, new List<ParticleSystem>());
        }

        if (!engagedParticleSystems.ContainsKey(ResourceType.GOLD_RES))
        {
            engagedParticleSystems.Add(ResourceType.GOLD_RES, new List<ParticleSystem>());
        }


        if (!collectorCoroutines.ContainsKey(ResourceType.STANDARD_RES))
        {
            collectorCoroutines.Add(ResourceType.STANDARD_RES, null);
        }

        if (!collectorCoroutines.ContainsKey(ResourceType.GOLD_RES))
        {
            collectorCoroutines.Add(ResourceType.GOLD_RES, null);
        }

    }

    private void CollectStandard(ref ParticleSystem originSystem)
    {
        CollectStandard(ref originSystem);
    }

    private void CollectGold(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem)
    {

        engagedParticleSystems[ResourceType.GOLD_RES].Add(originSystem);
        if (collectorCoroutines[ResourceType.GOLD_RES] != null)
        {
            StopCoroutine(collectorCoroutines[ResourceType.GOLD_RES]);
        }

        collectorCoroutines[ResourceType.GOLD_RES] = StartCoroutine(GoldCollectorCoroutine(particles));
    }

    public void Collect(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        switch (type)
        {
            case ResourceType.STANDARD_RES:
                //this.CollectStandard(particles, ref originSystem);
                break;
            case ResourceType.GOLD_RES:
                this.CollectGold(particles, ref originSystem);
                break;
        }
    }


    void StandardCollectorRoutine(List<ParticleSystem.Particle> particles)
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
        if (engagedParticleSystems[ResourceType.STANDARD_RES][engagedParticleSystems[ResourceType.STANDARD_RES].Count - 1].GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
        {
            engagedParticleSystems[ResourceType.STANDARD_RES][engagedParticleSystems[ResourceType.STANDARD_RES].Count - 1].SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
        }
    }

    IEnumerator GoldCollectorCoroutine(List<ParticleSystem.Particle> particles)
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
                    manager.IncrementGoldResources();
                }
                particles[i] = p;
            }
            if (engagedParticleSystems[ResourceType.GOLD_RES][engagedParticleSystems[ResourceType.GOLD_RES].Count - 1].GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
            {
                engagedParticleSystems[ResourceType.GOLD_RES][engagedParticleSystems[ResourceType.GOLD_RES].Count - 1].SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
            }
            yield return null;
        }
        //engagedParticleSystems[ResourceType.GOLD_RES].RemoveRange(engagedParticleSystems[ResourceType.GOLD_RES].Count - timesStarted, timesStarted);
    }

    void CancelCurrentStandardCollectionProcess()
    {
        foreach (ParticleSystem engagedSys in engagedParticleSystems[ResourceType.STANDARD_RES])
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

        //engagedParticleSystems.Clear();
    }
}
