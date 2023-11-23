using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public int maxHp;
    [Header("Variables")]
    public SpriteRenderer bodySpriteRenderer;
    public CameraManager cameraManager;
    public TextMeshProUGUI messageText;
    public WeaponHolder weaponHolder;
    public Material defaultMat;
    public Material flashMat;
    public Image mainHealthBar;
    public Transform projectileSpawnPoint;
    public PlayerMovement playerMovement;
    public GameSaveManager gameSaveManager;
    public Chat chat;
    public DialogManager dialogManager;
    public TextMeshProUGUI zoneText;
    public CharacterManager characterManager;
    public TextMeshPro useText;

    private bool canUse = true;
    private const float maxAttackSpeed = 0.05f;
    private bool isShowingMessage = false;
    private WaitForSeconds messageCharacterDelay = new WaitForSeconds(0.1f);
    private WaitForSeconds messageDelay = new WaitForSeconds(1.5f);
    private WaitForSeconds flashDelay = new WaitForSeconds(0.1f);
    private bool canFlash = true;
    private const float maxXpCoefficient = 20;
    private NPC? collidingNPC = null;
    private UIState uiState = Player.UIState.UNPAUSED;

    private enum UIState
    {
        UNPAUSED,
        PAUSED
    }

    #endregion
    #region Getters & Setters
    public bool CanUse
    {
        get { return canUse; }
        set { canUse = value; }
    }

    #endregion
    private void Start()
    {
        gameSaveManager.Load();
        SceneManager.LoadScene(PlayerPrefs.GetString("sceneToLoad", "Tier1_Map1"), LoadSceneMode.Additive);
    }


    private void Update()
    {
        //Input
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraManager.ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraManager.ZoomOut();
        }
        if (Input.GetKeyDown(KeyCode.F) && collidingNPC != null)
        {
            dialogManager.StartDialog(collidingNPC.avatar,collidingNPC.dialogs);
        }
        if (Input.GetKeyDown(KeyCode.F10)) { Toolkit.GoToScene("EquipmentCharacterSelect"); }
        UpdateUI();
        Debug.Log(PlayerPrefs.GetInt("Straw Hat_unlocked",0));
    }

    public void TakeDamage(int damage)
    {
        Toolkit.PlaySound("Hurt1", 0.35f, 0.8f, 1f);
        characterManager.GetCurrentCharacter().hp -= damage;
        Toolkit.SpawnParticle("Blood", this.transform.position);
        StartCoroutine(Flash());
        if (characterManager.GetCurrentCharacter().hp <= 0)
        {
            characterManager.Die();
        }
    }


    private IEnumerator Flash()
    {
        if (canFlash)
        {
            canFlash = false;
            bodySpriteRenderer.material = flashMat;
            yield return flashDelay;
            canFlash = false;
            bodySpriteRenderer.material = defaultMat;
            canFlash = true;
        }
    }

    public void Die(int index)
    {

    }


    public IEnumerator ShowMessage(string message)
    {
        string text = "";
        if (!isShowingMessage)
        {
            isShowingMessage = true;
            foreach (char c in message)
            {
                text += c;
                messageText.text = text;
                if (c != ' ') { yield return messageCharacterDelay; }
            }
            yield return messageDelay;
            messageText.text = "";
            isShowingMessage = false;
        }
    }

    private void PickupLoot(Collider2D col)
    {
        DroppedLoot dp = col.GetComponent<DroppedLoot>();
        if (PlayerPrefs.GetInt(dp.lootName + "_unlocked", 0) == 0)
        {
            chat.Print("Looted: " + dp.lootName);
            PlayerPrefs.SetInt(dp.lootName + "_unlocked", 1);
        }
        else
        {
            chat.Print(dp.lootName + " is already owned!");
        }
        Destroy(col.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage(col.gameObject.GetComponent<Projectile>().damage);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "DroppedLoot")
        {
            PickupLoot(col);
        }
        if (col.gameObject.GetComponent<ZoneTrigger>() != null)
        {
            ZoneTrigger zoneTrigger = col.gameObject.GetComponent<ZoneTrigger>();
            EnterZone(zoneTrigger);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Object")
        {
            SpriteRenderer spriteRenderer = col.gameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = new Color32(255, 255, 255, 255);
        }
        if (col.gameObject.tag == "NPC")
        {
            collidingNPC = null;
            useText.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "NPC")
        {
            collidingNPC = col.gameObject.GetComponent<NPC>();
            useText.gameObject.SetActive(true);
        }
    }

    public void ShakeCamera(float magnitude)
    {
        StartCoroutine(cameraManager.Shake(magnitude));
    }


    public void UpdateUI()
    {

    }

    private void EnterZone(ZoneTrigger zoneTrigger)
    {
        zoneText.text = zoneTrigger.zoneName;
        zoneText.GetComponent<Animator>().SetTrigger("start");
    }
}
