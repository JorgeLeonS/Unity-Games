using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 _destinationOffset = new Vector2(0f, 3f);
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _velocity = 1f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _timer;
    private Rigidbody2D _rigidbody;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_destinationOffset);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the start and end posiiton
        _startPosition = transform.position;
        _endPosition = _startPosition + (Vector3)_destinationOffset;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Increment our timer
        _timer += Time.fixedDeltaTime * _speed;
        float progress = 1f - Mathf.Abs((_timer - (int)_timer)  * 2f-1f);

        Vector3 targetPosition = Vector3.Lerp(_startPosition, _endPosition, progress);
        Vector3 directionToTarget = targetPosition- transform.position;
        _rigidbody.velocity = directionToTarget.normalized * _velocity;
    }
}
