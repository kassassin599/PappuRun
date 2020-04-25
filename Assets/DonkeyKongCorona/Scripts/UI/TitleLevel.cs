using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLevel : MonoBehaviour
{
    GameObject hud;

    private void Start()
    {
        hud = GameObject.Find("HUD");
    }

    public void HUDEnable()
    {
        hud.SetActive(true);
    }

    public void HUDDisable()
    {
        hud.SetActive(false);
    }
}
