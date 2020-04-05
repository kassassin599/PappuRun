using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClearPrefs : MonoBehaviour
{
   
    [MenuItem("Tools/Clear PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Hearts");
    }
}
