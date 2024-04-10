using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI dialogText;
    public Image dialogAvatar;
    public GameObject dialogPanel;

    private string[] dialogs;
    private int dialogIndex = -1;
    private WaitForSeconds charDelay = new WaitForSeconds(0.05f);
    private bool isWriting = false;
    private NPC? collidingNPC = null;
    private Player player;
    private void Start()
    {
        player = this.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && collidingNPC != null)
        {
            StartDialog(collidingNPC.avatar, collidingNPC.dialogs);
        }
    }
    public void StartDialog(Sprite avatar,string[] dialogs)
    {
        if (!dialogPanel.activeSelf)
        {
            dialogPanel.SetActive(true);
            this.dialogs = dialogs;
            dialogAvatar.sprite = avatar;
            dialogIndex = -1;
            dialogText.text = "";
            ShowNext();
            Debug.LogWarning("Dialog opened");
        }
    }

    public void ShowNext()
    {
        if (!isWriting)
        {
            dialogIndex++;
            if (dialogIndex >= dialogs.Length)
            {
                Close();
                return;
            }
            StartCoroutine(WriteTextAnimation(dialogs[dialogIndex]));
        }
    }

    private void Close()
    {
        dialogPanel.SetActive(false);
    }

    private IEnumerator WriteTextAnimation(string text)
    {
        isWriting = true;
        string currentText = "";
        foreach (char c in text)
        {
            currentText += c;
            if (c != ' ') 
            { 
                yield return charDelay; 
            }
            dialogText.text = currentText;
        }
        isWriting = false;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<NPC>() != null)
        {
            collidingNPC = null;
            player.useText.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<NPC>() != null)
        {
            NPC npc = col.gameObject.GetComponent<NPC>();
            collidingNPC = npc;
            player.useText.gameObject.SetActive(true);
        }
    }
}
