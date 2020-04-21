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

    [SerializeField]
    GameObject titleImage;

    [SerializeField]
    public GameObject _maskActionButton;

    public PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        GameManager.OnPlayerDead += OnPlayerDead;
        //GameManager.OnMaskPicked += OnMaskPicked;
        player.OnMaskPickup += OnMaskPicked;

        if (FindObjectOfType<EnemyAI>())
        {
            //Do nothing
        }
        else
        {
            maskImage.SetActive(false);
            numOfMaskText.SetActive(false);
        }

        titleImage.SetActive(true);
        StartCoroutine(TitleImage());
    }

    IEnumerator TitleImage()
    {
        yield return new WaitForSeconds(2f);
        titleImage.SetActive(false);
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

    public void MaskActionButton()
    {
        player.MaskActionButtonPressed();
        numOfMaskText.GetComponent<TextMeshProUGUI>().text = "X" + FindObjectOfType<PlayerController>().numOfMask;
        _maskActionButton.SetActive(false);
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
    }

    public void HomeButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartLevelButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDead -= OnPlayerDead;
        //GameManager.OnMaskPicked -= OnMaskPicked;
        player.OnMaskPickup -= OnMaskPicked;
    }
}
