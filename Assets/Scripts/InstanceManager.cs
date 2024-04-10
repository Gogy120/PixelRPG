using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstanceManager : MonoBehaviour
{
    [Header("Components")]
    public GameObject panel;
    public Animator panelAnimator;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI tierText;
    public Image instanceImage;

    private Instance? collidingInstance = null;
    private Player player;
    void Start()
    {
        player = this.GetComponent<Player>();
    }


    private void ShowInfo()
    {
        if (collidingInstance != null && !panel.activeSelf)
        {
            panel.SetActive(true);
            nameText.text = collidingInstance.name;
            tierText.text = Toolkit.IntToRoman(collidingInstance.tier);
        }
    }

    private void CloseInfo()
    {
        panelAnimator.SetTrigger("close");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Instance>() != null)
        {
            collidingInstance = null;
            player.useText.gameObject.SetActive(false);
            CloseInfo();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Instance>() != null)
        {
            Instance instance = col.gameObject.GetComponent<Instance>();
            collidingInstance = instance;
            ShowInfo();
        }
    }

    public void Enter(int difficulty)
    {
        if (collidingInstance != null)
        {
            PlayerPrefs.SetInt("instanceSelectedTier", collidingInstance.tier);
            PlayerPrefs.SetInt("instanceSelectedDifficulty", difficulty);
            Toolkit.GoToMap(collidingInstance.name);
        }
    }

    public void Leave()
    {
        Time.timeScale = 1;
        StartCoroutine(Toolkit.GoToScene("Menu"));
    }

}
