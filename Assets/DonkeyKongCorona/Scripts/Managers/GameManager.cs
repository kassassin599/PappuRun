using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public delegate void PlayerDead();
    public static event PlayerDead OnPlayerDead;

    public delegate void Action(int action);
    public static event Action OnAction;


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

    public void ActiveActionButton(int actionObject)
    {
        OnAction(actionObject);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Health", 3);
        PlayerPrefs.SetInt("Hearts", 3);
    }
}

