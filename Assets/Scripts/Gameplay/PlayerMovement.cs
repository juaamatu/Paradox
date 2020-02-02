using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IRewindable
{
    [Header("References")]
    [SerializeField] private Animator animator;
    
    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5f)] private float moveSpeed = 1f;
    [SerializeField] [Range(0.1f, 10f)] private float turnSpeed = 1f;
    [SerializeField] private Renderer playerModelRend;
    
    private CharacterController characterController;
    private List<SavedPlayerFrame> savedPlayerFrames;
    private float horizontal;
    private float vertical;
    private int rewindSpeed;
    private bool isRewinding;
    private float currentSpeed;

    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string ViewConeTag = "ViewCone";

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
            currentSpeed = Mathf.Clamp01(Mathf.MoveTowards(currentSpeed, characterController.velocity.magnitude, Time.deltaTime * 5));
            animator.SetFloat("Speed", currentSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding && savedPlayerFrames.Count > 0)
        {
            int popIndex = Mathf.Clamp(savedPlayerFrames.Count - rewindSpeed, 0, savedPlayerFrames.Count - 1);
            SavedPlayerFrame savedPlayerFrame = savedPlayerFrames[popIndex];
            for (int i = 0; i < rewindSpeed; i++)
            {
                if (savedPlayerFrames.Count == 0)
                {
                    break;
                }
                savedPlayerFrames.RemoveAt(savedPlayerFrames.Count - 1);
            }
            transform.position = savedPlayerFrame.Position;
            transform.rotation = savedPlayerFrame.Rotation;
        }
        else if (!isRewinding)
        {
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * Time.deltaTime * moveSpeed;
            characterController.Move(movement);
        
            if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
            }
            
            SavedPlayerFrame savedPlayerFrame = new SavedPlayerFrame(transform.position, transform.rotation);
            savedPlayerFrames.Add(savedPlayerFrame);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ViewConeTag))
        {
            GameController.Instance.StartRewind(1f);
        }
    }

    public List<SavedPlayerFrame> GetFrames()
    {
        return new List<SavedPlayerFrame>(savedPlayerFrames);
    }

    public void Reset()
    {
        playerModelRend.material = GameController.Instance.playerMaterials[Mathf.Clamp(GameController.Instance.TriggersReached,0,3)];
        savedPlayerFrames = new List<SavedPlayerFrame>();
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
