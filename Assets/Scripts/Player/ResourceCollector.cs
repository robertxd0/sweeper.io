using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    //private List<ParticleSystem> engagedParticleSystems = new List<ParticleSystem>();

    private Dictionary<ResourceType, List<ParticleSystem>> engagedParticleSystems = new Dictionary<ResourceType, List<ParticleSystem>>(); 

    private Dictionary<ResourceType, List<Coroutine>> collectorCoroutines = new Dictionary<ResourceType, List<Coroutine>>();
    
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
    }

    private void CollectStandard(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem)
    {
        if (!manager.ResourceLimitReached())
        {
            if(!engagedParticleSystems.ContainsKey(ResourceType.STANDARD_RES))
            {
                engagedParticleSystems.Add(ResourceType.STANDARD_RES, new List<ParticleSystem>());
            }
            engagedParticleSystems[ResourceType.STANDARD_RES].Add(originSystem);

            if(!collectorCoroutines.ContainsKey(ResourceType.STANDARD_RES))
            {
                collectorCoroutines.Add(ResourceType.STANDARD_RES, new List<Coroutine>());
            }

            Coroutine _coroutine = StartCoroutine(StandardCollectorCoroutine(particles));
            collectorCoroutines[ResourceType.STANDARD_RES].Add(_coroutine);
        }else
        {
            for (int i = 0; i < collectorCoroutines[ResourceType.STANDARD_RES].Count; i++)
            {
                StopCoroutine(collectorCoroutines[ResourceType.STANDARD_RES][i]);
            }

            collectorCoroutines[ResourceType.STANDARD_RES].Clear();
            CancelCurrentStandardCollectionProcess();
        } 
    }

    private void CollectGold(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem)
    {
        if(!engagedParticleSystems.ContainsKey(ResourceType.GOLD_RES))
        {
            engagedParticleSystems.Add(ResourceType.GOLD_RES, new List<ParticleSystem>());
        }
        engagedParticleSystems[ResourceType.GOLD_RES].Add(originSystem);

        Coroutine _coroutine = StartCoroutine(GoldCollectorCoroutine(particles));
        if(!collectorCoroutines.ContainsKey(ResourceType.GOLD_RES))
        {
            collectorCoroutines.Add(ResourceType.GOLD_RES, new List<Coroutine>());
        }
        collectorCoroutines[ResourceType.GOLD_RES].Add(_coroutine);
    }

    public void Collect(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        switch(type)
        {
            case ResourceType.STANDARD_RES:
                this.CollectStandard(particles, ref originSystem);
            break;
            case ResourceType.GOLD_RES:
                this.CollectGold(particles, ref originSystem);
            break;
        }
    }

    IEnumerator StandardCollectorCoroutine(List<ParticleSystem.Particle> particles)
    {
        var end = Time.time + 0.5f;
        bool lol = false;
        this.coroutineStartCountDbg++;

        Debug.Log("[START]: " + coroutineStartCountDbg + " | " + coroutineEndCountDbg  + " | cCount: " + collectorCoroutines[ResourceType.STANDARD_RES].Count);

        while (!lol)
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
            lol = true;
            yield return null;
        }
        engagedParticleSystems[ResourceType.STANDARD_RES].Remove(engagedParticleSystems[ResourceType.STANDARD_RES][engagedParticleSystems[ResourceType.STANDARD_RES].Count - 1]);
        RemoveLatestCoroutine(ResourceType.STANDARD_RES);
        this.coroutineEndCountDbg++;
        Debug.Log("[END]: " + coroutineStartCountDbg + " | " + coroutineEndCountDbg  + " | cCount: " + collectorCoroutines[ResourceType.STANDARD_RES].Count);

    }

    void RemoveLatestCoroutine(ResourceType type)
    {
        StopCoroutine(collectorCoroutines[type][collectorCoroutines[type].Count - 1]);
        collectorCoroutines[type].RemoveAt(collectorCoroutines[type].Count - 1);

        // Debug.Log(type.ToString() +  ":" + collectorCoroutines[type].Count);
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
        engagedParticleSystems[ResourceType.GOLD_RES].Remove(engagedParticleSystems[ResourceType.GOLD_RES][engagedParticleSystems[ResourceType.GOLD_RES].Count - 1]);
        RemoveLatestCoroutine(ResourceType.GOLD_RES);
    }

    void CancelCurrentStandardCollectionProcess()
    {
        foreach(ParticleSystem engagedSys in engagedParticleSystems[ResourceType.STANDARD_RES])
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
