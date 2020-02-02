using System;
using System.Collections.Generic;
using System.Collections;
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

    [SerializeField] private CinemachineVirtualCamera endVcam;
    [SerializeField] private PlayableDirector endTimeline;

    public Material[] playerMaterials;


    public int TriggersReached { get; private set; }
    private IRewindable[] rewindables;
    private CinemachineVirtualCamera lastGoalTriggerVirtualCamera;
    private List<SavedPlayerFrame> lastSavedFrames;
    private List<PlayerCloneMovement> instantiatedCloneMovements = new List<PlayerCloneMovement>();
    private int clonesReachedFinalButton;
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

    private IEnumerator RewindCoroutine(float time)
    {
        StartRewind();
        yield return new WaitForSeconds(time);
        EndRewind();
    }

    private IEnumerator RewindCoroutineAlternative()
    {
        StartRewind();
        yield return new WaitForSeconds(1);
        playerMovement.EndRewind();
        yield return new WaitForSeconds(0.5f);
        foreach (PlayerCloneMovement instantiatedCloneMovement in instantiatedCloneMovements)
        {
            instantiatedCloneMovement.EndRewind();
        }
    }
    
    private IEnumerator EnablePlayerDelayed()
    {
        yield return null;
        playerMovement.enabled = true;
    }

    public void StartRewind(float time)
    {
        StartCoroutine(RewindCoroutine(time));
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
        PlayerCloneMovement cloneMovements = playerClonePool.GetPlayerCloneMovement(lastSavedFrames);
        cloneMovements.Enable();
        instantiatedCloneMovements.Add(cloneMovements);
        playerMovement.Reset();
        playerMovement.enabled = false;
        foreach (PlayerCloneMovement instantiatedCloneMovement in instantiatedCloneMovements)
        {
            instantiatedCloneMovement.ResetPosition();
        }

        clonesReachedFinalButton = 0;
        StartCoroutine(EnablePlayerDelayed());
    }

    public void TriggerReached(int triggerIndex, CinemachineVirtualCamera virtualCamera)
    {
        if (triggerIndex == TriggersReached)
        {
            if (triggerIndex == 3)
            {
                endVcam.Priority = 12;
                if (clonesReachedFinalButton == 3)
                {
                    endTimeline.gameObject.SetActive(true);
                    endTimeline.Play();
                }
            }
            else
            {
                TriggersReached++;
                lastSavedFrames = playerMovement.GetFrames();
                Debug.Log("Player Reached correct trigger");
                lastGoalTriggerVirtualCamera = virtualCamera;
                lastGoalTriggerVirtualCamera.Priority = 11;
                playableDirector.Play();
            }
        }
    }

    public void CloneTriggerReached(int triggerIndex)
    {
        // Check if clone triggered corresponding goal.
        // Check if next trigger has been triggered.
        Debug.Log("Clone reached trigger");
        clonesReachedFinalButton++;
        if (TriggersReached == 4)
        {
            if (clonesReachedFinalButton == 3)
            {
                endTimeline.gameObject.SetActive(true);
                endTimeline.Play();
            }
        }
        else
        {
            RewindCoroutineAlternative();
            clonesReachedFinalButton = 0;
        }
    }

}
