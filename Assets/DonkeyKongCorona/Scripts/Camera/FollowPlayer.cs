using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;

    private void Update()
    {
        transform.position = new Vector3(0, player.position.y, player.position.z) + offset;
    }
}
