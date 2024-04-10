using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    void Start()
    {
        base.Start();
    }


    public override void Behaviour()
    {
        navMeshAgent.destination = player.transform.position;
        if (distanceToPlayer > thresholdDistanceToPlayer)
        {
            if (canUse) { CanMove = true; }
        }
        else
        {
            CanMove = false;
            if (distanceToPlayer <= hitRange && canUse)
            {
                Attack();
            }
        }
    }

    public void Hit()
    {
        if (distanceToPlayer <= hitRange)
        {
            player.TakeDamage(damage, this);
        }
    }

    public override void EndAction()
    {
        base.EndAction();
        canUse = true;
    }
}
