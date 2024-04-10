using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Variables - RANGED ENEMY")]
    public float useCooldown;
    [Header("Components - RANGED ENEMY")]
    public Transform projectileSpawnPoint;
    void Start()
    {
        base.Start();
    }
    public override void Behaviour()
    {
        navMeshAgent.destination = player.transform.position;
        if (distanceToPlayer <= hitRange && canUse)
        {
            Attack();
        }
        if (distanceToPlayer > thresholdDistanceToPlayer)
        {
            CanMove = true;
        }
        else
        {
            CanMove = false;
        }
    }

    public override void EndAction()
    {
        base.EndAction();
        StartCoroutine(StartCanUseCooldown());
    }

    private IEnumerator StartCanUseCooldown()
    {
        if (!canUse)
        {
            yield return new WaitForSeconds(useCooldown);
            canUse = true;
        }
    }

    public void ShootProjectile(string name)
    {
        GameObject projectileObj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Projectile_" + name), projectileSpawnPoint.position, Quaternion.identity);
        projectileObj.transform.right = player.transform.position - this.transform.position;
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.damage = damage;
        projectile.entity = this;
        Destroy(projectile, 10f);
    }
}
