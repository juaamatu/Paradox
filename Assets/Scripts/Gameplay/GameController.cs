using System;
using System.Collections.Generic;
using System.Linq;
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
            rewindable.StartRewind(2);
        }
    }
    
    public void EndRewind()
    {
        foreach (IRewindable rewindable in rewindables)
        {
            rewindable.EndRewind();
        }
    }

    public void TriggerReached(int triggerIndex)
    {
        if (triggerIndex == TriggersReached)
        {
            TriggersReached++;
        }
    }
}
