using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Variables")]
    public string name;
    public string[] dialogs;
    public Sprite avatar;

    [Header("Components")]
    public TextMeshPro nameText;

    private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        nameText.text = name;
    }

    public void StartDialog()
    {
        
    }

}
