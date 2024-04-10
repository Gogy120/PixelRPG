using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Variables")]
    public ProjectileType type;
    public Vector2 dir;
    public float speed;
    public int damage;
    public bool destroyOnHit;
    public float randomOffset;
    public string sound;
    [Header("Variables-EXPLOSIVE")]
    public float radius;
    public GameObject effect;
    [Header("Variables-DOT")]
    public int ticks;
    public float tickSpeed;
    [Header("Variables-STUN")]
    public float stunDuration;
    [Header("Variables-SLOW")]
    public float slowDuration;
    public float slowMult;
    public Material slowMat;

    [HideInInspector] public Enemy entity;

    private void Start()
    {
        if (randomOffset != 0)
        {
            dir = new Vector2(dir.x, dir.y + Random.Range(-randomOffset, randomOffset));
        }
    }
    public enum ProjectileType
    {
        NORMAL,EXPLOSIVE,DOT,STUN,SLOW
    }

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
    public void Explode(Vector2 pos, float radius, int damage)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(pos, radius);
        foreach (Collider2D c in col)
        {
            if (c.GetComponent<Entity>() != null)
            {
                c.GetComponent<Entity>().TakeDamage(this.gameObject,damage);
            }
        }
        Instantiate(effect,pos,Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null) { Destroy(this.gameObject); }

    }
}
