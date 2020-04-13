using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField]
    GameObject titleImage;

    private void Start()
    {
        titleImage.SetActive(true);
        StartCoroutine(TitleImage());
    }

    IEnumerator TitleImage()
    {
        yield return new WaitForSeconds(5f);
        titleImage.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SecondLevelLoad()
    {
        SceneManager.LoadScene(2);
    }
    public void ThirdLevelLoad()
    {
        SceneManager.LoadScene(3);
    }
    public void FourthLevelLoad()
    {
        SceneManager.LoadScene(4);
    }
    public void FifthLevelLoad()
    {
        SceneManager.LoadScene(5);
    }
    public void SixthLevelLoad()
    {
        SceneManager.LoadScene(6);
    }
    public void SeventhLevelLoad()
    {
        SceneManager.LoadScene(7);
    }
    public void EighthLevelLoad()
    {
        SceneManager.LoadScene(8);
    }
    public void NinthLevelLoad()
    {
        SceneManager.LoadScene(9);
    }
    public void TenthLevelLoad()
    {
        SceneManager.LoadScene(10);
    }
}
