using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrawlMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void GoToMap(string sceneName)
    {
        PlayerPrefs.SetString("current_map", sceneName);
        StartCoroutine(Toolkit.GoToMap(sceneName));
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(Toolkit.GoToScene(sceneName));
    }
}
