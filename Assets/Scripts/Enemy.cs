using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Enemy : Entity
{
    [Header("Variables - ENEMY")]
    public string name;
    public int damage;
    public float attackSpeed;
    public float speed;
    public float hitRange;
    public float aggroRange;
    public float thresholdDistanceToPlayer = 0.5f;
    [Header("Components - ENEMY")]
    public Rigidbody2D rigidbody2D;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public LootDropManager lootDropManager;
    public Animator engageAnimator;

    [HideInInspector] public bool isEngaged = false;
    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public bool canUse = true;
    private bool canMove = false;
    private bool isSlowed = false;
    [HideInInspector] public bool isStunned = false;

    private Color32 xpTextColor = new Color32(249,194,43,255);

    public bool CanMove
    {
        get { return canMove; }
        set
        {
            canMove = value;
            if (value == true)
            {
                navMeshAgent.speed = speed;
            }
            else
            {
                navMeshAgent.speed = 0;
            }
        }
    }

    public void Start()
    {
        base.Start();
        animator.enabled = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    public void Update()
    {
        distanceToPlayer = Vector2.Distance(new Vector2(this.transform.position.x,this.transform.position.y + 0.5f), new Vector2(player.transform.position.x, player.transform.position.y + 0.5f));
        if (!isStunned)
        {
            if (!isEngaged)
            {
                if (distanceToPlayer <= aggroRange && isSpawned)
                {
                    Engage();
                }
            }
            else
            {
                Behaviour();
                //Turn to player
                if (player.transform.position.x < this.transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
            //Running animation
            if (navMeshAgent.speed != 0)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false);
            }
        }
    }

    public void Engage()
    {
        if (!isEngaged)
        {
            engageAnimator.SetTrigger("engage");
            isEngaged = true;
            canUse = true;
            CanMove = true;
        }
    }

    public virtual void Behaviour() { }


    public void Attack()
    {
        if (canUse)
        {
            canUse = false;
            CanMove = false;
            animator.speed = 1 / attackSpeed;
            animator.SetTrigger("attack");
        }
    }


    private void OnDrawGizmos()
    {
        Vector2 offsetPos = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(offsetPos, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(offsetPos, hitRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(offsetPos, thresholdDistanceToPlayer);
    }


    public override void Die()
    {
        base.Die();
        lootDropManager.DropLoot(this.transform.position);
        if (GameObject.Find("Brawl") != null)
        {
            Brawl brawl = GameObject.Find("Brawl").GetComponent<Brawl>();
            brawl.IncreaseKills();
        }
        Toolkit.SpawnText("+" + player.levelManager.GetXpAmount() + " XP",this.transform.position,xpTextColor);
        player.levelManager.AddXp();
    }

    public virtual void EndAction()
    {
        animator.speed = 1;
        if (!isStunned)
        {
            CanMove = true;
        }
    }

    private IEnumerator Slow(float duration, float mult, Material mat)
    {
        if (!isSlowed && this is Enemy)
        {
            isSlowed = true;
            Material savedDefaultMat = defaultMat;
            float savedSpeed = speed;
            speed = savedSpeed * mult;
            navMeshAgent.speed = speed;
            spriteRenderer.material = mat;
            defaultMat = mat;
            yield return new WaitForSeconds(duration);
            spriteRenderer.material = savedDefaultMat;
            defaultMat = savedDefaultMat;
            speed = savedSpeed;
            navMeshAgent.speed = savedSpeed;
            isSlowed = false;
        }
    }

    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (isSpawned)
        {
            if (col.gameObject.tag == "PlayerProjectile")
            {
                Projectile projectile = col.GetComponent<Projectile>();
                switch (projectile.type)
                {
                    case Projectile.ProjectileType.STUN:
                        StartCoroutine(Stun(projectile.stunDuration));
                        TakeDamage(col.gameObject, col.GetComponent<Projectile>().damage);
                        break;
                    case Projectile.ProjectileType.SLOW:
                        StartCoroutine(Slow(projectile.slowDuration, projectile.slowMult, projectile.slowMat));
                        TakeDamage(col.gameObject, col.GetComponent<Projectile>().damage);
                        break;
                }
            }
        }
    }
    public override void TakeDamage(GameObject? source, int damage)
    {
        base.TakeDamage(source, damage);
        if (isSpawned)
        {
            player.combo.AddCombo();
            Engage();
        }
    }
    public IEnumerator Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            animator.speed = 0;
            navMeshAgent.speed = 0;
            yield return new WaitForSeconds(duration);
            isStunned = false;
            animator.speed = 1;
            navMeshAgent.speed = speed;
        }
    }

    public override IEnumerator Spawn()
    {
        StartCoroutine(base.Spawn());
        StartCoroutine(RandomAnimationDelay());
        yield return null;
    }

    private IEnumerator RandomAnimationDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        animator.enabled = true;
    }
}