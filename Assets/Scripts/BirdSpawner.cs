using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("Variables")]
    public float sizeY;
    public float spawnDelay;
    public float destroyDelay;
    [Header("Component")]
    public GameObject[] birds;

    private WaitForSeconds spawnDelayWFS;
    void Start()
    {
        spawnDelayWFS = new WaitForSeconds(spawnDelay);
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
        {
            Spawn();
            yield return spawnDelayWFS;
        }
    }

    void Spawn()
    {
        GameObject randomBird = birds[Random.Range(0, birds.Length)];
        Vector2 randomPos = new Vector2(this.transform.position.x, Random.Range(this.transform.position.y + sizeY / 2, this.transform.position.y - sizeY / 2));
        GameObject bird = Instantiate(randomBird,randomPos,Quaternion.identity);
        Destroy(bird,destroyDelay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(0,this.transform.position.y + sizeY/2, this.transform.position.y - sizeY/2));
    }
}
