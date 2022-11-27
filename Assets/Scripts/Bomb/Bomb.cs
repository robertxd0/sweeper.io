using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{    
    public static float bombStrength = 1f;

    [SerializeField] private ParticleSystem explosionFX;
    
    [SerializeField] private GameObject timer;
    
    private HexagonTile plate;
    private MeshRenderer meshRenderer;

    private void Start() {
        if(meshRenderer == null)
            this.meshRenderer = GetComponent<MeshRenderer>();
        
        this.meshRenderer.enabled = true;
        timer.SetActive(true);
    }

    public void SetPlate(HexagonTile plate)
    {
        this.plate = plate;
        this.plate.SetBomb(this);
    }

    public void Explode()
    {
        this.meshRenderer.enabled = false;
        this.timer.SetActive(false);
        //this.explosionFX.SetActive(true);
        this.explosionFX.Play(true);
        this.plate.TriggerPostExplosion();
    }

    public void PostExplosion()
    {
        this.gameObject.SetActive(false);
        this.timer.SetActive(true);
        this.meshRenderer.enabled = true;

        BombHelper.bombPool.PoolObject(this.gameObject);
    }
}
