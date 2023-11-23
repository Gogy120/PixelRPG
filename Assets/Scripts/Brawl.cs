using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Brawl : MonoBehaviour
{
    [Header("Variables")]
    public int waves;
    public float enemySpawnDelay;
    public int enemyAmount;
    public float enemyAmountMultiplier;
    public GameObject[] enemies;
    public Transform[] spawnPositions;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI waveText;

    private enum BrawlState
    {
        IN_PROGRESS,
        PAUSE
    }

    private int currentWave = 0;
    private int currentWaveKills = 0;
    private Player player;
    private BrawlState brawlState = BrawlState.PAUSE;
    private WaitForSeconds enemySpawnDelayWFS;
    private WaitForSeconds waveCheckDelayWFS = new WaitForSeconds(5);
    private List<GameObject> currentEnemies = new List<GameObject>();
    void Start()
    {
        enemySpawnDelayWFS = new WaitForSeconds(enemySpawnDelay);
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(GameLoop());
    }
    private IEnumerator GameLoop()
    {
        WaitForSeconds secondDelayWFS = new WaitForSeconds(1);
        for (int i = 5; i > 0; i--)
        {
            player.chat.Print("Brawl starting in " + i + "...");
            yield return secondDelayWFS;
        }
        while (currentWave < waves)
        {
            if (AreAllEnemiesDead())
            {
                StartCoroutine(SendWave());
            }
            yield return waveCheckDelayWFS;
        }
    }
    private IEnumerator SendWave()
    {
        if (brawlState == BrawlState.PAUSE)
        {
            brawlState = BrawlState.IN_PROGRESS;
            currentEnemies.Clear();
            currentWaveKills = 0;
            currentWave++;
            enemyAmount = Mathf.RoundToInt(enemyAmount * enemyAmountMultiplier);
            killsText.text = "Enemies: " + currentWaveKills + "/" + enemyAmount;
            waveText.text = "Wave: " + currentWave;
            for (int i = 0; i < enemyAmount; i++)
            {
                GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity);
                enemy.GetComponent<Enemy>().Engage();
                currentEnemies.Add(enemy);
                yield return enemySpawnDelayWFS;
            }
        }
    }

    private bool AreAllEnemiesDead()
    {
        if (currentEnemies.Count == 0) { return true; }
        int deadEnemies = 0;
        foreach (GameObject enemy in currentEnemies)
        {
            if (enemy == null)
            {
                deadEnemies++;
            }
        }
        if (deadEnemies == enemyAmount)
        {
            brawlState = BrawlState.PAUSE;
            return true;
        }
        return false;
    }
    public void IncreaseKills()
    {
        currentWaveKills++;
        if (currentWaveKills == enemyAmount)
        {
            killsText.text = "Enemies: CLEARED";
        }
        else
        {
            killsText.text = "Enemies: " + currentWaveKills + "/" + enemyAmount;
        }
    }
}
