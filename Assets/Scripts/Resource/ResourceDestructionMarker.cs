using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDestructionMarker : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;


    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }
}
