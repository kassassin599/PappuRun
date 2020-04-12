using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    float enemySpeed = 5f;

    [SerializeField]
    bool moveRight = true;

    [SerializeField]
    Transform detectChair;

    private void Start()
    {
        StartCoroutine(MoveRight());
    }

    IEnumerator MoveRight()
    {
        while (true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * enemySpeed);

            RaycastHit2D frontInfo = Physics2D.Raycast(detectChair.position, Vector2.left, 0.01f);
            if (frontInfo.collider == true)
            {
                if (/*frontInfo.collider.CompareTag("Enemy") || */frontInfo.collider.CompareTag("Wall"))
                {

                    if (moveRight == true)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        moveRight = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        moveRight = true;
                    }
                }
            }

            yield return null;
        }
    }
}
