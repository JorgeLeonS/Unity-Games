using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _velocity = 10f;
    [SerializeField] private float _deflectionForce = 5f;
    [SerializeField] private float _deflectionAngle = 3f;
    [SerializeField] private GameObject _arrowPickupPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip _shootsound;
    [SerializeField] private AudioClip _wallHitSound;
    [SerializeField] private AudioClip _bodyHitSound;

    private Rigidbody2D _rigidbody;
    private bool _passedThroughShooter;
    private bool _landed;
    private bool _deflected;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * _velocity;

        //Play shoot sound
        AudioManager.PlayClip(_shootsound, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Get move direction
        Vector2 velocity = _rigidbody.velocity;
        //Create a rotation from velocity
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, velocity);
        //Set our rotation
        transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        //Clamp velocity
        if(_rigidbody.velocity.magnitude > _velocity){
            _rigidbody.velocity = _rigidbody.velocity.normalized * _velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Ignore Screenlooper
        if(other.GetComponent<ScreenLooper>() != null) return;

        //Get player component
        Player hitPlayer = other.GetComponent<Player>();

        //Ignore if passing thouth shooter
        if(!_passedThroughShooter && hitPlayer != null){
            _passedThroughShooter = true;
            return;
        }
        
        //Arrow has stuck a player
        if( hitPlayer != null && !hitPlayer.IsDead){
            hitPlayer.Damage();
            Destroy(gameObject);
            return;
        }else if (hitPlayer != null){
            return;
        }

        //Arrow struck another arorw
        Arrow otherArrow = other.GetComponent<Arrow>();
        ArrowPickup otherArrowPickup = other.GetComponent<ArrowPickup>();

        bool otherArrowHit = otherArrow != null || otherArrowPickup != null;
        if(!_deflected && otherArrowHit){ // !variable = variable == false

            _deflected = true;

            //Get direction of deflection
            Vector2 deflectionDirection = (transform.position - other.transform.position).normalized;
            //Random delfection rotation
            float randomAngle = Random.Range(-_deflectionAngle, _deflectionAngle);
            //Random deflection rotation
            Quaternion delfectionRotation = Quaternion.Euler(0f, 0f, randomAngle);
            //Final combined rotation
            transform.rotation =  Quaternion.LookRotation(Vector3.forward, delfectionRotation * deflectionDirection);
            //Set new velocity
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(delfectionRotation * deflectionDirection * _deflectionForce);
            return;
        }else if(otherArrowHit){
             //If we hit other arrow but have already delflect, ignore collision
            return;
        }

        //If already landed, stop
        if(_landed) return;
        _landed = true;

        //Spawn Pickup arrow and attach it to hit collider
        GameObject arrowPickup = Instantiate(_arrowPickupPrefab, transform.position, transform.rotation);
        arrowPickup.transform.SetParent(other.transform);

        //Play wall hit sound
        AudioManager.PlayClip(_wallHitSound, 0.5f);


        Destroy(gameObject);
    }
}
