using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public delegate void PlayerDead();
    public static event PlayerDead OnPlayerDead;
    
    public delegate void MaskPicked();
    public static event MaskPicked OnMaskPicked;

    public delegate void Action(int action);
    public static event Action OnAction;

    [SerializeField]
    AudioSource bgAudioSource;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance == null)
            {
                instance = value;
            }

        }
    }

    void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            Debug.Log("Deleted - GameManager");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Debug.Log("Game Manager Instance Created");
        }
    }

    public void IsPlayerDead()
    {
        OnPlayerDead();
    }

    public void PlayerPickedMask()
    {
        OnMaskPicked();
    }

    public void ActiveActionButton(int actionObject)
    {
        OnAction(actionObject);
    }

    public void StopBGMusic()
    {
        bgAudioSource.Pause();
    }

    public void ResumeBGMusic()
    {
        bgAudioSource.Play();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Health", 3);
        PlayerPrefs.SetInt("Hearts", 3);
    }
}

