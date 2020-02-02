using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private ParticleSystemTrigger particleSystems;
    
    [Header("Settings")]
    [SerializeField] private int triggerIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.TriggerReached(triggerIndex, virtualCamera);
            if(GameController.Instance.TriggersReached != 4)
            {
                particleSystems.Explode();
            }

        }
        else if (other.CompareTag("Clone"))
        {
            if (other.GetComponent<PlayerCloneMovement>())
            {
                GameController.Instance.CloneTriggerReached(triggerIndex);
            }
        }
    }
}
