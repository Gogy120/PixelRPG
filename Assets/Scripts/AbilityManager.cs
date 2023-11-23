using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Player player;
    private CharacterManager characterManager;
    private float savedSpeed;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }
    #region Functions
    public void EndAction()
    {
        characterManager.EndAction();
    }
    public void Explode(Vector2 pos, float radius, int damage)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pos, radius);
        foreach (Collider2D c in col)
        {
            if (c.GetComponent<Entity>() != null)
            {
                c.GetComponent<Entity>().TakeDamage(damage);
            }
        }
    }

    private void ShootAbilityProjectile(string name, int ability)
    {
        GameObject projectileObj = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name);
        GameObject projectile = Instantiate(projectileObj, player.projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.eulerAngles = new Vector3(0, 0, player.weaponHolder.rotationZ);
        projectile.GetComponent<Projectile>().damage = characterManager.GetCurrentCharacter().abilityDamage[ability - 1];
        Destroy(projectile, 1.5f);
    }

    private void ShootProjectile(string name)
    {
        GameObject projectileObj = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name);
        GameObject projectile = Instantiate(projectileObj, player.projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.eulerAngles = new Vector3(0, 0, player.weaponHolder.rotationZ);
        projectile.GetComponent<Projectile>().damage = characterManager.CalculateDamage();
        Destroy(projectile, 1.5f);
    }

    public void PlaySound(string name)
    {
        Toolkit.PlaySound(name);
    }
    #endregion
    #region Wurst
    public void Wurst_Ability1()
    {
        characterManager.weaponAnimator.SetTrigger("ability1");
    }
    public void Wurst_Ability2()
    {
        player.playerMovement.bodyAnimator.SetTrigger("ability2");
    }
    public void Wurst_Ability3()
    {
        player.playerMovement.bodyAnimator.SetTrigger("ability3");
    }

    public void Wurst_ThrowBeer()
    {
        ShootAbilityProjectile("Beer",1);
    }
    public void Wurst_Fart()
    {
        player.playerMovement.CanMove = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject fartObj = Resources.Load<GameObject>("Prefabs/Fart");
        int mult = -1;
        if (player.bodySpriteRenderer.flipX) { mult = 1; }
        Vector2 pos = new Vector2(player.transform.position.x + 0.5f * mult, player.transform.position.y + 0.25f);
        float rotY = 0;
        if (player.bodySpriteRenderer.flipX) { rotY = 180; }
        GameObject fart = Instantiate(fartObj, pos, Quaternion.identity);
        fart.GetComponent<Projectile>().tickDamage = characterManager.GetCurrentCharacter().abilityDamage[2];
        fart.transform.eulerAngles = new Vector3(0, rotY, 0);
    }

    public void Wurst_Burp()
    {
        player.playerMovement.CanMove = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject burpObj = Resources.Load<GameObject>("Prefabs/Burp");
        GameObject burp = Instantiate(burpObj, new Vector2(player.transform.position.x, player.transform.position.y + 0.5f), Quaternion.identity);
        burp.GetComponentInChildren<Projectile>().damage = characterManager.GetCurrentCharacter().abilityDamage[0];
        Destroy(burp, 1f);
    }

    #endregion
    #region Fleck
    public void Fleck_Ability1()
    {
        characterManager.weaponAnimator.SetTrigger("ability1");
    }
    public void Fleck_Ability2()
    {
        characterManager.weaponAnimator.SetTrigger("ability2");
    }
    public void Fleck_Ability3()
    {
        characterManager.weaponAnimator.SetTrigger("ability3");
    }
    public IEnumerator Fleck_Shootminigun()
    {
        int count = 20;
        float seconds = 2;
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        for (int i = 0; i < count; i++)
        {
            ShootAbilityProjectile("MinigunBean",3);
            yield return delay;
        }
    }

    public void Fleck_ShootOil()
    {
        GameObject oilProjectile = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Oil");
        GameObject oilObj = Instantiate(oilProjectile, characterManager.weaponAnimator.gameObject.transform.position,Quaternion.identity);
        oilObj.transform.parent = characterManager.weaponAnimator.transform;
        oilObj.transform.localPosition = new Vector2(1.35f,0f);
        oilObj.transform.localEulerAngles = Vector3.zero;
    }

    public void Fleck_ShootRocket()
    {
        ShootAbilityProjectile("Rocket", 1);
    }
    #endregion
    #region Viktor
    public void Viktor_Ability1()
    {
        characterManager.weaponAnimator.SetTrigger("ability1");
    }
    public void Viktor_Ability2()
    {
        player.playerMovement.bodyAnimator.SetTrigger("ability2");
    }
    public void Viktor_Ability3()
    {
        player.playerMovement.bodyAnimator.SetTrigger("ability3");
    }

    public void Viktor_SpeedUp()
    {
        savedSpeed = player.playerMovement.speed;
        player.playerMovement.speed *= 3;
    }
    public void Viktor_SlowDown()
    {
        player.playerMovement.speed = savedSpeed;
    }

    public void Viktor_UseTaser()
    {
        GameObject taserProjectile = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_Taser");
        GameObject taserObj = Instantiate(taserProjectile, characterManager.weaponAnimator.gameObject.transform.position, Quaternion.identity);
        taserObj.transform.parent = characterManager.weaponAnimator.transform;
        taserObj.transform.localPosition = new Vector2(1.2f, 0f);
        taserObj.transform.localEulerAngles = Vector3.zero;
        taserObj.GetComponent<Projectile>().damage = characterManager.GetCurrentCharacter().abilityDamage[0];
    }
    #endregion
}
