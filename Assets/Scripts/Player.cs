using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Reflection;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    public CameraManager cameraManager;
    public TextMeshProUGUI messageText;
    public WeaponHolder weaponHolder;
    public Image mainHealthBar;
    public PlayerMovement playerMovement;
    public GameSaveManager gameSaveManager;
    public Chat chat;
    public DialogManager dialogManager;
    public TextMeshProUGUI zoneText;
    public TextMeshPro useText;
    public InstanceManager instanceManager;
    public GameObject pausePanel;
    public TextMeshProUGUI[] characterNameTexts = new TextMeshProUGUI[3];
    public Image[] characterIcons = new Image[3];
    public Image[] characterHealthBars = new Image[3];
    public Image[] abilityIcons = new Image[3];
    public TextMeshProUGUI[] abilityCooldownTexts = new TextMeshProUGUI[3];
    public Image[] abilityCooldowns = new Image[3];
    public Animator weaponAnimator;
    public TextMeshProUGUI hpBarText;
    public Image hpBar;
    public SpriteRenderer weaponSpriteRenderer;
    public ActionManager actionManager;
    public BoxCollider2D weaponHitbox;
    public Material defaultMat;
    public Material flashMat;
    public SpriteRenderer bodySpriteRenderer;
    public Transform projectileSpawnPoint;
    public GameObject deathScreen;
    public Image killerSprite;
    public Animator bodyAnimator;
    public Tooltip tooltip;
    public Combo combo;
    public TextMeshProUGUI characterSwapText;
    public TextMeshPro interactText;
    public LevelManager levelManager;
    public CharacterManager characterManager;

    private WaitForSeconds weaponHitDelay = new WaitForSeconds(0.05f);
    private bool canFlash = true;
    private WaitForSeconds flashDelay = new WaitForSeconds(0.1f);
    [HideInInspector] public UIState uiState = UIState.UNPAUSED;
    [HideInInspector] public PlayerState playerState = PlayerState.IDLE;
    [HideInInspector] public bool immune = false;
    private GameObject interactableObject;

    public enum UIState
    {
        UNPAUSED,
        PAUSED,
        GAMEOVER
    }

    public enum PlayerState
    {
        IDLE,
        ATTACKING
    }
    #endregion
    private void Start()
    {
        
    }
    private void Update()
    {
        UpdateUI();
    }


    private void PickupLoot(Item droppedLoot)
    {
        Inventory.AddItem(droppedLoot.itemData);
        Destroy(droppedLoot.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<ZoneTrigger>() != null)
        {
            ZoneTrigger zoneTrigger = col.gameObject.GetComponent<ZoneTrigger>();
            EnterZone(zoneTrigger);
        }
        if (col.gameObject.tag == "EnemyProjectile")
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.damage, projectile.entity);
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Interactable")
        {
            interactableObject = null;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Interactable")
        {
            interactableObject = col.gameObject;
        }
    }


    public void ShakeCamera(float magnitude)
    {
        StartCoroutine(cameraManager.Shake(magnitude));
    }

    private void EnterZone(ZoneTrigger zoneTrigger)
    {
        zoneText.text = zoneTrigger.zoneName;
        zoneText.GetComponent<Animator>().SetTrigger("start");
    }

    public void Respawn()
    {
        Toolkit.GoToScene(PlayerPrefs.GetString("currentScene","Menu"));
    }

    public void TogglePause(bool toggle)
    {
        if (toggle && uiState != UIState.GAMEOVER)
        {
            pausePanel.SetActive(true);
            uiState = UIState.PAUSED;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            uiState = UIState.UNPAUSED;
        }
    }


    private void UpdateUI()
    {
        ICharacter currentCharacter = characterManager.GetCurrentCharacter();
        hpBarText.text = currentCharacter.hp + "/" + currentCharacter.maxHp;
        mainHealthBar.fillAmount = (float)((float)currentCharacter.hp / (float)currentCharacter.baseMaxHp);
        for (int i = 0; i < 3; i++)
        {
            characterHealthBars[i].fillAmount = (float)((float)characterManager.characters[i].hp / (float)characterManager.characters[i].baseMaxHp);
        }
        for (int i = 0; i < 3; i++)
        {
            if (currentCharacter.currentAbilityCooldown[i] > 0)
            {
                abilityCooldowns[i].fillAmount = 1;
                abilityCooldownTexts[i].text = currentCharacter.currentAbilityCooldown[i].ToString();
            }
            else { abilityCooldownTexts[i].text = ""; abilityCooldowns[i].fillAmount = 0; }
        }
        if (interactableObject != null)
        {
            if (interactableObject.GetComponent<Item>() != null)
            {
                interactText.text = "[F] Pickup item";
            }
            else
            {
                interactText.text = "";
            }
        }
        else
        {
            interactText.text = "";
        }
    }

    public void Attack()
    {
        if (playerState == PlayerState.IDLE)
        {
            playerState = PlayerState.ATTACKING;
            weaponAnimator.speed = 1 / characterManager.GetCurrentCharacter().GetAttackSpeed();
            weaponAnimator.SetTrigger("attack1");
        }
    }
    public void Hit()
    {
        StartCoroutine(HitIEnum());
    }
    private IEnumerator HitIEnum()
    {
        weaponHitbox.enabled = true;
        weaponHitbox.offset = new Vector2(weaponHitbox.offset.x * -1f, 0);
        yield return weaponHitDelay;
        weaponHitbox.enabled = false;
    }

    public void EndAction()
    {
        playerState = PlayerState.IDLE;
        playerMovement.CanMove = true;
        weaponAnimator.speed = 1;
    }
    public void Die(Enemy killer)
    {
        characterManager.GetCurrentCharacter().isDead = true;
        if (GetDeadCharactersAmount() < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!characterManager.characters[i].isDead)
                {
                    characterManager.SwapCharacter(i);
                    break;
                }
            }
        }
        else
        {
            GameOver(killer);
        }
    }

    private void GameOver(Enemy killer)
    {
        if (uiState != UIState.GAMEOVER)
        {
            uiState = UIState.GAMEOVER;
            bodySpriteRenderer.sprite = null;
            weaponSpriteRenderer.sprite = null;
            deathScreen.gameObject.SetActive(true);
            killerSprite.sprite = Resources.Load<Sprite>("Sprites/Enemies/" + killer.name + "/" + killer.name + "_Idle1");
        }
    }

    private int GetDeadCharactersAmount()
    {
        int count = 0;
        foreach (ICharacter c in characterManager.characters)
        {
            if (c.isDead) { count++; }
        }
        return count;
    }

    public void TakeDamage(int damage, Enemy killer)
    {
        if (!immune)
        {
            ICharacter currentCharacter = characterManager.GetCurrentCharacter();
            Toolkit.PlaySound("Hurt1", 0.35f, 0.8f, 1f);
            currentCharacter.hp -= damage;
            Toolkit.SpawnParticle("Blood", this.transform.position);
            Toolkit.SpawnText(damage.ToString(),new Vector2(this.transform.position.x,this.transform.position.y+0.5f),new Color32(255,0,0,255));
            StartCoroutine(Flash());
            if (currentCharacter.hp <= 0)
            {
                Die(killer);
            }
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

    public void Interact()
    {
        if (interactableObject != null)
        {
            if (interactableObject.GetComponent<Item>() != null)
            {
                PickupLoot(interactableObject.GetComponent<Item>());
                return;
            }
        }
    }
}