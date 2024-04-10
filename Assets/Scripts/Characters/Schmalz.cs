using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schmalz : ICharacter
{
    public Schmalz(Player player, EquipmentManager equipmentManager) : base(player, equipmentManager)
    {
        base.name = "Schmalz";
        base.description = "Prd";
        base.baseMaxHp = 200;
        base.basePhysicalDamage = 15;
        base.baseMagicDamage = 0;
        base.baseAttackSpeed = 0.65f;
        base.baseAbilityCooldown = new int[3] { 5, 5, 5 };
        base.abilityPhysicalScaling = new float[3] { 0.75f, 0f, 0.15f };
        base.abilityMagicScaling = new float[3] { 0f, 0f, 0.35f };
        base.damageType = DamageType.PHYSICAL;
        base.player = player;
        base.equipmentManager = equipmentManager;

    }
    public override void StartAbility1()
    {
        
    }
    public override void StartAbility2()
    {
        
    }
    public override void StartAbility3()
    {
        
    }
}
