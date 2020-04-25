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
    
    [SerializeField]
    LayerMask playerLayerMask;

    bool speedIncresed = false;

    [SerializeField]
    GameObject medicalTeamPrefab;

    public bool reachedToEnemy;
    
    bool reachedToBase;

    Vector2 basePos;

    GameObject medicalTeam;

    private void Start()
    {
        medicalTeam = Instantiate(medicalTeamPrefab, new Vector2(transform.position.x, transform.position.y+100), Quaternion.identity);
        Physics2D.queriesStartInColliders = false;
        StartCoroutine(MoveRight());
        StartCoroutine(DetectPlayer());
    }

    IEnumerator MoveRight()
    {
        while (!_maskOn)
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

    IEnumerator DetectPlayer()
    {
        while (!_maskOn)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right * -1, 100, playerLayerMask);
            if (hitInfo.collider != null)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                if (!speedIncresed)
                {
                    enemySpeed *= 2;
                    speedIncresed = true;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + transform.right * -100, Color.blue);
                enemySpeed = 1f;
                speedIncresed = false;
            }

            yield return null;
        }
    }

    public void MaskOn()
    {
        _maskOn = true;

        GetComponent<Animator>().SetBool("MaskOn", true);

        medicalTeam.transform.position = new Vector2(transform.position.x, transform.position.y + 10);

        StartCoroutine(TeamToEnemy());
    }

    IEnumerator TeamToEnemy()
    {
        while (!reachedToEnemy)
        {
            if (Vector2.Distance(medicalTeam.transform.position, transform.position)>0.1)
            {
                medicalTeam.transform.Translate(Vector3.down * Time.deltaTime * (enemySpeed+2));
            }
            else
            {
                reachedToEnemy = true;
                medicalTeam.GetComponent<Animator>().SetBool("Pick", true);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
            }
            yield return null;
        }
    }

    public void PickedUp()
    {
        medicalTeam.GetComponent<Animator>().SetBool("Pick", false);
        basePos = new Vector2(medicalTeam.transform.position.x, medicalTeam.transform.position.y + 10);
        StartCoroutine(TeamToBase());
    }

    IEnumerator TeamToBase()
    {
        while (!reachedToBase)
        {
            if (Vector2.Distance(medicalTeam.transform.position, basePos) > 0.1)
            {
                medicalTeam.transform.Translate(Vector3.up * Time.deltaTime * (enemySpeed+2));
            }
            else
            {
                reachedToBase = true;
                Destroy(medicalTeam);
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
}
