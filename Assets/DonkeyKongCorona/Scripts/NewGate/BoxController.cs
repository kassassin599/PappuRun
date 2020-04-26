using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{

    public float speed = 3.0f;

    Rigidbody2D rigidbody2D;

    bool goRight = false;
    public bool goLeft = false;

    [SerializeField]
    LayerMask pressurePlateMask;

    bool pressurePlatePressed = false;

    GameObject player;
    GameObject newGate;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        newGate = GameObject.Find("NewGate");
        //GetComponent<Rigidbody2D>().gravityScale = 0;
        //GetComponent<Collider2D>().enabled = false;
    }

    private void Update()
    {
        InputUpdate();
    }

    private void FixedUpdate()
    {
        if (!pressurePlatePressed)
        {
            RayUpdate();
        }
    }

    private void RayUpdate()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up*-1 , 2, pressurePlateMask);
        if (hitInfo.collider != null)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Collider2D>().enabled = false;
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            hitInfo.collider.GetComponent<Animator>().Play("PressurePlatePressed");
            pressurePlatePressed = true;
            newGate.GetComponent<Collider2D>().enabled = false;
            newGate.GetComponent<Animator>().Play("NewGateOpen");
            player.GetComponent<PlayerController>().PlayerOutHeadsetReset();
           
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * -1 * 2, Color.green);
            
        }
    }

    private void InputUpdate()
    {
        if (goLeft)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else if (goRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
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
    public void UpdateBoxPhysics()
    {
        GetComponent<BoxController>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 3;
        GetComponent<Collider2D>().enabled = true;
    }
}