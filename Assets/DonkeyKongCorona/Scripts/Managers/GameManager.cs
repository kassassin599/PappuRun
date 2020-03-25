using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public delegate void PlayerDead();
    public static event PlayerDead OnPlayerDead;


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
}

