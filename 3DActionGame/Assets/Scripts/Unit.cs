using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _knockbackForce = 50f;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private int _team = 0;

    [Header("Movement")]
    [SerializeField] protected float _moveVelocity = 5f;
    [SerializeField] private float _moveAcceleration = 10f;
    [SerializeField] private float _moveDeaceleration = 20f;
    [SerializeField] private float _airAcceleration = 3f;
    [SerializeField] private float _groundedGravity = -20f;
    [SerializeField] private float _airGravity = -20f;
    [SerializeField] private float _jumpVelocity = 10f;
    [SerializeField] private float _turnLerp = 5f;          //Rotation velocity value
    

    [Header("Grounding")]
    [SerializeField] private float _groundCheckDistance = 0.4f;
    [SerializeField] private float _groundCheckOffset = 0.3f;
    [SerializeField] private float _groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask _groundCheckMask;

    //[Header("Sounds")]
   // [SerializeField] private AudioSource _jump;
   //[SerializeField] private AudioSource _grassRun;
    

    //Properties
    public int Team => _team;
    public Vector3 MoveInput { get; protected set; }
    public Vector3 GroundNormal  {get; private set; }
    public Vector3 GroundForward { get; private set; }
    public Vector3 FlattenedForward { get; private set; }  
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAlive => _currentHealth > 0f;

    //Unity components
    private Rigidbody _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void TryAtttack(){
        if(IsGrounded && IsAlive){
            _animator.SetTrigger("Attack");
        }
    }

    protected void TryJump(){
        if(IsGrounded && IsAlive){
            IsJumping = true;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpVelocity, _rigidbody.velocity.z);
            _animator.SetTrigger("Jump");
            //_jump.Play();
        }
    }

    protected virtual void Update(){
        GroundCheck();

        //Animations
        float moveSpeed = Vector3.Dot(GroundForward, _rigidbody.velocity) / _moveVelocity;
        _animator.SetFloat("Speed", moveSpeed, 0.1f, Time.deltaTime); 
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("VerticalVelocity", _rigidbody.velocity.y);
    }

    private void FixedUpdate(){
        if(_rigidbody.velocity.y <0) IsJumping = false;

        float gravity = !IsGrounded || IsJumping ? _airGravity : _groundedGravity;
        _rigidbody.AddForce(Vector3.up * gravity);

        //Stop if dead
        if(!IsAlive){
            AccelerateToVelocity(Vector3.zero, _moveDeaceleration);
            return;
        }

        bool isMoving = MoveInput.magnitude > 0.05f;
        if(MoveInput.magnitude > 1f) MoveInput = MoveInput.normalized;

        float acceleration = 0f;                                                        //No input in the air
        
        if(isMoving){ 
        acceleration = _moveAcceleration;

            //if(_grassRun.isPlaying == false && IsGrounded){
           //     _grassRun.Play();
           // }
        
        }                                  //Moving on ground

        if(IsGrounded && !isMoving) acceleration = _moveDeaceleration;                  //Not moving in ground
        if((!IsGrounded && IsJumping) && isMoving) acceleration = _airAcceleration;     //Moving in the air

        float velocity = _moveVelocity;
        if(_animator.GetBool("OverrideMovement")) velocity = 0f;

        AccelerateToVelocity(MoveInput * velocity, acceleration);
        TurnInDirection(MoveInput);
    }

    private void AccelerateToVelocity(Vector3 targetVelocity, float acceleration){
        Vector3 currentVelocity = _rigidbody.velocity;
        targetVelocity = GroundForward * targetVelocity.magnitude;
        Vector3 velocityDiff = targetVelocity - currentVelocity;
        Vector3 appliedAcceleration = velocityDiff * acceleration;

        if(!IsGrounded || IsJumping) appliedAcceleration.y = 0f;

        _rigidbody.AddForce(appliedAcceleration);
        
    }

    private void TurnInDirection(Vector3 direction){
        if(direction.magnitude < 0.25f) return;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(FlattenedForward, Vector3.up);
        Quaternion lerpedRotation = Quaternion.Slerp(currentRotation, targetRotation, _turnLerp * Time.fixedDeltaTime); //Slerp helps with the rotation velocity when the rigidbody is roating from 0 to 180 degrees

        _rigidbody.MoveRotation(lerpedRotation);
    }

    private void GroundCheck(){
        Vector3 origin = transform.position + Vector3.up * _groundCheckOffset;
        Vector3 direction = Vector3.down;
        RaycastHit hitInfo;

        bool groundHit = Physics.SphereCast(origin, _groundCheckRadius,direction, out hitInfo, _groundCheckDistance, _groundCheckMask);
        if(groundHit){
            GroundNormal = hitInfo.normal;
        }else{
            GroundNormal = Vector3.up;
        }

        Vector3 movementRightDirection = Vector3.Cross(Vector3.up, MoveInput.normalized);
        GroundForward = Vector3.Cross(-GroundNormal, movementRightDirection).normalized; 
        FlattenedForward = Vector3.Cross(Vector3.down, movementRightDirection);

        IsGrounded = groundHit;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 rayStart = transform.position + Vector3.up * _groundCheckOffset;
        Vector3 rayEnd = rayStart + Vector3.down * _groundCheckDistance;
        Gizmos.DrawWireSphere(rayStart, _groundCheckRadius);
        Gizmos.DrawWireSphere(rayEnd, _groundCheckRadius);
    }

    public void MeleeWeaponHit(Unit victim){
        Vector3 dirToVictim = (victim.transform.position - transform.position).normalized;
        Vector3 knockback = dirToVictim * _knockbackForce;
        victim.Damage(_damage, knockback);
    }

    public void Damage(float amount, Vector3 knockback){
        _rigidbody.AddForce(knockback);
        _animator.SetTrigger("Hit");
        _currentHealth -= amount;
        if(_currentHealth <= 0f){
            Death();

        }
    }
    protected virtual void Death(){
        _animator.SetBool("IsDead", true);
    }
}
