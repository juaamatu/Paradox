using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FinishTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0, 3)] private int triggerIndex; 
    
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
