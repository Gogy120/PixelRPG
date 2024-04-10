using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermann : ICharacter
{
    public Hermann(Player player, EquipmentManager equipmentManager) : base(player, equipmentManager)
    {
        base.name = "Hermann";
        base.description = "Prd";
        base.baseMaxHp = 125;
        base.basePhysicalDamage = 0;
        base.baseMagicDamage = 9;
        base.baseAttackSpeed = 0.4f;
        base.baseAbilityCooldown = new int[3] { 5, 12, 15 };
        base.abilityPhysicalScaling = new float[3] { 1.25f, 0.5f, 0.9f };
        base.abilityMagicScaling = new float[3] { 0f, 0.25f, 0.45f };
        base.damageType = DamageType.MAGIC;
        base.player = player;
        base.equipmentManager = equipmentManager;
    }

    public override void StartAbility1()
    {
        player.weaponAnimator.SetTrigger("ability1");
    }
    public override void StartAbility2()
    {
        player.bodyAnimator.SetTrigger("ability2");
    }
    public override void StartAbility3()
    {
        player.bodyAnimator.SetTrigger("ability3");
    }
  

    public override string GetAbilityTooltip(int ability)
    {
        string text = "";
        switch (ability)
        {
            case 1:
                text = $"<u>Beer toss</u>\nThrows a beer glass forward, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[0] * 100}%</color> physical AOE damage";
                break;
            case 2:
                text = $"<u>Burp</u>\nErupts a giant burp, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[1] * 100}%</color> physical and <color={Toolkit.magicDamageColor}>{abilityMagicScaling[1] * 100}%</color> AOE damage and stuns enemies";
                break;
            case 3:
                text = $"<u>Fart</u>\nFarts in the opposite direction and applies a DOT that deals <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[2] * 100}%</color> physical and <color={Toolkit.magicDamageColor}>{abilityMagicScaling[2] * 100}%</color> magic damage per tick";
                break;
        }
        return text;
    }
}
