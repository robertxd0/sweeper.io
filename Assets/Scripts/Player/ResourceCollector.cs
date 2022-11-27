using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    public ParticleSystem.MinMaxCurve uncollectedParticleCurve;

    private GameManager manager;
    private List<ParticleSystem> uncollectedParticleSystems = new List<ParticleSystem>();

    private Coroutine collectorStoppedCoroutine;

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

    public void CollectRoutine(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        if (type == ResourceType.STANDARD_RES && manager.ResourceLimitReached())
        {
            if (uncollectedParticleSystems.Contains(originSystem))
            {
                bool hasCollectableParticles = false;
                for (int i = 0; i < particles.Count; i++)
                {
                    ParticleSystem.Particle p = particles[i];
                    if (p.remainingLifetime < 1f)
                    {
                        p.position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * 5f);
                        hasCollectableParticles = true;
                    }
                    particles[i] = p;
                }

                if (originSystem.GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
                {
                    originSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
                }

                if (!hasCollectableParticles)
                {
                    uncollectedParticleSystems.Remove(originSystem);
                }
            }
            return;
        }

        for (int i = 0; i < particles.Count; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.position = Vector3.Lerp(p.position, transform.position, Time.deltaTime * 5f);
            if (p.remainingLifetime > 0.35f) //TODO: adapt lifetime based on particle distance.
            {
                p.remainingLifetime = 0.35f;
                if (type == ResourceType.GOLD_RES) manager.IncrementGoldResources();
                else if (type == ResourceType.STANDARD_RES) manager.IncrementResources();
            }
            else
            {
                if (!uncollectedParticleSystems.Contains(originSystem))
                    uncollectedParticleSystems.Add(originSystem);
            }

            particles[i] = p;
        }

        if (originSystem.particleCount == 0)
        {
            if (uncollectedParticleSystems.Contains(originSystem))
            {
                uncollectedParticleSystems.Remove(originSystem);
            }
        }

        if (originSystem.GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
        {
            originSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
        }
    }
}
