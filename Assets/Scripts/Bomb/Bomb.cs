using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static float bombStrength = 2f;

    [SerializeField] private ParticleSystem explosionFX;
    
    [SerializeField] private GameObject timer;

    
    private HexagonPlate plate;
    private MeshRenderer meshRenderer;

    private void Start() {
        if(meshRenderer == null)
            this.meshRenderer = GetComponent<MeshRenderer>();
        
        this.meshRenderer.enabled = true;
        timer.SetActive(true);
    }

    public void SetPlate(HexagonPlate plate)
    {
        this.plate = plate;
        this.plate.SetBomb(this);
    }

    public void Explode()
    {
        this.meshRenderer.enabled = false;
        this.timer.SetActive(false);
        this.explosionFX.Play(true);
        this.plate.TriggerPostExplosion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
