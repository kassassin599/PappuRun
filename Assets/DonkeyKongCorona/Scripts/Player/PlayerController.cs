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
    //bool isJumping = false;
    bool isPoweringUp = false;

    Animator playerAnimator;

    bool isGrounded;
    [SerializeField]
    Transform feetPos;
    [SerializeField]
    float checkRadius;
    [SerializeField]
    LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public bool secondJumpAvail = false;

    [SerializeField]
    Sprite damagedChair;

    public bool _hasKeyToLift= false;
    bool closeLift = false;

    public bool _actionButtonPressed = false;

    [SerializeField]
    GameObject deathImage;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Health") && PlayerPrefs.HasKey("Hearts"))
        {
            numOfHearts = PlayerPrefs.GetInt("Hearts");
            health = PlayerPrefs.GetInt("Health");
        }
        else
        {
            numOfHearts = 3;
            health = 3;
            PlayerPrefs.SetInt("Health", health);
            PlayerPrefs.SetInt("Hearts", numOfHearts);
        }
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        _actionButtonPressed = false;
    }

    private void Update()
    {
        UpdateHealthUI();

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

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

    private void FixedUpdate()
    {
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

    public void JumpPlayer()
    {

        if (isGrounded)
        {
            rigidbody2D.AddForce(Vector2.up * uchalneKiTikat, ForceMode2D.Impulse);
            secondJumpAvail = true;
            playerAnimator.SetTrigger("Jump");
        }
        else
        {
            if (secondJumpAvail)
            {
                rigidbody2D.AddForce(Vector2.up * uchalneKiTikat, ForceMode2D.Impulse);
                secondJumpAvail = false;
                //GetComponent<AudioSource>().Play();
                //bheemAnimator.SetTrigger("Jump");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            health--;
            FindObjectOfType<InGameUI>()._damageEffect.GetComponent<Animator>().Play("DamageEffect");
            PlayerPrefs.SetInt("Health", health);
            if (health<=0)
            {
                playerAnimator.SetBool("IsDead", true);
                StartCoroutine(PlayerDead());
            }
        }
        else if (collision.CompareTag("Wash"))
        {
            FindObjectOfType<InGameUI>()._actionButton.SetActive(true);
            //numOfHearts++;
            //health = numOfHearts;
            //PlayerPrefs.SetInt("Health", health);
            //PlayerPrefs.SetInt("Hearts", numOfHearts);
            //collision.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (collision.CompareTag("Health"))
        {
            if (health<numOfHearts)
            {
                health++;
                PlayerPrefs.SetInt("Health", health);
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.CompareTag("Key"))
        {
            _hasKeyToLift = true;
            collision.gameObject.SetActive(false);
        }else if (collision.CompareTag("Lift"))
        {
            if (_hasKeyToLift)
            {
                collision.GetComponent<Animator>().SetBool("Open", true);
                StartCoroutine(GotoNextLevel());
            }
        }
    }

    IEnumerator GotoNextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        
        closeLift = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            health--;
            FindObjectOfType<InGameUI>()._damageEffect.GetComponent<Animator>().Play("DamageEffect");
            PlayerPrefs.SetInt("Health", health);
            if (health <= 0)
            {
                playerAnimator.SetBool("IsDead", true);
                StartCoroutine(PlayerDead());
                
            }
            collision.collider.GetComponent<SpriteRenderer>().sprite = damagedChair;
            collision.collider.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (collision.collider.CompareTag("Platform"))
        {
            //Vector3 camPos = FindObjectOfType<Camera>().transform.position;
            //FindObjectOfType<Camera>().transform.position = new Vector3(camPos.x, transform.position.y + 2, camPos.z);
            //GetComponent<MoveCameraUp>().StartLerping();
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            playerAnimator.Play("Idle");
        }
    }

    IEnumerator PlayerDead()
    {
        //FindObjectOfType<InGameUI>()._damageEffect.GetComponent<Animator>().Play("DeathEffect");
        deathImage.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(3f);
        GameManager.Instance.IsPlayerDead();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lift"))
        {
            if (closeLift)
            {
                GetComponent<PlayerController>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<Animator>().SetBool("Open", false);
                StartCoroutine(LoadNextStage());
            }
        }
        else if (collision.CompareTag("Wash"))
        {
            if (_actionButtonPressed)
            {
                collision.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void WashHands()
    {
        numOfHearts++;
        health = numOfHearts;
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("Hearts", numOfHearts);
    }

    IEnumerator LoadNextStage()
    {
        PlayerPrefs.SetInt("levelAt", SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wash"))
        {
            FindObjectOfType<InGameUI>()._actionButton.SetActive(false);
        }
    }

    public void MarioDeath()
    {
        rigidbody2D.AddForce(Vector2.up * uchalneKiTikat, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
        FindObjectOfType<CameraFollow>().enabled = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            playerAnimator.Play("Idle");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            playerAnimator.Play("Idle");
        }
    }
}
