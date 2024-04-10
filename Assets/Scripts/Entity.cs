using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
 
    [Header("Variables - ENTITY")]
    public int baseMaxHp;
    public string hurtSound;
    [Header("Components - ENTITY")]
    public SpriteRenderer spriteRenderer;
    public Material flashMat;
    public Material defaultMat;
    public SpriteRenderer shadowSpriteRenderer;
    public Transform? particleSpawnPoint;

    [HideInInspector] public Player player;
    [HideInInspector] public int hp;
    private bool canFlash = true;
    private WaitForSeconds flashDelay = new WaitForSeconds(0.1f);
    [HideInInspector] public bool isSpawned = false;
    [HideInInspector] public Color32 hurtDamageColor = new Color32(255, 255, 255, 255);
    private bool isDotActive = false;

    private const float scaleConstant = 1.13f;

    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp < 0) { hp = 0; }
        }
    }
    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        baseMaxHp = GetMaxHp();
        Hp = baseMaxHp;
        StartCoroutine(Spawn());
    }

    private int GetMaxHp()
    {
        int level = PlayerPrefs.GetInt("level", 1);
        int scaledHp = Mathf.RoundToInt(baseMaxHp * Mathf.Pow(scaleConstant,level-1));
        return scaledHp;
    }

    public virtual void TakeDamage(GameObject? source,int damage)
    {
        if (isSpawned)
        {
            if (hurtSound != "") { Toolkit.PlaySound(hurtSound, 0.35f, 0.8f, 1f); }
            StartCoroutine(Flash(flashMat));
            if (damage > 0)
            {
                Hp -= damage;
                Toolkit.SpawnText(damage.ToString(), this.transform.position, hurtDamageColor);
            }
            if (Hp <= 0)
            {
                Die();
            }
        }
    }


    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator Flash(Material material)
    {
        if (canFlash)
        {
            canFlash = false;
            spriteRenderer.material = material;
            yield return flashDelay;
            canFlash = true;
            spriteRenderer.material = defaultMat;
        }
    }


    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (isSpawned)
        {
            if (col.gameObject.tag == "WeaponHitbox")
            {
                TakeDamage(col.gameObject,player.characterManager.GetCurrentCharacter().GetAttackDamage());
            }
            if (col.gameObject.tag == "PlayerProjectile")
            {
                Projectile projectile = col.GetComponent<Projectile>();
                if (projectile.sound != "")
                {
                    Toolkit.PlaySound(projectile.sound);
                }
                switch (projectile.type)
                {
                    case Projectile.ProjectileType.NORMAL:
                        TakeDamage(col.gameObject,col.GetComponent<Projectile>().damage);
                        break;
                    case Projectile.ProjectileType.EXPLOSIVE:
                        projectile.Explode(col.transform.position, projectile.radius, projectile.damage);
                        break;
                    case Projectile.ProjectileType.DOT:
                        StartCoroutine(ApplyDot(projectile.damage, projectile.ticks, projectile.tickSpeed));
                        break;
                }
                if (projectile.destroyOnHit) { Destroy(col.gameObject); }
            }
        }
    }

    public IEnumerator ApplyDot(int tickDamage, int ticks, float tickSpeed)
    {
        if (!isDotActive && isSpawned)
        {
            isDotActive = true;
            WaitForSeconds tickDelay = new WaitForSeconds(tickSpeed);
            for (int i = 0; i < ticks; i++)
            {
                TakeDamage(null, tickDamage);
                yield return tickDelay;
            }
            isDotActive = false;
        }
    }

    public virtual IEnumerator Spawn()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                shadowSpriteRenderer.enabled = !shadowSpriteRenderer.enabled;
                yield return delay;
            }
        }
        isSpawned = true;
    }


}
