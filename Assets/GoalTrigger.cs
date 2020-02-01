using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
   // [SerializeField] private string targetTag;
    [SerializeField] private int triggerIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.TriggerReached(triggerIndex);
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
