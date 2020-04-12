using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{

    public Button[] lvlButtons;

    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt",1);
        //print(levelAt);
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i+1>levelAt)
            {
                lvlButtons[i].GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
                lvlButtons[i].interactable = false;
            }
            else
            {
                lvlButtons[i].GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(true);
            }
        }
    }
}
