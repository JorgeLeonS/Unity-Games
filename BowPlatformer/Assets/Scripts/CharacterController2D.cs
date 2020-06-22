using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 649

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _maxVelocity = 5f;                               // max velocity reached while moving on ground
    [SerializeField] private float _groundAcceleration = 5f;                        // acceleration on ground
    [SerializeField] private float _airAcceleration = 2f;                           // acceleration while in the air
    [SerializeField] private float _drag = 10f;                                     // how fast character stops moving while grounded
    [SerializeField] private float _jumpVelocity = 9f;                              // initial vertical velocity during jump
    [SerializeField] private float _jumpHorizontalVelocityMultiplier = 0.75f;       // multiplier on initial horizontal velocity during jump
    [SerializeField] private int _jumpCount = 2;                                    // how many times character can jump before landing
    [SerializeField] private int _wallJumpAllowance = 1;                            // additional jumps when wall is touched
    [SerializeField] private float _gravity = -20f;                                 // gravity force
    [SerializeField] private float _terminalVelocity = 15f;                          // Maximum velocity
    
    [Header("Grounding")]
    [SerializeField] private float _groundCheckDistance = 0.3f;                     // how far down circle travels during ground check
    [SerializeField] private float _groundCheckOffset = 0.5f;                       // how high circle starts
    [SerializeField] private float _groundCheckRadius = 0.3f;                       // radius of circle
    [SerializeField] private float _maxSlopAngle = 45f;                             // max angle character can stand on
    [SerializeField] private LayerMask _groundCheckMask;                            // layer to look for ground colliders
    [SerializeField] private string _wallJumpTag = "WallJump";                      // tag to identify objects that allow a wall jump

    public bool IsGrounded {get; private set;}      // is the character grounded
    public bool IsJumping {get; private set;}       // is the character jumping (still moving up)
    public int JumpsCount => _currentJumpCount;     //how many times the player has jumped

    private Rigidbody2D _rigidbody;
    private float _moveInput;
    private Rigidbody2D _groundedBody;
    private Vector2 _groundNormal;
    private int _currentJumpCount;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // sets movement input from player or AI controller
    public void SetMoveInput(float input)
    {
        _moveInput = input;
    }

    // attempt a jump
    public bool TryJump(bool force = false)
    {
        // only jump if jumps are left (for multi/air jumps)
        if(force || _currentJumpCount < _jumpCount)
        {
            // sets vertical velocity directly, multiplies horizontal velocity
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * _jumpHorizontalVelocityMultiplier, _jumpVelocity);
            IsJumping = true;
            _currentJumpCount++;
            return true;
        }

        return false;
    }

    private void Update()
    {
        // reset jump count if grounded
        IsGrounded = GroundCheck();
        if(IsGrounded && !IsJumping) _currentJumpCount = 0;
    }

    private void FixedUpdate() 
    {
        //Clamp velocity
        if(_rigidbody.velocity.magnitude > _terminalVelocity){
            _rigidbody.velocity = _rigidbody.velocity.normalized * _terminalVelocity;
        }

        // set IsJumping false if character is falling
        if(_rigidbody.velocity.y < 0f) IsJumping = false;

        // added gravity force if not grounded
        if(!IsGrounded) _rigidbody.AddForce(new Vector2(0f, _gravity));

        // check for minimum move input
        bool hasMoveInput = Mathf.Abs(_moveInput) >= 0.05f;
        if(!hasMoveInput) 
        {
            // decelerate to a stop or match moving platform velocity if not trying to move
            if(IsGrounded && !IsJumping) _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, GetGroundedBodyVelocity(), Time.fixedDeltaTime * _drag);
            return;
        }

        // accelerate using ground accel if grounded and not jumping
        float acceleration = IsGrounded && !IsJumping ? _groundAcceleration : _airAcceleration;
        AccelerateToVelocity(_moveInput * _maxVelocity, acceleration);
    }

    // accelerate up to given velocity with given acceleration, uses ground normal for slopes
    private void AccelerateToVelocity(float velocity, float acceleration)
    {
        // target takes velocity of ground and slope direction into account
        Vector2 targetVelocity = GetGroundedBodyVelocity() + (Vector2)(velocity * Vector3.Cross(Vector3.forward, -_groundNormal));
        Vector2 currentVelocity = _rigidbody.velocity;
        Vector2 velocityDifference = targetVelocity - currentVelocity;
        Vector2 finalAcceleration = velocityDifference * acceleration;
        
        // no vertical acceleration if character is falling or jumping
        if(!IsGrounded || IsJumping) finalAcceleration.y = 0f;

        _rigidbody.AddForce(finalAcceleration);
    }

    // check for the ground from character position and offset
    private bool GroundCheck()
    {
        // set up ground check ray
        Vector2 verticalOffset = Vector2.up * _groundCheckOffset;
        Vector2 origin = (Vector2)transform.position + verticalOffset;
        Vector2 direction = Vector2.down;
        RaycastHit2D raycastHit;

        // check for ground
        raycastHit = Physics2D.CircleCast(origin, _groundCheckRadius, direction, _groundCheckDistance, _groundCheckMask);
        if(raycastHit.collider != null)
        {
            // check slope angle
            float slopeAngle = Vector2.Angle(Vector2.up, raycastHit.normal);
            if(slopeAngle < _maxSlopAngle)
            {
                // ground found
                if(raycastHit.collider.attachedRigidbody != null) _groundedBody = raycastHit.collider.attachedRigidbody;
                _groundNormal = raycastHit.normal;
                return true;
            }
        }
        
        // not grounded
        _groundedBody = null;
        _groundNormal = Vector3.up;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // reduce jump count if WallJump object is hit
        if(other.gameObject.CompareTag(_wallJumpTag))
        {
            _currentJumpCount = _jumpCount - _wallJumpAllowance; 
        }
    }

    // returns velocity of rigidbody character is standing on
    private Vector2 GetGroundedBodyVelocity()
    {
        return _groundedBody != null ? _groundedBody.velocity : Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        // draw start and end circles for ground check
        Gizmos.color = Color.yellow;
        Vector2 verticalOffset = Vector2.up * _groundCheckOffset;
        Vector2 start = (Vector2)transform.position + verticalOffset;
        Vector2 end = start + Vector2.down * _groundCheckDistance;
        Gizmos.DrawWireSphere(start, _groundCheckRadius);
        Gizmos.DrawWireSphere(end, _groundCheckRadius);
    }
}