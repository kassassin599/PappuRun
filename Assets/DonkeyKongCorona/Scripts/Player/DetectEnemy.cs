using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{

    //public float raycastMaxDistance = 10f;

    //public LayerMask layerMask;

    //private const int OBSTACLE_LAYER = 9;

    //private Animator anim;

    //private Rigidbody2D body;

    //private float originOffset = 0.5f;

    ///// <summary>
    ///// Raycasts out from the player and returns the targets hit.
    ///// </summary>
    ///// <param name="direction"></param>
    //public RaycastHit2D CheckRaycast(Vector2 direction)
    //{
    //    float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);

    //    Vector2 startingPosition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y);

    //    return Physics2D.Raycast(startingPosition, direction, raycastMaxDistance, layerMask);
    //}

    //private void Awake()
    //{
    //    body = GetComponent<Rigidbody2D>();
    //    anim = GetComponent<Animator>();
    //}

    //private void FixedUpdate()
    //{
    //    RaycastCheckUpdate();
    //}
    ///// <summary>
    ///// Checks for user input to create raycasts on update and proceeds to make those raycast checks
    ///// if the button is down
    ///// </summary>
    ///// <returns>True if raycheck is made this frame, false otherwise</returns>
    //private bool RaycastCheckUpdate()
    //{
    //    //// Raycast button pressed
    //    //if (Input.GetButtonDown("Fire1"))
    //    //{
    //        // Launch a raycast in the forward direction from where the player is facing.
    //        Vector2 direction = new Vector2(1, 0);

    //        // If facing left, negative direction
    //        if (GetComponent<Transform>().rotation.eulerAngles.y == -180)
    //            direction *= -1;

    //        // First target hit
    //        RaycastHit2D hit = CheckRaycast(direction);

    //        if (hit.collider)
    //        {
    //            Debug.Log("Hit the collidable object " + hit.collider.name);

    //            Debug.DrawRay(transform.position, hit.point, Color.red, 3f);
    //        }

    //        return true;
    //    //}
    //    //else
    //    //{
    //    //    return false;
    //    //}
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CoronaEnemy"))
        {
            FindObjectOfType<InGameUI>()._actionButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CoronaEnemy"))
        {
            FindObjectOfType<InGameUI>()._actionButton.SetActive(false);
        }
    }
}