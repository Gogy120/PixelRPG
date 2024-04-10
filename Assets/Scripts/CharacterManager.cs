using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CharacterManager : MonoBehaviour
{
    public int currentCharacterIndex = 0;
    public Player player;
    [HideInInspector] public ICharacter[] characters = new ICharacter[3];

    void Start()
    {
    
    }

    public ICharacter GetCurrentCharacter()
    {
        return characters[currentCharacterIndex];
    }

    public void SwapCharacter(int index)
    {
        if (player.playerState == Player.PlayerState.IDLE && !characters[index].isDead)
        {
            currentCharacterIndex = index;
            ICharacter currentCharacter = GetCurrentCharacter();
            Toolkit.SpawnParticle("Swap", new Vector2(this.transform.position.x, this.transform.position.y + 0.5f));
            player.bodyAnimator.runtimeAnimatorController = currentCharacter.GetBodyAnimatorController();
            player.weaponAnimator.runtimeAnimatorController = currentCharacter.GetWeaponAnimatorController();
            player.weaponSpriteRenderer.sprite = currentCharacter.GetWeaponSprite();
            player.characterSwapText.text = currentCharacter.name;
            player.characterSwapText.GetComponent<Animator>().SetTrigger("end");
            //Set health bars
            for (int i = 0; i < 3; i++)
            {
                if (currentCharacterIndex == i)
                {
                    player.characterHealthBars[i].transform.parent.gameObject.SetActive(false);
                }
                else { player.characterHealthBars[i].transform.parent.gameObject.SetActive(true); }
            }
            //Set ability sprites
            for (int i = 0; i < 3; i++)
            {
                player.abilityIcons[i].sprite = currentCharacter.GetAbilityIcon(i + 1);
            }
            currentCharacter.maxHp = currentCharacter.baseMaxHp + currentCharacter.equipmentManager.GetTotalHP();
            if (!currentCharacter.hasSwitched)
            {
                currentCharacter.hp = currentCharacter.maxHp;
                currentCharacter.hasSwitched = true;
            }
        }
    }

    public void CastAbility(int ability)
    {
        ICharacter currentCharacter = GetCurrentCharacter();
        if (player.playerState == Player.PlayerState.IDLE && currentCharacter.currentAbilityCooldown[ability - 1] == 0)
        {
            player.playerState = Player.PlayerState.ATTACKING;
            StartCoroutine(currentCharacter.StartAbilityCooldown(ability - 1));
            MethodInfo methodInfo = currentCharacter.GetType().GetMethod("StartAbility" + ability);
            methodInfo.Invoke(currentCharacter, null);
        }
    }
}
