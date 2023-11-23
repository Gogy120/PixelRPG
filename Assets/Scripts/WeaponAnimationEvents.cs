using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEvents : MonoBehaviour
{
    private CharacterManager characterManager;
    private Player player;
    private void Start()
    {
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void End()
    {
        characterManager.EndAction();
    }

    public void Hit()
    {
        characterManager.Hit();
    }

    public void ShakeCamera(float magnitude)
    {
        player.ShakeCamera(magnitude);
    }

    public void ChangeWeaponSprite(string name)
    {
        characterManager.weaponSpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Characters/Weapons/Weapon_" + name);
    }

    public void ChangeWeaponSpriteToDefault()
    {
        characterManager.weaponSpriteRenderer.sprite = characterManager.GetCurrentCharacter().GetWeaponSprite();
    }
}
