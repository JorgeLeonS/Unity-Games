using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLooper : MonoBehaviour
{
    private BoxCollider2D _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Get player or arrow components
        Player otherPlayer = other.GetComponent<Player>();
        Arrow otherArrow = other.GetComponent<Arrow>();

        //Stop if other object isn't a player or arrow
        if (otherPlayer == null && otherArrow == null)
        {
            return;
        }
        Vector2 currentPosition = other.transform.position;

        //Check for horizontal mirroring
        if(currentPosition.x > _boxCollider.bounds.max.x || currentPosition.x < _boxCollider.bounds.min.x){
            Vector2 horizontalMirroredPosition = new Vector2(-currentPosition.x, currentPosition.y);
            horizontalMirroredPosition -= new Vector2(Mathf.Sign(horizontalMirroredPosition.x) * 0.01f, 0f);
            other.transform.position = horizontalMirroredPosition;
        }

        //Check for vertical posiiton
        if (currentPosition.y > _boxCollider.bounds.max.y || currentPosition.y < _boxCollider.bounds.min.y)
        {
            Vector2 verticalMirroredPosition = new Vector2(currentPosition.x, -currentPosition.y);
            verticalMirroredPosition -= new Vector2(0f, Mathf.Sign(verticalMirroredPosition.y) * 0.01f);
            other.transform.position = verticalMirroredPosition;
        }
    }

}
