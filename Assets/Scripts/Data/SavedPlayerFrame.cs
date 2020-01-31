using UnityEngine;

public struct SavedPlayerFrame
{
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;

    public SavedPlayerFrame(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
