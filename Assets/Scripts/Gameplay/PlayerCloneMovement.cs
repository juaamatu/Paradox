using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloneMovement : MonoBehaviour
{
    private List<SavedPlayerFrame> savedPlayerFrames;

    private void FixedUpdate()
    {
        if (savedPlayerFrames != null && savedPlayerFrames.Count > 0)
        {
            SavedPlayerFrame savedPlayerFrame = savedPlayerFrames[0];
            savedPlayerFrames.RemoveAt(0);
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
}
