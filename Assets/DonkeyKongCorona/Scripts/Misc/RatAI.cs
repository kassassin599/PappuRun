using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : MonoBehaviour
{
    [SerializeField]
    float enemySpeed = 5f;

    [SerializeField]
    bool moveRight = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    LayerMask playerLayerMask;

    bool sittingIdle = false;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        StartCoroutine(MoveRight());
        StartCoroutine(DetectPlayer());
    }

    IEnumerator MoveRight()
    {
        while (true)
        {
            if (!sittingIdle)
            {
                transform.Translate(Vector3.left * Time.deltaTime * enemySpeed);

                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 1, layerMask);
                if (hitInfo.collider != null)
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
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
                else
                {
                    Debug.DrawLine(transform.position, transform.position + transform.right * -1, Color.green);

                }
            }

            yield return null;
        }
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 10, playerLayerMask);
            if (hitInfo.collider != null)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                if (!sittingIdle)
                {
                    GetComponent<Animator>().SetBool("Idle", true);
                    sittingIdle = true;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + transform.right * -10, Color.blue);
                enemySpeed = 3f;
                GetComponent<Animator>().SetBool("Idle", false);
                sittingIdle = false;
            }

            yield return null;
        }
    }
}
