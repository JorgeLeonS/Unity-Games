using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerID;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private int _maxArrows = 5;
    [SerializeField] private float _stompDistance = 0.3f;

    [Header ("Sound")]
    [SerializeField] private AudioSource _jump;
    [SerializeField] private AudioSource _doubleJump;
    [SerializeField] private AudioClip _deathSound;

    public bool IsDead{get; private set;}
    public float AmmoPercentage => (float)_currentArrows / _maxArrows;

    private CharacterController2D _characterController;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _currentArrows;


    //Similar to Start but called before
    private void Awake()
    {
        _characterController  = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _currentArrows = _maxArrows;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Initialize player values
    public void Initialized(int playerID, Color color){
        _spriteRenderer.color = color;
        _playerID = playerID;
    }

    // Update is called once per frame
    void Update()
    {
        //Get aim/move input
        Vector2 input = new Vector2(Input.GetAxis("Horizontal" + _playerID), Input.GetAxis("Vertical" + _playerID));
        //Get aim
        bool aiming = Input.GetButton("Shoot" + _playerID) && _currentArrows > 0;

        //move using horizontal input
        float movement = input.x;
        //Move using horizontal input(stop moving if aiming, or dead)
        if(aiming || IsDead) movement = 0f;
        _characterController.SetMoveInput(movement);

        //Jump
        if(!IsDead && Input.GetButtonDown("Jump" + _playerID)){
            bool jumped = _characterController.TryJump();
            if(jumped){
                _animator.SetTrigger("Jump");
                if(_characterController.JumpsCount == 1){
                     _jump.Play();
                }else{
                    _doubleJump.Play();
                }
               
            }
        }

        //Shoot
        if(!IsDead && _currentArrows > 0 && Input.GetButtonUp("Shoot" + _playerID)){

            _currentArrows--;

            //Spawn arrow
            Vector3 spawnPosition = transform.GetChild(0).position;
            Vector2 aimDirection = input.normalized;
            //If we have a little or no aim input we aim in direction player is facing

            if(input.magnitude < 0.05f){
                //If the sprite flipped (facing left), we aim left, otherwise we aim right
                aimDirection = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
            }
            Quaternion spawnRotation = Quaternion.LookRotation(Vector3.forward, aimDirection);

            Instantiate(_arrowPrefab, spawnPosition, spawnRotation);
        }

        //Animation
        _animator.SetFloat("Movement", Mathf.Abs(input.x));
        _animator.SetBool("Grounded", _characterController.IsGrounded);
        _animator.SetBool("Dead", IsDead);
        _animator.SetBool("Aiming", aiming);

        //Flip sprite left
        if(!IsDead && Mathf.Abs(input.x) > 0.05f) 
            _spriteRenderer.flipX = input.x < 0f;
    }
    public void Damage(){
        IsDead = true;
        _currentArrows = 0;
        gameObject.layer = LayerMask.NameToLayer("Ghost");

        AudioManager.PlayClip(_deathSound, 0.25f);
    }
    //Add arrow
    public bool AddArrow(){
        //Check if arrows are maxed
        if(_currentArrows >= _maxArrows) return false;

        //otherwise, add an arrow
        _currentArrows++;
        return true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player otherPlayer = other.gameObject.GetComponent<Player>();
        if(otherPlayer == null){
            return;
        }
        //Check if player is on top of other
        if(transform.position.y < otherPlayer.transform.position.y){
            return;
        }
        //Check horizontal ditance vs stomp distance
        float horizontalDistance = Mathf.Abs(transform.position.x - otherPlayer.transform.position.x);
        if(horizontalDistance < _stompDistance){
            otherPlayer.Damage();
            _characterController.TryJump(true);
        }
    }
}
