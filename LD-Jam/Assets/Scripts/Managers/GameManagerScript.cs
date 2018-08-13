using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [Header("Shop Settings")]
    [Space]
    public int saveRoomPrice = 3;
    public int repairRoomPrice = 10;

    [Header("Room Service")]
    [Space]
    public float saveRoomSpeed = 0.5f;
    public float repairRoomSpeed = 0.2f;

    [Header("Spawn Settings")]
    [Space]
    [SerializeField]
    private int initialEnemies = 5;
    [SerializeField]
    private bool limitMaxEnemies = true;
    [SerializeField]
    private int maxEnemies = 10;
    [SerializeField]
    private float spawnTimer = 2;

    [Space]
    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject[] spawners;
    private GameObject[] enemies;
    private int numberOfSpawns;
    private int poolIndex;
    private int numberOfEnemies;

    private bool spawnerIsOn;

    [HideInInspector]
    public bool isPaused;

    public static GameManagerScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        poolIndex = PoolManagerScript.instance.PreCache(enemyPrefab);
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        numberOfSpawns = spawners.Length;

        StartCoroutine(Spawn());

        if (initialEnemies > 0)
        {
            SpawnInitialEnemies();
        }
    }

    private void SpawnInitialEnemies()
    {
        int currentSpawner = 0;

        for (int i = 0; i < initialEnemies; ++i)
        {
            if (currentSpawner > spawners.Length - 1)
            {
                currentSpawner = 0;
            }

            Transform spawner = spawners[currentSpawner].GetComponent<Transform>();
            //GameObject enemy = PoolManagerScript.instance.GetCachedPrefab(poolIndex);
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = spawner.transform.position;
            enemy.SetActive(true);

            ++currentSpawner;
        }

        UpdateNumberOfEnemies();
    }

    private IEnumerator Spawn()
    {
        spawnerIsOn = true; 

        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);

            Transform spawner = spawners[Random.Range(0, spawners.Length)].GetComponent<Transform>();
            //GameObject enemy = PoolManagerScript.instance.GetCachedPrefab(poolIndex);
            //*****************************************************************************
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = spawner.transform.position;
            enemy.SetActive(true);

            UpdateNumberOfEnemies();
        }
    }

    public void UpdateNumberOfEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        numberOfEnemies = enemies.Length;

        if (numberOfEnemies >= maxEnemies)
        {
            StopAllCoroutines();
            spawnerIsOn = false;
        }
        else if (!spawnerIsOn)
        {
            StartCoroutine(Spawn());
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        isPaused = true;
        UIManagerScript.instance.ShowGameOverScreen();
    }
}
