using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    [SerializeField]
    Image[] hearts;
    [SerializeField]
    Sprite fullHeart;
    [SerializeField]
    Sprite emptyHeart;

    //float directionX;

    public float speed = 3.0f;

    public float uchalneKiTikat = 10.0f;

    Rigidbody2D rigidbody2D;

    bool goRight = false;
    bool goLeft = false;
    bool isJumping = false;

    Animator playerAnimator;

    bool isGrounded;
    [SerializeField]
    Transform feetPos;
    [SerializeField]
    float checkRadius;
    [SerializeField]
    LayerMask whatIsGround;

    [SerializeField]
    Sprite damagedChair;

    bool hasKeyToLift= false;
    bool closeLift = false;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateHealthUI();

        if (goLeft)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            playerAnimator.SetBool("Run", true);
        }
        else if (goRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            playerAnimator.SetBool("Run", true);
        }
        else
        {
            playerAnimator.SetBool("Run", false);
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //if (isJumping)
        //{
        //    transform.Translate(Vector3.up * Time.deltaTime * uchalneKiTikat);
        //}

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime * uchalneKiTikat);
        }
#endif
    }

    private void UpdateHealthUI()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void LeftButtonDown()
    {
        goLeft = true;
        transform.rotation = Quaternion.Euler(0, -180, 0);
    }

    public void LeftButtonUp()
    {
        goLeft = false;
    }

    public void RightButtonDown()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        goRight = true;
    }

    public void RightButtonUp()
    {
        goRight = false;
    }

    public void JumpButtonDown()
    {
        isJumping = true;
    }

    public void JumpButtonUp()
    {
        isJumping = false;
    }

    public void JumpPlayer()
    {
        if (isGrounded)
        {
            rigidbody2D.velocity = Vector2.up * uchalneKiTikat;
            playerAnimator.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            health--;
            if (health<=0)
            {
                GameManager.Instance.IsPlayerDead();
            }
        }
        else if (collision.CompareTag("Wash"))
        {
            numOfHearts++;
            health = numOfHearts;
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (collision.CompareTag("Health"))
        {
            health++;
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Key"))
        {
            hasKeyToLift = true;
            collision.gameObject.SetActive(false);
        }else if (collision.CompareTag("Lift"))
        {
            if (hasKeyToLift)
            {
                collision.GetComponent<Animator>().SetBool("Open", true);
                StartCoroutine(GotoNextLevel());
            }
        }
    }

    IEnumerator GotoNextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        closeLift = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            health--;
            if (health <= 0)
            {
                GameManager.Instance.IsPlayerDead();
            }
            collision.collider.GetComponent<SpriteRenderer>().sprite = damagedChair;
            collision.collider.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lift"))
        {
            if (closeLift)
            {
                collision.GetComponent<Animator>().SetBool("Open", false);
                StartCoroutine(LoadNextStage());
            }
        }
    }

    IEnumerator LoadNextStage()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
