using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ActionManager : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void EndAction()
    {
        player.EndAction();
    }

    public void Hit()
    {
        player.Hit();
    }

    public void ShakeCamera(float magnitude)
    {
        player.ShakeCamera(magnitude);
    }

    public void ChangeWeaponSprite(string name)
    {
        player.weaponSpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Characters/Weapons/Weapon_" + name);
    }

    public void ChangeWeaponSpriteToDefault()
    {
        player.weaponSpriteRenderer.sprite = player.characterManager.GetCurrentCharacter().GetWeaponSprite();
    }
    public void Explode(Vector2 pos, float radius, int damage)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pos, radius);
        foreach (Collider2D c in col)
        {
            if (c.GetComponent<Entity>() != null)
            {
                c.GetComponent<Entity>().TakeDamage(null,damage);
            }
        }
    }
    public GameObject ShootAbilityProjectile(string name, int ability) //Shoot ability projectile from the projectile spawn point
    {
        GameObject projectileObj = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name);
        GameObject projectile = Instantiate(projectileObj, player.projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.eulerAngles = new Vector3(0, 0, player.weaponHolder.rotationZ);
        projectile.GetComponentInChildren<Projectile>().damage = player.characterManager.GetCurrentCharacter().GetAbilityDamage(ability);
        Destroy(projectile, 1.5f);
        return projectile;
    }
    public GameObject ShootAbilityProjectile(string name, int ability, Vector2 position) //Shoot ability projectile from a position
    {
        GameObject projectile = ShootAbilityProjectile(name,ability);
        projectile.transform.position = position;
        return projectile;
    }

    public GameObject ShootAbilityProjectile(string name, int ability, Vector2 position, Vector3 rotation) //Shoot ability projectile from a position and with a rotation
    {
        GameObject projectile = ShootAbilityProjectile(name, ability, position);
        projectile.transform.eulerAngles = rotation;
        return projectile;
    }

    public GameObject ShootProjectile(string name) //Shoot projectile from the projectile spawn point
    {
        GameObject projectileObj = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name);
        GameObject projectile = Instantiate(projectileObj, player.projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.eulerAngles = new Vector3(0, 0, player.weaponHolder.rotationZ);
        projectile.GetComponentInChildren<Projectile>().damage = player.characterManager.GetCurrentCharacter().GetAttackDamage();
        Destroy(projectile, 1.5f);
        return projectile;
    }
    public GameObject ShootProjectile(string name, Vector2 position) //Shoot projectile from a position
    {
        GameObject projectile = ShootProjectile(name);
        projectile.transform.position = position;
        return projectile;
    }

    public GameObject ShootProjectile(string name, Vector2 position, Vector3 rotation) //Shoot ability projectile from a position and with a rotation
    {
        GameObject projectile = ShootProjectile(name, position);
        projectile.transform.eulerAngles = rotation;
        return projectile;
    }



    public void ExecuteAbility(string methodName)
    {
        ICharacter currentCharacter = player.characterManager.GetCurrentCharacter();
        System.Reflection.MethodInfo method = currentCharacter.GetType().GetMethod(methodName);

        if (method != null)
        {
            if (typeof(IEnumerator).IsAssignableFrom(method.ReturnType))
            {
                StartCoroutine((IEnumerator)method.Invoke(currentCharacter, null));
            }
            else
            {
                method.Invoke(currentCharacter, null);
            }
        }
        else
        {
            Debug.LogError("Method not found: " + methodName);
        }

    }

    public void ExecuteAttack()
    {
        MethodInfo methodInfo = player.characterManager.GetCurrentCharacter().GetType().GetMethod("Attack");
        ExecuteMethod(methodInfo);
    }

    private void ExecuteMethod(MethodInfo method)
    {
        ICharacter currentCharacter = player.characterManager.GetCurrentCharacter();
        if (typeof(IEnumerator).IsAssignableFrom(method.ReturnType))
        {
            StartCoroutine((IEnumerator)method.Invoke(currentCharacter, null));
        }
        else
        {
            method.Invoke(currentCharacter, null);
        }
    }


    public void PlaySound(string name)
    {
        Toolkit.PlaySound(name,1,0.8f,1f);
    }
}
