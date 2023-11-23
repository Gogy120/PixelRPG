using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Components & Resources")]
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
    public AbilityManager abilityManager;
    public BoxCollider2D weaponHitbox;
    public PlayerMovement playerMovement;
    public Player player;

    private int currentCharacterIndex;
    [HideInInspector] public Character[] characters = new Character[3];
    private bool canUse = true;
    private WaitForSeconds weaponHitDelay = new WaitForSeconds(0.05f);
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CastAbility(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CastAbility(3);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SwapCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwapCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SwapCharacter(2);
        }
        UpdateUI();
    }
    public Character GetCurrentCharacter()
    {
        return characters[currentCharacterIndex];
    }

    public void SwapCharacter(int index)
    {
        if (canUse)
        {
            Toolkit.SpawnParticle("Swap", new Vector2(this.transform.position.x, this.transform.position.y + 0.5f));
            currentCharacterIndex = index;
            playerMovement.bodyAnimator.runtimeAnimatorController = GetCurrentCharacter().GetBodyAnimatorController();
            weaponAnimator.runtimeAnimatorController = GetCurrentCharacter().GetWeaponAnimatorController();
            weaponSpriteRenderer.sprite = GetCurrentCharacter().GetWeaponSprite();
            for (int i = 0; i < 3; i++)
            {
                if (currentCharacterIndex == i)
                {
                    characterHealthBars[i].transform.parent.gameObject.SetActive(false);
                }
                else { characterHealthBars[i].transform.parent.gameObject.SetActive(true); }
            }
            for (int i = 0; i < 3; i++)
            {
                abilityIcons[i].sprite = GetCurrentCharacter().GetAbilityIcon(i + 1);
            }
            GetCurrentCharacter().maxHp = GetCurrentCharacter().baseMaxHp + GetCurrentCharacter().equipmentManager.GetTotalHP();
            if (!GetCurrentCharacter().hasSwitched)
            {
                GetCurrentCharacter().hp = GetCurrentCharacter().maxHp;
                GetCurrentCharacter().hasSwitched = true;
            }
        }
    }

    private void CastAbility(int ability)
    {
        if (canUse && GetCurrentCharacter().currentAbilityCooldown[ability - 1] == 0)
        {
            canUse = false;
            StartCoroutine(GetCurrentCharacter().StartAbilityCooldown(ability - 1));
            abilityManager.StartCoroutine(GetCurrentCharacter().name + "_Ability" + ability);
        }
    }

    private void UpdateUI()
    {
        hpBarText.text = GetCurrentCharacter().hp + "/" + GetCurrentCharacter().maxHp;
        player.mainHealthBar.fillAmount = (float)((float)GetCurrentCharacter().hp / (float)GetCurrentCharacter().baseMaxHp);
        for (int i = 0; i < 3; i++)
        {
            characterHealthBars[i].fillAmount = (float)((float)characters[i].hp / (float)characters[i].baseMaxHp);
        }
        for (int i = 0; i < 3; i++)
        {
            if (GetCurrentCharacter().currentAbilityCooldown[i] > 0)
            {
                abilityCooldowns[i].fillAmount = 1;
                abilityCooldownTexts[i].text = GetCurrentCharacter().currentAbilityCooldown[i].ToString();
            }
            else { abilityCooldownTexts[i].text = ""; abilityCooldowns[i].fillAmount = 0; }
        }
    }

    private void Attack()
    {
        if (canUse)
        {
            canUse = false;
            weaponAnimator.speed = 1 / CalculateAttackSpeed();
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

    public float CalculateAttackSpeed()
    {
        return Mathf.Clamp(GetCurrentCharacter().attackSpeed - GetCurrentCharacter().attackSpeed * GetCurrentCharacter().equipmentManager.GetTotalAttackSpeed() / 100, 0.1f, 999);
    }
    public int CalculateDamage()
    {
        return (int)Mathf.Clamp(GetCurrentCharacter().damage + GetCurrentCharacter().equipmentManager.GetTotalDamage(), 0, int.MaxValue);
    }

    public void EndAction()
    {
        canUse = true;
        playerMovement.CanMove = true;
        weaponAnimator.speed = 1;
    }
    public void Die()
    {
        
    }
}
