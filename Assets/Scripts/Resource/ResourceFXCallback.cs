using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFXCallback : MonoBehaviour
{
    ParticleSystem particleSystem;

    List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();

    Resource resourceObj;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.trigger.AddCollider(ResourceCollector.instance.collectorCollider);

        resourceObj = GetComponentInParent<Resource>();
    }
    void OnParticleTrigger()
    {
        int numEnter = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enterParticles);

        if (numEnter > 0)
        {
            ResourceCollector.instance.Collect(enterParticles, ref particleSystem, resourceObj.getResourceType);
        }
    }
}
