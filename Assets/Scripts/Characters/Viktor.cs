using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viktor : ICharacter
{
    public Viktor(Player player, EquipmentManager equipmentManager) : base(player, equipmentManager)
    {
        base.name = "Viktor";
        base.description = "Prd";
        base.baseMaxHp = 110;
        base.basePhysicalDamage = 12;
        base.baseMagicDamage = 0;
        base.baseAttackSpeed = 0.5f;
        base.baseAbilityCooldown = new int[3] { 5, 15, 5 };
        base.abilityPhysicalScaling = new float[3] { 0.75f, 0f, 0.15f };
        base.abilityMagicScaling = new float[3] { 0f, 0f, 0.35f };
        base.damageType = DamageType.PHYSICAL;
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
        player.weaponAnimator.SetTrigger("ability3");
    }
    public void Taser()
    {
        GameObject projectile = player.actionManager.ShootAbilityProjectile("Taser",1);
        projectile.transform.parent = player.projectileSpawnPoint.transform;
        projectile.transform.localPosition = Vector2.zero;
    }

    public void StartSegway()
    {
        player.playerMovement.speed *= 3;
    }

    public void StopSegway()
    {
        player.playerMovement.speed /= 3;
    }


    public override void Attack()
    {
        player.actionManager.ShootProjectile("Bullet");
    }

    public override string GetAbilityTooltip(int ability)
    {
        string text = "";
        switch (ability)
        {
            case 1:
                text = $"<u>Taser</u>\nPulls out a taser, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[0] * 100}%</color> physical AOE damage";
                break;
            case 2:
                text = $"<u>Segway</u>\nHops on a segway, increasing movement speed";
                break;
            case 3:
                text = $"<u>Minigun</u>\nShoots a volley of beans, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[2] * 100}%</color> physical damage per bean";
                break;
        }
        return text;
    }
}
