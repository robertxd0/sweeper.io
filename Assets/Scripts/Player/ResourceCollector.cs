using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public static ResourceCollector instance;
    public Collider collectorCollider;

    private void Awake() {
        instance = this;

    }
    
    private void OnDisable() {
        instance = null;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("lol");
        ParticleSystem originParticleSystem = other.GetComponent<ParticleSystem>();
        if(originParticleSystem != null)
        {
            Debug.LogFormat("collided with particles from {0}", other.name);
        }
    }
}
