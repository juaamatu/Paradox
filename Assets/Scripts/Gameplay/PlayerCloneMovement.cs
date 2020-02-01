using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneMovement : MonoBehaviour, IRewindable
{
    private List<SavedPlayerFrame> savedPlayerFrames;
    private bool isRewinding;
    private int rewindSpeed;
    private int currentFrameIndex = 0;

    private void FixedUpdate()
    {
        if (savedPlayerFrames != null && savedPlayerFrames.Count > 0)
        {
            currentFrameIndex =
                Mathf.Clamp(isRewinding ? currentFrameIndex - rewindSpeed : currentFrameIndex + 1, 0, savedPlayerFrames.Count - 1);
            SavedPlayerFrame savedPlayerFrame = savedPlayerFrames[currentFrameIndex];
            Debug.Log(currentFrameIndex);
            transform.position = savedPlayerFrame.Position;
            transform.rotation = savedPlayerFrame.Rotation;
        }
        else
        {
            enabled = false;
        }
    }

    public void Initialize(List<SavedPlayerFrame> frames)
    {
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
}
