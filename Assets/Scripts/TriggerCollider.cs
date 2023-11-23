using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    [Header("Variables")]
    public Collider2D collider;
    public float triggerDelay;
    public int cycles;

    private WaitForSeconds? delay;
    private WaitForSeconds smallDelay = new WaitForSeconds(0.05f);

    private void Start()
    {
        delay = new WaitForSeconds(triggerDelay);
        StartCoroutine(TriggerLoop());
    }

    private IEnumerator TriggerLoop()
    {
        if (cycles == 0)
        {
            while (true)
            {
                StartCoroutine(Trigger());
                yield return delay;
            }
        }
        else
        {
            for (int i = 0; i < cycles; i++)
            {
                StartCoroutine(Trigger());
                yield return delay;
            }
        }
    }

    private IEnumerator Trigger()
    {
        collider.enabled = true;
        collider.offset = new Vector2(collider.offset.x * -1f, 0);
        yield return smallDelay;
        collider.enabled = false;
    }
}
