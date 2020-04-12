using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverUI;

    [SerializeField]
    public GameObject _actionButton;

    [SerializeField]
    public GameObject _damageEffect;

    [SerializeField]
    GameObject maskImage;
    [SerializeField]
    GameObject numOfMaskText;

    private void Start()
    {
        GameManager.OnPlayerDead += OnPlayerDead;
        GameManager.OnMaskPicked += OnMaskPicked;

        if (FindObjectOfType<EnemyAI>())
        {
            //Do nothing
        }
        else
        {
            maskImage.SetActive(false);
            numOfMaskText.SetActive(false);
        }
    }

    private void OnMaskPicked()
    {
        numOfMaskText.GetComponent<TextMeshProUGUI>().text = "X" + FindObjectOfType<PlayerController>().numOfMask;
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
        PlayerPrefs.SetInt("Health", 3);
        PlayerPrefs.SetInt("Hearts", 3);
        PlayerPrefs.SetInt("levelAt", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void WashStationActionButton()
    {
        FindObjectOfType<PlayerController>()._actionButtonPressed = true;
        FindObjectOfType<PlayerController>().WashHands();
    }
}
