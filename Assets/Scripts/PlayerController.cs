using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float turnSpeed = 120f;
    public float jumpForce = 5f;
    public float gravity = -20f;

    [Header("Animation")]
    public Animator animator;

    private CharacterController _cc;
    private float _yVelocity;
    private bool _jumpQueued;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float h = 0f, v = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) h = -1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h =  1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v =  1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v = -1f;

        bool run = kb.leftShiftKey.isPressed || kb.rightShiftKey.isPressed;

        if (kb.spaceKey.wasPressedThisFrame && _cc.isGrounded)
            _jumpQueued = true;

        // Rotate
        transform.Rotate(0f, h * turnSpeed * Time.deltaTime, 0f);

        // Move
        float speed = run ? runSpeed : walkSpeed;
        Vector3 move = transform.forward * v * speed;

        // Gravity & jump
        if (_cc.isGrounded)
        {
            _yVelocity = -2f;
            if (_jumpQueued)
            {
                _yVelocity = jumpForce;
                _jumpQueued = false;
            }
        }
        _yVelocity += gravity * Time.deltaTime;
        move.y = _yVelocity;

        _cc.Move(move * Time.deltaTime);

        // Animator
        if (animator != null)
        {
            float absV = Mathf.Abs(v);
            animator.SetFloat("Speed", absV);
            animator.SetBool("IsRunning", run && absV > 0.1f);
            animator.SetBool("IsJumping", !_cc.isGrounded);
        }
    }

    // Receiver for Unity-chan's built-in AnimationEvents
    void OnCallChangeFace(string faceName) { }
    void OnCallCrossFade(string stateName) { }
}
