using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(Toolkit.GoToScene(sceneName));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
