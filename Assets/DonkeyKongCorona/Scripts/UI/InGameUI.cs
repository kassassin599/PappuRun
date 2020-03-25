using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverUI;

    private void Start()
    {
        GameManager.OnPlayerDead += OnPlayerDead;
    }

    private void OnPlayerDead()
    {
        if (gameOverUI!=null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
