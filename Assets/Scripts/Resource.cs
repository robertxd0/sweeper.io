using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionFX;

    private MeshRenderer meshRenderer;

    private void Start() {
        if(meshRenderer == null)
            this.meshRenderer = GetComponent<MeshRenderer>();
        
        this.meshRenderer.enabled = true;
    }

    public void Explode()
    {
        meshRenderer.enabled = false;
        explosionFX.Play(false);
    }
    
}
