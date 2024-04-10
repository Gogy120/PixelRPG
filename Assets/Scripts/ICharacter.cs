using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICharacter : MonoBehaviour
{
    [HideInInspector] public Player player;
    [HideInInspector] public string name;
    [HideInInspector] public string description;
    [HideInInspector] public int baseMaxHp;
    [HideInInspector] public int basePhysicalDamage;
    [HideInInspector] public int baseMagicDamage;
    [HideInInspector] public float baseAttackSpeed;
    [HideInInspector] public int[] baseAbilityCooldown = new int[3];
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public float[] abilityPhysicalScaling = new float[3];
    [HideInInspector] public float[] abilityMagicScaling = new float[3];

    [HideInInspector] public int hp;
    [HideInInspector] public int[] currentAbilityCooldown = new int[3];
    [HideInInspector] public int maxHp;
    [HideInInspector] public bool hasSwitched;
    [HideInInspector] public bool isDead;
    [HideInInspector] public EquipmentManager equipmentManager;

    public enum DamageType
    {
        PHYSICAL, MAGIC
    }

    public virtual void StartAbility1() { } //Start bodyAnimator or weaponAnimator
    public virtual void StartAbility2() { } //Start bodyAnimator or weaponAnimator
    public virtual void StartAbility3() { } //Start bodyAnimator or weaponAnimator
    public virtual void Attack() { }

    public ICharacter(Player player, EquipmentManager equipmentManager) { }
    public virtual void Start() { } //Init stats and variables
    public IEnumerator StartAbilityCooldown(int ability)
    {
        WaitForSeconds secondDelay = new WaitForSeconds(1);
        if (currentAbilityCooldown[ability] == 0)
        {
            currentAbilityCooldown[ability] = baseAbilityCooldown[ability];
            for (int i = 0; i < baseAbilityCooldown[ability]; i++)
            {
                yield return secondDelay;
                currentAbilityCooldown[ability]--;
            }
        }
        currentAbilityCooldown[ability] = 0;
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

    public float GetAttackSpeed()
    {
        return Mathf.Clamp(baseAttackSpeed - baseAttackSpeed * equipmentManager.GetTotalAttackSpeed() / 100, 0.1f, 999f);
    }
    public int GetAttackDamage()
    {
        return GetDamageByType(damageType);
    }
    public int GetDamageByType(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.PHYSICAL: return (int)Mathf.Clamp(basePhysicalDamage + equipmentManager.GetTotalPhysicalDamage(), 0, int.MaxValue);
            case DamageType.MAGIC: return (int)Mathf.Clamp(baseMagicDamage + equipmentManager.GetTotalMagicDamage(), 0, int.MaxValue);
            default: return 0;
        }
    }

    public float GetAbilityDamageScaling(int ability, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.PHYSICAL: return abilityPhysicalScaling[ability - 1];
            case DamageType.MAGIC: return abilityPhysicalScaling[ability - 1];
            default: return 0;
        }
    }

    public int GetAbilityDamage(int ability)
    {
        return Mathf.RoundToInt(GetAbilityDamageScaling(ability, DamageType.PHYSICAL) * GetDamageByType(DamageType.PHYSICAL) + GetAbilityDamageScaling(ability, DamageType.MAGIC) * GetDamageByType(DamageType.MAGIC));
    }

    public override string ToString()
    {
        return name + "|" + description + "|" + baseMaxHp + "|";
    }

    public virtual string GetAbilityTooltip(int ability)
    {
        return null;
    }

   
}
