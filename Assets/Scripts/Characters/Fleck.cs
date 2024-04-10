using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleck : ICharacter
{
    public Fleck(Player player, EquipmentManager equipmentManager) : base(player, equipmentManager)
    {
        base.name = "Fleck";
        base.description = "Prd";
        base.baseMaxHp = 115;
        base.basePhysicalDamage = 13;
        base.baseMagicDamage = 0;
        base.baseAttackSpeed = 0.4f;
        base.baseAbilityCooldown = new int[3] { 6, 15, 20 };
        base.abilityPhysicalScaling = new float[3] { 1.25f, 0.25f, 1.35f };
        base.abilityMagicScaling = new float[3] { 0f, 1.25f, 0f };
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
        player.weaponAnimator.SetTrigger("ability2");
    }
    public override void StartAbility3()
    {
        player.weaponAnimator.SetTrigger("ability3");
    }
    public void ShootRocketLauncher()
    {
        player.actionManager.ShootAbilityProjectile("Rocket",1);
    }

    public void SpillOil()
    {
        GameObject projectile = player.actionManager.ShootAbilityProjectile("Oil", 2);
        projectile.transform.parent = player.weaponAnimator.transform;
        projectile.transform.localPosition = new Vector2(1f,0f);

    }


    public IEnumerator ShootMinigun()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        int shots = 30;
        for (int i = 0; i < shots; i++)
        {
            player.actionManager.ShootAbilityProjectile("Bean",3);
            yield return delay;
        }
    }

    public override void Attack()
    {
        player.actionManager.ShootProjectile("Bean");
    }

    public override string GetAbilityTooltip(int ability)
    {
        string text = "";
        switch (ability)
        {
            case 1:
                text = $"<u>Rocket launcher</u>\nShoots a rocket, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[0] * 100}%</color> physical AOE damage";
                break;
            case 2:
                text = $"<u>Oil spill</u>\nSprays enemies with oil, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[1] * 100}%</color> physical and <color={Toolkit.magicDamageColor}>{abilityMagicScaling[1] * 100}%</color> magic AOE damage and slows enemies";
                break;
            case 3:
                text = $"<u>Minigun</u>\nShoots a volley of beans, dealing <color={Toolkit.physicalDamageColor}>{abilityPhysicalScaling[2] * 100}%</color> physical damage per bean";
                break;
        }
        return text;
    }
}
