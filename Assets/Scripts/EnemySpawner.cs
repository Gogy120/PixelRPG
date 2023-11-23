using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    public GameObject enemyPrefab;
    public float respawnTime;
    public float checkDelay;

    private WaitForSeconds checkDelayWFS;
    private WaitForSeconds respawnDelayWFS;
    private bool loop = true;
    private GameObject? enemy = null;
    void Start()
    {
        checkDelayWFS = new WaitForSeconds(checkDelay);
        respawnDelayWFS = new WaitForSeconds(respawnTime);
        StartCoroutine(CheckEnemy());
    }

    private IEnumerator CheckEnemy()
    {
        while (loop)
        {
            if (!IsEnemyAlive()) { StartCoroutine(RespawnEnemy()); }
            yield return checkDelayWFS;
        }
    }

    private IEnumerator RespawnEnemy()
    {
        loop = false;
        enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        yield return respawnDelayWFS;
        loop = true;
        StartCoroutine(CheckEnemy());
    }

    private bool IsEnemyAlive()
    {
        if (enemy != null) { return true; }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 0.2f);
    }
}
