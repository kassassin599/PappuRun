using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField]
    GameObject messagebox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "Lift")
        {
            if (collision.CompareTag("Player"))
            {
                if (!collision.GetComponent<PlayerController>()._hasKeyToLift)
                {
                    messagebox.SetActive(true);
                }
            }
        }
        else
        {
            messagebox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Lift")
        {
            if (collision.CompareTag("Player"))
            {
                if (!collision.GetComponent<PlayerController>()._hasKeyToLift)
                {
                    messagebox.SetActive(false);
                }
            }
        }
        else
        {
            messagebox.SetActive(false);
        }
    }
}
