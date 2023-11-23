using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    public int maxHp;
    public bool knockback;
    [Header("Variables")]
    public SpriteRenderer spriteRenderer;
    public Material flashMat;
    public Material defaultMat;
    public SpriteRenderer shadowSpriteRenderer;
    public Transform? particleSpawnPoint;

    private Player player;
    private CharacterManager characterManager;
    private int hp;
    private bool canFlash = true;
    private WaitForSeconds flashDelay = new WaitForSeconds(0.1f);
    private bool isSpawned = false;
    private Color32 hurtDamageColor = new Color32(255, 255, 255, 255);
    private bool isDotActive = false;
    private Enemy? enemy;
    private bool isStunned = false;
    private bool isSlowed = false;
    #endregion
    #region Getters & Setters
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
    public bool IsStunned
    {
        get { return isStunned; }
        set { isStunned = value; }
    }
    public bool IsSpawned
    {
        get { return isSpawned; }
        set { isSpawned = value; }
    }
    #endregion
    public void Start()
    {
        if (this.GetComponent<Enemy>() != null) { enemy = this.GetComponent<Enemy>(); }
        player = GameObject.Find("Player").GetComponent<Player>();
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
        hp = maxHp;
        StartCoroutine(Spawn());
    }

    public void TakeDamage(int damage)
    {
        if (isSpawned)
        {
            Vector2 direction = (this.transform.position - player.transform.position).normalized;
            Toolkit.PlaySound("Hurt1",0.35f,0.8f,1f);
            StartCoroutine(Flash(flashMat));
            if (damage > 0)
            {
                hp -= damage;
                Toolkit.SpawnText(damage.ToString(), this.transform.position, hurtDamageColor);
            }
            if (hp <= 0)
            {
                Die();
            }
            if (this is Enemy)
            {
                this.GetComponent<Enemy>().Engage();
            }
        }
    }

    public void TakeDamage(int damage, Material material)
    {
        if (isSpawned)
        {
            Toolkit.PlaySound("Hurt1", 0.35f, 0.8f, 1f);
            hp -= damage;
            Toolkit.SpawnText(damage.ToString(), this.transform.position, hurtDamageColor);
            StartCoroutine(Flash(material));
            if (hp <= 0)
            {
                Die();
            }
            if (this is Enemy)
            {
                enemy.Engage();
            }
        }
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator Flash(Material material)
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

    private IEnumerator Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            enemy.animator.speed = 0;
            enemy.navMeshAgent.speed = 0;
            yield return new WaitForSeconds(duration);
            isStunned = false;
            enemy.animator.speed = 1;
            enemy.navMeshAgent.speed = enemy.speed;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
       if (IsSpawned)
        {
            if (col.gameObject.tag == "WeaponHitbox")
            {
                TakeDamage(characterManager.CalculateDamage());
            }
            if (col.gameObject.tag == "PlayerProjectile")
            {
                Projectile projectile = col.GetComponent<Projectile>();
                switch (projectile.type)
                {
                    case Projectile.ProjectileType.NORMAL:
                        TakeDamage(col.GetComponent<Projectile>().damage);
                        break;
                    case Projectile.ProjectileType.EXPLOSIVE:
                        projectile.Explode(col.transform.position, projectile.radius, projectile.damage);
                        break;
                    case Projectile.ProjectileType.DOT:
                        StartCoroutine(ApplyDot(projectile.tickDamage, projectile.ticks, projectile.tickSpeed, projectile.dotMat));
                        break;
                    case Projectile.ProjectileType.STUN:
                        StartCoroutine(Stun(projectile.stunDuration));
                        TakeDamage(col.GetComponent<Projectile>().damage);
                        break;
                    case Projectile.ProjectileType.SLOW:
                        StartCoroutine(Slow(projectile.slowDuration,projectile.slowMult,projectile.slowMat));
                        TakeDamage(col.GetComponent<Projectile>().damage);
                        break;
                }
                if (projectile.destroyOnHit) { Destroy(col.gameObject); }
            }
        }
    }

    public IEnumerator ApplyDot(int tickDamage, int ticks, float tickSpeed, Material material)
    {
        if (!isDotActive && isSpawned)
        {
            isDotActive = true;
            WaitForSeconds tickDelay = new WaitForSeconds(tickSpeed);
            for (int i = 0; i < ticks; i++)
            {
                yield return tickDelay;
                TakeDamage(tickDamage, material);
            }
            isDotActive = false;
        }
    }

    private IEnumerator Spawn()
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

    private IEnumerator Slow(float duration,float mult,Material mat)
    {
        if (!isSlowed && this is Enemy)
        {
            isSlowed = true;
            Material savedDefaultMat = defaultMat;
            float savedSpeed = enemy.speed;
            enemy.speed = savedSpeed * mult;
            enemy.navMeshAgent.speed = enemy.speed;
            spriteRenderer.material = mat;
            defaultMat = mat;
            yield return new WaitForSeconds(duration);
            spriteRenderer.material = savedDefaultMat;
            defaultMat = savedDefaultMat;
            enemy.speed = savedSpeed;
            enemy.navMeshAgent.speed = savedSpeed;
            isSlowed = false;
        }
    }

}
