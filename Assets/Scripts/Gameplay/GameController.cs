using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerClonePool playerClonePool;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Transform[] spawnPoints;

    public int TriggersReached { get; private set; }
    private IRewindable[] rewindables;
    private CinemachineVirtualCamera lastGoalTriggerVirtualCamera;
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

    private void Start()
    {
        rewindables = GetComponentsInChildren<IRewindable>(true);
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
            StartRewind();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            EndRewind();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            playableDirector.Play();
        }
    }

    public void StartRewind()
    {
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.StartRewind(4);
        }
    }

    public void EndRewind()
    {
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.EndRewind();
        }
    }

    public void ResetPlayer()
    {
        playerMovement.transform.position = spawnPoints[TriggersReached - 1].position;
        lastGoalTriggerVirtualCamera.Priority = -1;
    }

    public void TriggerReached(int triggerIndex, CinemachineVirtualCamera virtualCamera)
    {
        if (triggerIndex == TriggersReached)
        {
            TriggersReached++;
            Debug.Log("Player Reached correct trigger");
            lastGoalTriggerVirtualCamera = virtualCamera;
            lastGoalTriggerVirtualCamera.Priority = 11;
            playableDirector.Play();
            if (triggerIndex == 4)
            {
                Debug.Log("Last trigger");
                // The end.
            }
            else
            {
                playerClonePool.GetPlayerCloneMovement(playerMovement.GetFrames()).Enable();
            }
        }
    }

    public void CloneTriggerReached(int triggerIndex)
    {
        // Check if clone triggered corresponding goal.
        // Check if next trigger has been triggered.
        Debug.Log("Clone reached trigger");
    }

}
