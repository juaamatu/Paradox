using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneMovement : MonoBehaviour
{
    private Queue<SavedPlayerFrame> savedPlayerFrames;

    private void FixedUpdate()
    {
        if (savedPlayerFrames != null && savedPlayerFrames.Count > 0)
        {
            SavedPlayerFrame savedPlayerFrame = savedPlayerFrames.Dequeue();
            transform.position = savedPlayerFrame.Position;
            transform.rotation = savedPlayerFrame.Rotation;
        }
        else
        {
            enabled = false;
        }
    }

    public void Initialize(Queue<SavedPlayerFrame> frames)
    {
        savedPlayerFrames = frames;
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }
}
