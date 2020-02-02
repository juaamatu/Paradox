using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneMovement : MonoBehaviour, IRewindable
{
    [Header("References")]
    [SerializeField] private Animator animator;
    public Renderer cloneModelRend;
    
    private List<SavedPlayerFrame> savedPlayerFrames;
    private bool isRewinding;
    private int rewindSpeed;
    private int currentFrameIndex = 0;
    private float currentSpeed;
    public int targetGoalIndex;
    private Vector3 lastPosition;

    private void Update()
    {
        animator.SetFloat("Speed", currentSpeed);
    }

    private void FixedUpdate()
    {
        if (savedPlayerFrames != null && savedPlayerFrames.Count > 0)
        {
            currentFrameIndex =
                Mathf.Clamp(isRewinding ? currentFrameIndex - rewindSpeed : currentFrameIndex + 1, 0, savedPlayerFrames.Count - 1);
            SavedPlayerFrame savedPlayerFrame = savedPlayerFrames[currentFrameIndex];
            transform.position = savedPlayerFrame.Position;
            transform.rotation = savedPlayerFrame.Rotation;
        }
        else
        {
            enabled = false;
        }

        float movementDelta = Vector3.Magnitude(lastPosition - transform.position) / Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp01(Mathf.MoveTowards(currentSpeed, movementDelta, Time.deltaTime * 5));
        lastPosition = transform.position;
    }

    public void Initialize(List<SavedPlayerFrame> frames)
    {
        ChangeMaterial();
        savedPlayerFrames = frames;
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void StartRewind(int speed)
    {
        isRewinding = true;
        rewindSpeed = speed;
    }

    public void EndRewind()
    {
        isRewinding = false;
    }

    public void ResetPosition()
    {
        
        currentFrameIndex = 0;
    }

    public void ChangeMaterial()
    {
        cloneModelRend.material = GameController.Instance.playerMaterials[Mathf.Clamp(GameController.Instance.TriggersReached - 1, 0, 3)];
    }
}
