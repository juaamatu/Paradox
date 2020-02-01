using System.Collections.Generic;
using UnityEngine;

public class PlayerClonePool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCloneMovement playerCloneMovementPrefab;

    private Stack<PlayerCloneMovement> pool;
    private const int cloneCount = 4;

    private void Awake()
    {
        pool = new Stack<PlayerCloneMovement>();
        for (int i = 0; i < cloneCount; i++)
        {
            PlayerCloneMovement playerCloneMovement = Instantiate(playerCloneMovementPrefab);
            playerCloneMovement.gameObject.SetActive(false);
            pool.Push(playerCloneMovement);
        }
    }

    public PlayerCloneMovement GetPlayerCloneMovement(List<SavedPlayerFrame> savedPlayerFrames)
    {
        PlayerCloneMovement playerCloneMovement = pool.Pop();
        playerCloneMovement.Initialize(savedPlayerFrames);
        playerCloneMovement.gameObject.SetActive(true);
        return playerCloneMovement;
    }
}
