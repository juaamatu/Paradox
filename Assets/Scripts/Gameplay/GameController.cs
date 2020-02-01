using System.Collections.Generic;
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
            Queue<SavedPlayerFrame> savedPlayerFrames = playerMovement.GetFrames();
            PlayerCloneMovement playerCloneMovement = playerClonePool.GetPlayerCloneMovement(savedPlayerFrames);
            playerCloneMovement.Enable();
        }
    }
}
