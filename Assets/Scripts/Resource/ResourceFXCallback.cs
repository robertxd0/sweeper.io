using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFXCallback : MonoBehaviour
{
    ParticleSystem particleSystem;

    private void Start() {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.trigger.AddCollider(ResourceCollector.instance.collectorCollider);
    }
    void OnParticleTrigger()
    {
        Debug.Log("haha lol");
    }
}
