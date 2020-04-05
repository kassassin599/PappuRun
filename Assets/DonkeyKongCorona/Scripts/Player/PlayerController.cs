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

        //if (!isPoweringUp)
        //{
            //if (goLeft)
            //{
            //    transform.Translate(Vector3.right * Time.deltaTime * speed);
            //    playerAnimator.SetBool("Run", true);
            //}
            //else if (goRight)
            //{
            //    transform.Translate(Vector3.right * Time.deltaTime * speed);
            //    playerAnimator.SetBool("Run", true);
            //}
            //else
            //{
            //    playerAnimator.SetBool("Run", false);
            //}
        //}

        //if (isJumping)
        //{
        //    jumpTimeCounter -= Time.deltaTime;
            
        //}

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

    public void JumpButtonDown()
    {
        isJumping = true;
    }

    public void JumpButtonUp()
    {
        isJumping = false;
    }

    //public void JumpPowerUp()
    //{
    //    playerAnimator.SetBool("JumpPower", true);
    //    isPoweringUp = true;

    //    jumpTimeCounter = jumpTime;
    //    isJumping = true;

        
    //}

    public void JumpPlayer()
    {

        if (isGrounded)
        {
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            //{
            rigidbody2D.AddForce(Vector2.up * uchalneKiTikat, ForceMode2D.Impulse);
            secondJumpAvail = true;
            //GetComponent<Collider2D>().enabled = false;
            //GetComponent<AudioSource>().Play();
            playerAnimator.SetTrigger("Jump");
            //}
            //StartCoroutine(cameraShake.Shake(0.05f, 0.05f));

        }
        else
        {
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            //{
            if (secondJumpAvail)
            {
                rigidbody2D.AddForce(Vector2.up * uchalneKiTikat, ForceMode2D.Impulse);
                secondJumpAvail = false;
                //GetComponent<AudioSource>().Play();
                //bheemAnimator.SetTrigger("Jump");
            }
            //}
        }
        //if (isGrounded && isJumping)
        //{
        //    playerAnimator.SetBool("JumpPower", false);
        //    isPoweringUp = false;
        //    if (jumpTimeCounter<0)
        //    {
        //        print(uchalneKiTikat);
        //        rigidbody2D.velocity = Vector2.up * uchalneKiTikat;
        //    }
        //    else
        //    {
        //        uchalneKiTikat = 10;
        //        rigidbody2D.velocity = Vector2.up * uchalneKiTikat;
        //    }
        //    playerAnimator.SetTrigger("Jump");

        //    isJumping = false;
        //    uchalneKiTikat = 15;
        //}
        //else
        //{
        //    isJumping = false;
        //}
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
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
    }

    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.IsPlayerDead();
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
}
