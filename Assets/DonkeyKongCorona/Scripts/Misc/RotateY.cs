using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Rotating());
    }

    IEnumerator Rotating()
    {
        while (true)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
            yield return null;
        }
    }
}
