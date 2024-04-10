using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public readonly string name;
    public readonly string description;
    public readonly int baseMaxHp;
    public readonly int damage;
    public float attackSpeed;
    public readonly int[] abilityCooldown = new int[3];
    public readonly int[] abilityDamage = new int[3];
    public int hp;
    public int[] currentAbilityCooldown = new int[3];
    public int maxHp;
    public bool hasSwitched = false;
    public bool isDead = false;
    public EquipmentManager equipmentManager;

    public Character(string name, string description, int baseMaxHp, int damage, float attackSpeed, int[] abilityCooldown, int[] abilityDamage)
    {
        this.name = name;
        this.description = description;
        this.baseMaxHp = baseMaxHp;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.abilityCooldown = abilityCooldown;
        this.abilityDamage = abilityDamage;
        this.maxHp = baseMaxHp;
        this.hp = maxHp;
        equipmentManager = new EquipmentManager(name);
    }

    public RuntimeAnimatorController GetBodyAnimatorController()
    {
        return Resources.Load<RuntimeAnimatorController>("Animations/Characters/" + name + "/" + name);
    }

    public RuntimeAnimatorController GetWeaponAnimatorController()
    {
        return Resources.Load<RuntimeAnimatorController>("Animations/Weapon/" + name + "/" + "Weapon_" + name);
    }

    public Sprite GetIconSprite()
    {
        return Resources.Load<Sprite>("Sprites/UI/CharacterIcons/Icon_" + name);
    }

    public Sprite GetWeaponSprite()
    {
        return Resources.Load<Sprite>("Sprites/Characters/Weapons/Weapon_" + name);
    }

    public Sprite GetAbilityIcon(int ability)
    {
        return Resources.Load<Sprite>("Sprites/UI/AbilityIcons/" + name + "_Ability" + ability);
    }

    public override string ToString()
    {
        return name + "|" + description + "|" + baseMaxHp + "|" + damage;
    }

    public IEnumerator StartAbilityCooldown(int ability)
    {
        WaitForSeconds secondDelay = new WaitForSeconds(1);
        if (currentAbilityCooldown[ability] == 0)
        {
            currentAbilityCooldown[ability] = abilityCooldown[ability];
            for (int i = 0; i < abilityCooldown[ability]; i++)
            {
                yield return secondDelay;
                currentAbilityCooldown[ability]--;
                Debug.Log(name + " Ability " + (ability + 1) + " cooldown: " +  currentAbilityCooldown[ability]);
            }
        }
        currentAbilityCooldown[ability] = 0;
    }
}
