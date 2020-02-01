using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IRewindable
{
    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5f)] private float moveSpeed = 1f;
    [SerializeField] [Range(0.1f, 5f)] private float turnSpeed = 1f;
    
    private CharacterController characterController;
    private List<SavedPlayerFrame> savedPlayerFrames;
    private float horizontal;
    private float vertical;
    private bool isRewinding;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        savedPlayerFrames = new List<SavedPlayerFrame>();
    }

    private void Update()
    {
        if (!isRewinding)
        {
            horizontal = Input.GetAxisRaw(HorizontalAxis);
            vertical = Input.GetAxisRaw(VerticalAxis);
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * Time.deltaTime * moveSpeed;
            characterController.Move(movement);
        
            if (Mathf.Abs(horizontal + vertical) > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }    
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            
        }
        else
        {
            SavedPlayerFrame savedPlayerFrame = new SavedPlayerFrame(transform.position, transform.rotation);
            savedPlayerFrames.Add(savedPlayerFrame);
        }
    }

    public List<SavedPlayerFrame> GetFrames()
    {
        return new List<SavedPlayerFrame>(savedPlayerFrames);
    }

    public void Rewind()
    {
        
    }
}
