using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5f)] private float moveSpeed = 1f;
    [SerializeField] [Range(0.1f, 5f)] private float turnSpeed = 1f;
    
    private CharacterController characterController;
    private Queue<SavedPlayerFrame> savedPlayerFrames;
    private float horizontal;
    private float vertical;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        savedPlayerFrames = new Queue<SavedPlayerFrame>();
    }

    private void Update()
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

    private void FixedUpdate()
    {
        SavedPlayerFrame savedPlayerFrame = new SavedPlayerFrame(transform.position, transform.rotation);
        savedPlayerFrames.Enqueue(savedPlayerFrame);
    }

    public Queue<SavedPlayerFrame> GetFrames()
    {
        return new Queue<SavedPlayerFrame>(savedPlayerFrames);
    }
}
