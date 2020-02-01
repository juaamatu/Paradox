using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerClonePool playerClonePool;
    [SerializeField] private PlayerMovement playerMovement;
    
    public static GameController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<SavedPlayerFrame> savedPlayerFrames = playerMovement.GetFrames();
            PlayerCloneMovement playerCloneMovement = playerClonePool.GetPlayerCloneMovement(savedPlayerFrames);
            playerCloneMovement.Enable();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            IRewindable[] rewindables = GetComponentsInChildren<IRewindable>(true);
            Debug.Log(rewindables.Length);
            foreach (IRewindable rewindable in rewindables)
            {
                rewindable.StartRewind(2);
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            IRewindable[] rewindables = GetComponentsInChildren<IRewindable>(true);
            Debug.Log(rewindables.Length);
            foreach (IRewindable rewindable in rewindables)
            {
                rewindable.EndRewind();
            }
        }
    }
}
