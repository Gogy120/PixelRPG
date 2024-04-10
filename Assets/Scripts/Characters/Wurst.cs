using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wurst : ICharacter
{
    public Wurst(Player player, EquipmentManager equipmentManager) : base(player, equipmentManager)
    {
        base.name = "Wurst";
        base.description = "Prd";
        base.baseMaxHp = 135;
        base.basePhysicalDamage = 15;
        base.baseMagicDamage = 0;
        base.baseAttackSpeed = 0.35f;
        base.baseAbilityCooldown = new int[3] { 5, 12, 15 };
        base.abilityPhysicalScaling = new float[3] { 1.25f, 0.5f, 0.9f };
        base.abilityMagicScaling = new float[3] { 0f, 0.25f, 0.45f };
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
        player.bodyAnimator.SetTrigger("ability3");
    }
    public void ThrowBeer()
    {
        player.actionManager.ShootAbilityProjectile("Beer", 1, player.projectileSpawnPoint.position);
    }

    public void Burp()
    {
        player.playerMovement.CanMove = false;
        int mult = player.playerMovement.GetFacingDirectionMultiplier();
        Vector2 position = new Vector2(player.transform.position.x + (0.125f * mult), player.transform.position.y + 0.5f);
        player.actionManager.ShootAbilityProjectile("Burp",2,position);
    }


    public void Fart()
    {
        player.playerMovement.CanMove = false;
        int mult = player.playerMovement.GetFacingDirectionMultiplier();
        float rotY = 0;
        if (player.bodySpriteRenderer.flipX) { rotY = 180; }
        Vector3 rotation = new Vector3(0,rotY,0);
        Vector2 pos = new Vector2(player.transform.position.x + (0.5f * mult), player.transform.position.y + 0.25f);
        player.actionManager.ShootAbilityProjectile("Fart", 3, pos, rotation);
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
