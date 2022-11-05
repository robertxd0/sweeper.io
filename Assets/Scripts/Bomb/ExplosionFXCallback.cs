using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFXCallback : MonoBehaviour
{
    private Bomb bombRef;


    private void Start() {
        bombRef = GetComponentInParent<Bomb>();
    }

    void OnParticleSystemStopped()
    {
       bombRef.PostExplosion();
    }
}
