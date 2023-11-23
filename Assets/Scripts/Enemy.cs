using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [Header("Stats")]
    public string name;
    public int damage;
    public float attackSpeed;
    public float speed;
    public float hitRange;
    public float aggroRange;
    public float thresholdDistanceToPlayer = 0.5f;
    public EnemyBehaviour behaviour;
    [Header("Stats-RANGED")]
    public float useCooldown;
    [Header("Variables")]
    public Rigidbody2D rigidbody2D;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform projectileSpawnPoint;
    public LootDropManager lootDropManager;

    private bool isEngaged = false;
    private float distanceToPlayer;
    private delegate void doBehaviour();
    private doBehaviour? DoBehaviour;
    private WaitForSeconds attackDelay;
    private bool canUse = true;
    private bool canMove = false;

    public enum EnemyBehaviour
    {
        Melee, Ranged
    }

    void Start()
    {
        base.Start();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        attackDelay = new WaitForSeconds(attackSpeed);
        switch (behaviour)
        {
            case EnemyBehaviour.Melee:
                DoBehaviour = MeleeBehaviour;
                break;
            case EnemyBehaviour.Ranged:
                DoBehaviour = RangedBehaviour;
                break;
        }
    }
    void Update()
    {
        distanceToPlayer = Vector2.Distance(this.transform.position, Player.transform.position);
        if (!IsStunned)
        {
            if (!isEngaged)
            {
                if (distanceToPlayer <= aggroRange && IsSpawned)
                {
                    Engage();
                }
            }
            else
            {
                DoBehaviour();
                if (Player.transform.position.x < this.transform.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
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
            isEngaged = true;
            canUse = true;
            canMove = true;
            animator.SetBool("running", true);
        }
    }

    private void MeleeBehaviour()
    {
        navMeshAgent.destination = Player.transform.position;
        if (distanceToPlayer > thresholdDistanceToPlayer && canMove)
        {
            navMeshAgent.speed = speed;
        }
        else
        {
            navMeshAgent.speed = 0;
            if (distanceToPlayer <= hitRange && canUse)
            {
                Attack();
            }
        }
    }
    private void RangedBehaviour()
    {
        navMeshAgent.destination = Player.transform.position;
        if (distanceToPlayer <= hitRange && canUse)
        {
            Attack();
        }
        if (distanceToPlayer > thresholdDistanceToPlayer && canMove)
        {
            navMeshAgent.speed = speed;
        }
        else
        {
            navMeshAgent.speed = 0;        
        }
    }

    private void Attack()
    {
        if (canUse)
        {
            navMeshAgent.speed = 0;
            canUse = false;
            canMove = false;
            animator.speed = 1 / attackSpeed;
            animator.SetTrigger("attack");
        }
    }

    public void ShootProjectile(string name)
    {
        GameObject projectileObj = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name);
        GameObject projectile = Instantiate(projectileObj, projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.right = Player.transform.position - this.transform.position;
        projectile.GetComponent<Projectile>().damage = damage;
        Destroy(projectile, 10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, hitRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, thresholdDistanceToPlayer);
    }


    public override void Die()
    {
        lootDropManager.DropLoot(this.transform.position);
        if (GameObject.Find("Brawl") != null)
        {
            Brawl brawl = GameObject.Find("Brawl").GetComponent<Brawl>();
            brawl.IncreaseKills();
        }
        Destroy(this.gameObject);
    }

    public void EndAction()
    {
        animator.speed = 1;
        if (behaviour == EnemyBehaviour.Melee)
        {
            canUse = true;
        }
        else
        {
            StartCoroutine(StartCanUseCooldown());
        }
        if (!IsStunned)
        {
            canMove = true;
        }
    }

    public void MeleeHit()
    {
        if (distanceToPlayer <= hitRange)
        {
            Player.TakeDamage(damage);
        }
    }

    private IEnumerator StartCanUseCooldown()
    {
        if (!canUse)
        {
            yield return new WaitForSeconds(useCooldown);
            canUse = true;
        }
    }
}