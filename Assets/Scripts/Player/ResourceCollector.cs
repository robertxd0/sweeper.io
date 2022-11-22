using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

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

    public void CollectRoutine(List<ParticleSystem.Particle> particles, ref ParticleSystem originSystem, ResourceType type)
    {
        if (type == ResourceType.STANDARD_RES && manager.ResourceLimitReached())
        {
            for (int i = 0; i < particles.Count; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.remainingLifetime = Mathf.Infinity;
                particles[i] = p;
            }

            if (originSystem.GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
            {
                originSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
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
            particles[i] = p;
        }

        if (originSystem.GetSafeTriggerParticlesSize(ParticleSystemTriggerEventType.Inside) == particles.Count) //not good. but it works (insert flushed emoji here)
        {
            originSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
        }

    }
}
