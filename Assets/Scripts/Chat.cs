using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Chat : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI chatText;

    public void Print(string text)
    {
        chatText.text += "[" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "] " + text + "\n";
    }

    public void Clear()
    {
        chatText.text = string.Empty;
    }
}
