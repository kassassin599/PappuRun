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

    public bool _maskOn = false;

    [SerializeField]
    LayerMask layerMask;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
        StartCoroutine(MoveRight());
    }

    IEnumerator MoveRight()
    {
        while (true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * enemySpeed);

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right *-1, 1, layerMask);
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

            yield return null;
        }
    }

    public void MaskOn()
    {
        _maskOn = true;

        GetComponent<Animator>().SetBool("MaskOn", true);
    }
}
